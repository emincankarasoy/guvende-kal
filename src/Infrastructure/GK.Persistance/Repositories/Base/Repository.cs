using GK.Application.Repositories.Base;
using GK.Domain.Entities.Base;
using GK.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GK.Persistance.Repositories.Base
{
    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, bool disableTracking = false, bool includeDeleted = false, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = this.GenerateQueryable(
                            predicate: predicate,
                            disableTracking: disableTracking,
                            includeDeleted: includeDeleted,
                            includes: includes);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, int index = 1, int size = 10, bool disableTracking = false, bool includeDeleted = false, params Expression<Func<TEntity, object>>[] includes)
        {
            index = index < 1 ? 1 : index;
            size = size > 0 ? size : 10;

            IQueryable<TEntity> query = this.GenerateQueryable(
                predicate: predicate,
                disableTracking: disableTracking,
                includeDeleted: includeDeleted,
                includes: includes);

            return await query.Skip((index - 1) * size).Take(size).ToListAsync<TEntity>();
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<Guid> InsertAsync(TEntity entity, Guid UserID)
        {
            try
            {
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }

                entity.IsActive = true;
                entity.IsDeleted = false;
                entity.CreatedUserId = UserID;
                entity.UpdatedUserId = UserID;
                entity.CreatedDateTime = DateTime.UtcNow;
                entity.UpdatedDateTime = DateTime.UtcNow;

                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();

                return entity.Id;
            }
            catch
            {
            }

            return Guid.Empty;
        }

        public async Task<ICollection<Guid>> InsertRangeAsync(IEnumerable<TEntity> entities, Guid UserID)
        {
            try
            {
                foreach (TEntity entity in entities)
                {
                    entity.Id = Guid.NewGuid();
                    entity.IsActive = true;
                    entity.IsDeleted = false;
                    entity.CreatedUserId = UserID;
                    entity.UpdatedUserId = UserID;
                    entity.CreatedDateTime = DateTime.UtcNow;
                    entity.UpdatedDateTime = DateTime.UtcNow;

                    await _dbSet.AddAsync(entity);
                }

                await _context.SaveChangesAsync();

                return entities.Select(x => x.Id).ToList();
            }
            catch
            {
            }

            return new List<Guid>();
        }

        public async Task<Guid> UpdateAsync(TEntity entity, Guid UserID)
        {
            try
            {
                entity.IsActive = true;
                entity.IsDeleted = false;
                entity.UpdatedUserId = UserID;
                entity.UpdatedDateTime = DateTime.UtcNow;

                _dbSet.Update(entity);
                await _context.SaveChangesAsync();

                return entity.Id;
            }
            catch
            {
            }

            return Guid.Empty;
        }

        public async Task<ICollection<Guid>> UpdateRangeAsync(IEnumerable<TEntity> entities, Guid UserID)
        {
            try
            {
                foreach (TEntity entity in entities)
                {
                    entity.IsActive = true;
                    entity.IsDeleted = false;
                    entity.UpdatedUserId = UserID;
                    entity.UpdatedDateTime = DateTime.UtcNow;
                }

                _dbSet.UpdateRange(entities);
                await _context.SaveChangesAsync();

                return entities.Select(x => x.Id).ToList();
            }
            catch 
            {
            }

            return new List<Guid>();
        }

        public async Task<Guid> DeleteAsync(TEntity entity, Guid UserID)
        {
            try
            {
                entity.IsActive = false;
                entity.IsDeleted = true;
                entity.UpdatedUserId = UserID;
                entity.UpdatedDateTime = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return entity.Id;
            }
            catch
            {
            }

            return Guid.Empty;
        }

        public async Task<ICollection<Guid>> DeleteRangeAsync(IEnumerable<TEntity> entities, Guid UserID)
        {
            try
            {
                foreach (TEntity entity in entities)
                {
                    entity.IsActive = false;
                    entity.IsDeleted = true;
                    entity.UpdatedUserId = UserID;
                    entity.UpdatedDateTime = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                return entities.Select(x => x.Id).ToList();
            }
            catch
            {
            }

            return new List<Guid>();
        }

        private IQueryable<TEntity> GenerateQueryable(
            bool disableTracking = false,
            bool includeDeleted = false,
            Expression<Func<TEntity, bool>>? predicate = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (!includeDeleted)
            {
                query = query.Where(x => x.IsActive && !x.IsDeleted);
            }

            return query.OrderByDescending(x => x.CreatedDateTime);
        }
    }
}
