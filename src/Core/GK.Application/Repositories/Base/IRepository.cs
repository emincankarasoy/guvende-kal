using GK.Domain.Entities.Base;
using System.Linq.Expressions;

namespace GK.Application.Repositories.Base
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        public Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool disableTracking = false,
            bool includeDeleted = false,
            params Expression<Func<TEntity, object>>[] includes);

        public Task<List<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            int index = 1, int size = 10,
            bool disableTracking = false,
            bool includeDeleted = false,
            params Expression<Func<TEntity, object>>[] includes);

        public Task<int> CountAsync();

        public Task<Guid> InsertAsync(TEntity entity, Guid UserID);

        public Task<ICollection<Guid>> InsertRangeAsync(IEnumerable<TEntity> entities, Guid UserID);

        public Task<Guid> UpdateAsync(TEntity entity, Guid UserID);

        public Task<ICollection<Guid>> UpdateRangeAsync(IEnumerable<TEntity> entities, Guid UserID);

        public Task<Guid> DeleteAsync(TEntity entity, Guid UserID);

        public Task<ICollection<Guid>> DeleteRangeAsync(IEnumerable<TEntity> entities, Guid UserID);
    }
}
