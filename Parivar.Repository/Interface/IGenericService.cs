using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace Parivar.Repository.Interface
{
    public interface IGenericService<T> where T : class
    {
        T GetSingle(Expression<Func<T, bool>> predicate, bool asNoTracking = false, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetAll(bool asNoTracking = false);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate, bool asNoTracking = false, params Expression<Func<T, object>>[] includePropertie);
        T GetById(object id);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, bool asNoTracking = false);
        int GetCount(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        void Add(T entity, long? userId = null);
        void AddAsync(T entity, long? userId = null);
        void AddRange(IEnumerable<T> entity, long? userId = null);
        void AddRangeAsync(IEnumerable<T> entity, long? userId = null);
        void Update(T entity, long? userId = null);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entity);
        void Save();
        void Detach(T entity);
        Task SaveAsync();
    }
}
