using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoDemoApp.API.Data.Abstract;
using TodoDemoApp.API.Entity;

namespace TodoDemoApp.API.Data.Concrete
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected TodoAppDbContext _todoAppDbContext;

        public GenericRepository(TodoAppDbContext todoAppDbContext)
        {
            _todoAppDbContext = todoAppDbContext;
        }

        public async Task<List<TEntity>> GetEntitiesAsync(Expression<Func<TEntity, object>> include = null)
            => include == null ? await _todoAppDbContext.Set<TEntity>().ToListAsync().ConfigureAwait(false)
                               : await _todoAppDbContext.Set<TEntity>().Include(include).ToListAsync().ConfigureAwait(false);
        public async Task<TEntity> GetEntityAsync(Guid id, Expression<Func<TEntity, object>> include = null)
            => include == null ? await _todoAppDbContext.Set<TEntity>().FirstAsync(entity => entity.Id.Equals(id)).ConfigureAwait(false)
                               : await _todoAppDbContext.Set<TEntity>().Include(include).FirstAsync(entity => entity.Id.Equals(id)).ConfigureAwait(false);

        public async Task<Guid> AddAsync(TEntity entity)
        {
            var entry = await _todoAppDbContext.AddAsync(entity).ConfigureAwait(false);

            await _todoAppDbContext.SaveChangesAsync().ConfigureAwait(false);

            return (Guid)entry.Property("Id").CurrentValue;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _todoAppDbContext.Update(entity);

            await _todoAppDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _todoAppDbContext.Remove(entity);

            await _todoAppDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
