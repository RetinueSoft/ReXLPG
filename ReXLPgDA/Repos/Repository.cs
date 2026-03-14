using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ReXLPgDA.Repos
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(Guid id)
        {
            return _dbSet.Find(id);
        }
        public T GetFirst(Expression<Func<T, bool>> predicate, bool withDefault = false)
        {
            return withDefault ? _dbSet.FirstOrDefault(predicate) : _dbSet.First(predicate);
        }
        public IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
        public IQueryable<T> GetByQuery(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }        
        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }
        public void Insert(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }
        public void Update(T oldEntity, T newEntity)
        {
            if (oldEntity == null || newEntity == null) return;

            _context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
            _context.Entry(oldEntity).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void Delete(Guid id)
        {
            Delete(_dbSet.Find(id));
        }
        public void Delete(T entity)
        {
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }
        public void Delete(List<T> entityList)
        {
            if (entityList != null)
            {
                _dbSet.RemoveRange(entityList);
            }
        }
        public DbSet<T> DbSet { get { return _dbSet; } }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
