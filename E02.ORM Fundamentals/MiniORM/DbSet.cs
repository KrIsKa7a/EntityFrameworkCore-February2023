namespace MiniORM;

using System.Collections;

/// <summary>
/// DbSet represents each table of the database. Entity class acts as a column description of the table.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class DbSet<TEntity> : ICollection<TEntity>
    where TEntity : class, new()
{
    internal DbSet(IEnumerable<TEntity> entities)
    {
        this.Entities = entities.ToList();
        this.ChangeTracker = new ChangeTracker<TEntity>(entities);
    }

    internal ChangeTracker<TEntity> ChangeTracker { get; set; }

    internal IList<TEntity> Entities { get; set; }

    public int Count
        => this.Entities.Count;

    public bool IsReadOnly
        => this.Entities.IsReadOnly;

    public void Add(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), ExceptionMessages.EntityNullException);
        }

        this.Entities.Add(entity);
        this.ChangeTracker.Add(entity); // Log added entity
    }

    public bool Remove(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), ExceptionMessages.EntityNullException);
        }

        bool isRemoved = this.Entities.Remove(entity);
        if (isRemoved)
        {
            this.ChangeTracker.Remove(entity);
        }

        return isRemoved;
    }

    public void RemoveRange(IEnumerable<TEntity> entitiesToRemove)
    {
        foreach (TEntity entity in entitiesToRemove)
        {
            this.Remove(entity);
        }
    }

    public void Clear()
    {
        while (this.Entities.Any())
        {
            TEntity entityToRemove = this.Entities.First();
            this.Remove(entityToRemove);
        }
    }

    public bool Contains(TEntity item)
        => this.Entities.Contains(item);

    // We will not use this method anywhere but it is derived from ICollection<T>
    public void CopyTo(TEntity[] array, int arrayIndex)
        => this.Entities.CopyTo(array, arrayIndex);

    // This will allow our DbSet<TEntity> to work with ForEach loop
    public IEnumerator<TEntity> GetEnumerator()
        => this.Entities.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();
}
