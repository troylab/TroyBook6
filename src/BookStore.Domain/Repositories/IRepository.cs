using System.Linq.Expressions;

namespace BookStore.Domain.Repositories;

public interface IRepository<TEntity, TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
{
    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null);

    TEntity? GetByID(TKey uid);

    void Insert(TEntity entity);

    void Update(TEntity entityToUpdate);

    void Delete(TEntity entityToDelete);

    void Delete(TKey uid);

    void SaveChanges();

    Task SaveChangesAsync();
}