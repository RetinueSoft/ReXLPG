using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReXLPgDA.Repos
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);
        IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetByQuery(Expression<Func<T, bool>> predicate);
        T GetFirst(Expression<Func<T, bool>> predicate, bool withDefault = true);
        DbSet<T> DbSet{ get; }
        bool Exists(Expression<Func<T, bool>> predicate);
        void Insert(T entity);
        void Update(T oldEntity, T newEntity);
        void Delete(Guid id);
        void Delete(T entity);
        void Delete(List<T> entityList);
        void Save();
    }
}
