namespace MiniORM;

using System.ComponentModel.DataAnnotations;
using System.Reflection;

internal class ChangeTracker<T>
    where T : class, new()
{
    private readonly IList<T> allEntities; // Tracks updates of the entities
    private readonly IList<T> added; // Tracks added entities (to be added)
    private readonly IList<T> removed; // Tracks removed entities (to be removed)

    private ChangeTracker()
    {
        this.added = new List<T>();
        this.removed = new List<T>();
    }

    public ChangeTracker(IEnumerable<T> allEntities)
        : this()
    {
        this.allEntities = this.CloneEntities(allEntities);
    }

    public IReadOnlyCollection<T> AllEntities
        => (IReadOnlyCollection<T>)this.allEntities;

    public IReadOnlyCollection<T> Added
        => (IReadOnlyCollection<T>)this.added;

    public IReadOnlyCollection<T> Removed
        => (IReadOnlyCollection<T>)this.removed;

    public void Add(T entity)
        => this.added.Add(entity);

    public void Remove(T entity)
        => this.removed.Add(entity);

    public IEnumerable<T> GetModifiedEntities(DbSet<T> dbSet)
    {
        IList<T> modifiedEntities = new List<T>();
        PropertyInfo[] primaryKeys = typeof(T)
            .GetProperties()
            .Where(pi => pi.HasAttribute<KeyAttribute>())
            .ToArray();

        foreach (T proxyEntity in this.AllEntities)
        {
            object[] primaryKeyValues = this.GetPrimaryKeyValues(primaryKeys, proxyEntity)
                .ToArray();
            // Original originalEntity
            T entity = dbSet.Entities
                .FirstOrDefault(e => this.GetPrimaryKeyValues(primaryKeys, e).SequenceEqual(primaryKeyValues));
            if (entity == null)
            {
                continue;
            }

            bool isModified = this.IsModified(proxyEntity, entity);
            if (isModified)
            {
                modifiedEntities.Add(proxyEntity);
            }
        }

        return modifiedEntities;
    }

    private IEnumerable<object> GetPrimaryKeyValues(IEnumerable<PropertyInfo> primaryKeys, T proxyEntity)
    {
        return primaryKeys
            .Select(pk => pk.GetValue(proxyEntity));
    }

    private bool IsModified(T proxyEntity, T originalEntity)
    {
        PropertyInfo[] monitoredProperties = typeof(T)
            .GetProperties()
            .Where(pi => DbContext.AllowedSqlTypes.Contains(pi.PropertyType))
            .ToArray();
        PropertyInfo[] modifiedProperties = monitoredProperties
            .Where(pi => !Equals(pi.GetValue(proxyEntity), pi.GetValue(originalEntity)))
            .ToArray();

        return modifiedProperties.Any();
    }

    private IList<T> CloneEntities(IEnumerable<T> originalEntities)
    {
        IList<T> clonedEntities = new List<T>();
        PropertyInfo[] propertiesToClone = typeof(T)
            .GetProperties()
            .Where(pi => DbContext.AllowedSqlTypes.Contains(pi.PropertyType))
            .ToArray();

        foreach (T originalEntity in originalEntities)
        {
            T entityClone = Activator.CreateInstance<T>();
            foreach (PropertyInfo property in propertiesToClone)
            {
                object originalValue = property.GetValue(originalEntity);
                property.SetValue(entityClone, originalValue);
            }

            clonedEntities.Add(entityClone);
        }

        return clonedEntities;
    }
}