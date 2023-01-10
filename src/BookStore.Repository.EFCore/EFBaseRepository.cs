using BookStore.Domain.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository.EFCore
{
    public class EFBaseRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        readonly BookStoreDbContext _context;
        readonly DbSet<TEntity> _dbSet;

        public EFBaseRepository(BookStoreDbContext dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<TEntity>();
        }

        public void Delete(TEntity entityToDelete)
        {
            if (entityToDelete == null) throw new ArgumentNullException(nameof(entityToDelete));

            _dbSet.Remove(entityToDelete);
        }

        public void Delete(TKey uid)
        {
            var entityToDelete = GetByID(uid);

            if (entityToDelete == null)
                return;

            _dbSet.Remove(entityToDelete);
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null)
        {
            if (filter == null)
                return _dbSet;
            else
                return _dbSet.Where(filter);
        }

        public TEntity? GetByID(TKey uid)
        {
            return _dbSet.Find(uid);
        }

        public void Insert(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _dbSet.Add(entity);
        }

        public void Update(TEntity entityToUpdate)
        {
            if (entityToUpdate == null)
                throw new ArgumentNullException(nameof(entityToUpdate));

            _dbSet.Update(entityToUpdate);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
