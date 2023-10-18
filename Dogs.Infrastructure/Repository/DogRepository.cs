using Dogs.Domain.Entity;
using Dogs.Infrastructure.Context;
using Dogs.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dogs.Infrastructure.Repository
{
    public class DogRepository : IDogRepository
    {
        private readonly DogContext _dbContext;

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public DogRepository(DogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Dog entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public void Delete(Dog entity)
        {
            _dbContext.Remove(entity);
        }

        public async Task<ICollection<Dog>> GetAllAsync()
        {
            return await _dbContext.Set<Dog>().ToListAsync();
        }

        public async Task<Dog> GetByName(string name)
        {
            return await _dbContext.Set<Dog>().
                Where(item => item.Name.Equals(name)).
                FirstOrDefaultAsync();
        }

        public async Task<Dog> GetEntityAsync(int id)
        {
            return await _dbContext.Set<Dog>()
                .FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<Dog> GetByNameNoTracking(string name)
        {
            return await _dbContext.Set<Dog>().
                AsNoTracking().
                Where(item => item.Name.Equals(name)).
                FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Dog>> TakeAsync(int skipElements, 
            int takeElements, 
            (Expression<Func<Dog, object>> expression, bool ascending) sortOrder)
        {
            var query = _dbContext.Set<Dog>().AsNoTracking();

            if (sortOrder.ascending)
            {
                query = query.OrderBy(sortOrder.expression);
            }
            else
            {
                query = query.OrderByDescending(sortOrder.expression);
            }

            return query.Skip(skipElements)
                .Take(takeElements)
                .AsEnumerable();
        }

        public void Update(Dog entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
