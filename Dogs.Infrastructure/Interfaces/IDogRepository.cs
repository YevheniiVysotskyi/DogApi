using Dogs.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dogs.Infrastructure.Interfaces
{
    public interface IDogRepository
    {
        Task SaveChangesAsync();
        Task<Dog> GetByName(string name);

        Task<Dog> GetByNameNoTracking(string name);

        Task AddAsync(Dog entity);
        void Delete(Dog entity);

        Task<Dog> GetEntityAsync(int id);

        void Update(Dog entity);

        Task<ICollection<Dog>> GetAllAsync();

        Task<IEnumerable<Dog>> TakeAsync(int skipElements, int takeElements, (Expression<Func<Dog, object>> expression, bool ascending) sortOrder);
    }
}
