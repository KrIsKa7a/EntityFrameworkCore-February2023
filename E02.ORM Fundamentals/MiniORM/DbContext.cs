namespace MiniORM;

using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

using Microsoft.Data.SqlClient;

/// <summary>
/// User should derive from DbContext and then he will add wanted DbSet from entities.
/// We need to discover run-time added DbSets from user.
/// </summary>
public abstract class DbContext
{
    private readonly DatabaseConnection connection;
    private readonly IDictionary<Type, PropertyInfo> dbSetProperties;

    public DbContext(string connectionString)
    {
        this.connection = new DatabaseConnection(connectionString);
        this.dbSetProperties = this.DiscoverDbSets();
        using (new ConnectionManager(connection))
        {
            this.InitializeDbSets();
        }

        this.MapAllRelations();
    }

    internal static readonly Type[] AllowedSqlTypes =
    {
        typeof(string), //VARCHAR, NVARCHAR, CHAR, NCHAR
        typeof(int), // INT
        typeof(uint), // INT
        typeof(long), //BIGINT
        typeof(ulong), //BIGINT
        typeof(decimal), //DECIMAL
        typeof(bool), //BIT
        typeof(DateTime) //DATETIME, DATETIME2
    };

    public void SaveChanges()
    {
        object[] dbSets = this.dbSetProperties
            .Select(dbSetInfo => dbSetInfo.Value.GetValue(this))
            .ToArray();
        foreach (IEnumerable<object> dbSet in dbSets)
        {
            IEnumerable<object> invalidEntities = dbSet
                .Where(e => !this.IsObjectValid(e))
                .ToArray();
            if (invalidEntities.Any())
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.InvalidEntitiesException,
                    invalidEntities.Count(), dbSet.GetType().Name));
            }
        }

        using (new ConnectionManager(this.connection))
        {
            using SqlTransaction transaction = this.connection.StartTransaction();

            foreach (IEnumerable dbSet in dbSets)
            {
                Type dbSetType = dbSet.GetType().GetGenericArguments().First();
                MethodInfo persistMethod = typeof(DbContext)
                    .GetMethod("Persist", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(dbSetType);

                try
                {
                    persistMethod.Invoke(this, new object[] { dbSet });
                }
                catch (TargetInvocationException tie)
                {
                    // No need of rollback because Persist<T> method was never invoked!
                    throw tie.InnerException;
                }
                catch (InvalidOperationException)
                {
                    transaction.Rollback();
                    throw;
                }
                catch (SqlException)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            transaction.Commit();
        }
    }

    // We do not use this Validator in real projects!
    private bool IsObjectValid(object o)
    {
        ValidationContext validationContext = new ValidationContext(o);
        ICollection<ValidationResult> errors = new List<ValidationResult>();

        bool validationResult = Validator.TryValidateObject(o, validationContext, errors, true);
        return validationResult;
    }

    private void Persist<TEntity>(DbSet<TEntity> dbSet)
        where TEntity : class, new()
    {
        string tableName = this.GetTableName(typeof(TEntity));
        string[] columns = this.connection
            .FetchColumnNames(tableName)
            .ToArray();
        if (dbSet.ChangeTracker.Added.Any())
        {
            this.connection.InsertEntities(dbSet.ChangeTracker.Added, tableName, columns);
        }

        IEnumerable<TEntity> modifiedEntities = dbSet.ChangeTracker
            .GetModifiedEntities(dbSet)
            .ToArray();
        if (modifiedEntities.Any())
        {
            this.connection.UpdateEntities(modifiedEntities, tableName, columns);
        }

        if (dbSet.ChangeTracker.Removed.Any())
        {
            this.connection.DeleteEntities(dbSet.ChangeTracker.Removed, tableName, columns);
        }
    }

    private string GetTableName(Type tableType)
    {
        string tableName = tableType.GetCustomAttribute<TableAttribute>()?.Name;
        if (tableName == null)
        {
            tableName = this.dbSetProperties[tableType].Name;
        }

        return tableName;
    }

    private IDictionary<Type, PropertyInfo> DiscoverDbSets()
    {
        IDictionary<Type, PropertyInfo> dbSets = this.GetType()
            .GetProperties()
            .Where(pi => pi.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            .ToDictionary(pi => pi.PropertyType.GetGenericArguments().First(), pi => pi);
        return dbSets;
    }

    private void InitializeDbSets()
    {
        foreach (KeyValuePair<Type, PropertyInfo> dbSetInfo in this.dbSetProperties)
        {
            Type dbSetType = dbSetInfo.Key;
            PropertyInfo dbSetProperty = dbSetInfo.Value;

            MethodInfo populateDbSetMethod = typeof(DbContext)
                .GetMethod("PopulateDbSet", BindingFlags.Instance | BindingFlags.NonPublic)
                .MakeGenericMethod(dbSetType);
            populateDbSetMethod.Invoke(this, new object[] { dbSetProperty });
        }
    }

    private void PopulateDbSet<TEntity>(PropertyInfo dbSet)
        where TEntity : class, new()
    {
        IEnumerable<TEntity> entities = this.LoadTableEntities<TEntity>();
        DbSet<TEntity> dbSetInstance = new DbSet<TEntity>(entities);

        ReflectionHelper.ReplaceBackingField(this, dbSet.Name, dbSetInstance);
    }

    private IEnumerable<TEntity> LoadTableEntities<TEntity>() where TEntity : class, new()
    {
        Type entityType = typeof(TEntity);
        string[] columns = this.GetEntityColumnNames(entityType);
        string tableName = this.GetTableName(entityType);

        IEnumerable<TEntity> fetchedRows = this.connection
            .FetchResultSet<TEntity>(tableName, columns)
            .ToArray();
        return fetchedRows;
    }

    private string[] GetEntityColumnNames(Type entityType)
    {
        string tableName = this.GetTableName(entityType);
        string[] dbColumns = this.connection
            .FetchColumnNames(tableName)
            .ToArray();

        string[] columnsTaken = entityType
            .GetProperties()
            .Where(pi => dbColumns.Contains(pi.Name) &&
                         !pi.HasAttribute<NotMappedAttribute>() &&
                         DbContext.AllowedSqlTypes.Contains(pi.PropertyType))
            .Select(pi => pi.Name)
            .ToArray();
        return columnsTaken;
    }

    private void MapAllRelations()
    {
        foreach (KeyValuePair<Type, PropertyInfo> dbSetInfo in this.dbSetProperties)
        {
            Type dbSetEntityType = dbSetInfo.Key;
            object dbSetInstance = dbSetInfo.Value.GetValue(this);
            MethodInfo mapRelationsMethod = typeof(DbContext)
                .GetMethod("MapRelations", BindingFlags.Instance | BindingFlags.NonPublic)
                .MakeGenericMethod(dbSetEntityType);

            mapRelationsMethod.Invoke(this, new object[] { dbSetInstance });
        }
    }

    private void MapRelations<TEntity>(DbSet<TEntity> dbSet)
        where TEntity : class, new()
    {
        Type entityType = typeof(TEntity);
        this.MapNavigationProperties(dbSet);
        PropertyInfo[] collections = entityType
            .GetProperties()
            .Where(pi => pi.PropertyType.IsGenericType &&
                         pi.PropertyType.GetGenericTypeDefinition() == typeof(ICollection))
            .ToArray();
        foreach (PropertyInfo collection in collections)
        {
            Type collectionEntityType = collection.PropertyType
                .GetGenericArguments()
                .First();
            MethodInfo mapCollectionMethod = typeof(DbContext)
                .GetMethod("MapCollection", BindingFlags.Instance | BindingFlags.NonPublic)
                .MakeGenericMethod(entityType, collectionEntityType);

            mapCollectionMethod.Invoke(this, new object[] { dbSet, collection });
        }
    }

    private void MapNavigationProperties<TEntity>(DbSet<TEntity> dbSet) 
        where TEntity : class, new()
    {
        Type entityType = typeof(TEntity);

        PropertyInfo[] foreignKeys = entityType
            .GetProperties()
            .Where(pi => pi.HasAttribute<ForeignKeyAttribute>())
            .ToArray();
        foreach (PropertyInfo fk in foreignKeys)
        {
            string navigationPropertyName = fk
                .GetCustomAttribute<ForeignKeyAttribute>().Name;
            PropertyInfo navigationProperty = entityType
                .GetProperty(navigationPropertyName);

            IEnumerable<object> navigationDbSet = (IEnumerable<object>)this.dbSetProperties[navigationProperty.PropertyType]
                .GetValue(this);
            PropertyInfo navigationEntityPK = navigationProperty.PropertyType
                .GetProperties()
                .First(pi => pi.HasAttribute<KeyAttribute>());

            foreach (TEntity entity in dbSet)
            {
                object foreignKeyValue = fk.GetValue(entity);
                object navigationPropertyValue = navigationDbSet
                    .First(cnp => navigationEntityPK.GetValue(cnp).Equals(foreignKeyValue));

                navigationProperty.SetValue(entity, navigationPropertyValue);
            }
        }
    }

    private void MapCollection<TEntity, TCollection>(DbSet<TEntity> dbSet, PropertyInfo collection)
        where TEntity : class, new()
        where TCollection : class, new()
    {
        Type entityType = typeof(TEntity);
        Type collectionType = typeof(TCollection);

        PropertyInfo[] collectionTypePrimaryKeys = collectionType
            .GetProperties()
            .Where(pi => pi.HasAttribute<KeyAttribute>())
            .ToArray();

        PropertyInfo foreignKey = collectionTypePrimaryKeys.First();
        PropertyInfo primaryKey = entityType
            .GetProperties()
            .First(pi => pi.HasAttribute<KeyAttribute>());

        bool isManyToMany = collectionTypePrimaryKeys.Length >= 2;
        if (isManyToMany)
        {
            foreignKey = collectionType
                .GetProperties()
                .First(pi => collectionType
                    .GetProperty(pi.GetCustomAttribute<ForeignKeyAttribute>().Name)
                    .PropertyType == entityType);
        }

        DbSet<TCollection> navigationDbSet = (DbSet<TCollection>)this.dbSetProperties[collectionType]
            .GetValue(this);
        foreach (TEntity entity in dbSet)
        {
            object primaryKeyValue = primaryKey.GetValue(entity);
            IEnumerable<TCollection> navigationEntities = navigationDbSet
                .Where(ne => foreignKey.GetValue(ne).Equals(primaryKeyValue))
                .ToArray();
            ReflectionHelper.ReplaceBackingField(entity, collection.Name, navigationEntities);
        }
    }
}
