using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Domain
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task CreateAsync(T entity);
        Task<ICollection<T>> GetAllAsync();
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<T> GetAsync(Guid id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, Guid id);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(T entity);
    }
}
