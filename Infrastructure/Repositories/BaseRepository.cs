
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;

        public BaseRepository(DbContext context) 
        {
            _context = context;
        }

        // CREATE
        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            try
            {
                await _context.Set<TEntity>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
                return null;
            }
        }
        // GET ALL
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await _context.Set<TEntity>().ToListAsync();
            }
            
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " +  ex.Message);
                return null;
            }
        }

        // GET ONE
        public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
                return null;
            }
        }
        // UPDATE
        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            try
            {
                _context.Set <TEntity>().Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
                return false;
            }
        }
        // DELETE
        public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entityToDelete = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
                if (entityToDelete != null)
                {
                    _context.Set<TEntity>().Remove(entityToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
                return false;
            }
        }
        // EXISTS - Check if an entity meets a specific condition exists in the database.
        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await _context.Set<TEntity>().AnyAsync(predicate);
            }
            catch (Exception ex) 
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
                return false;
            }
        }
    }
    
}