using Dogs.Application.DTO;
using Dogs.Application.Interfaces;
using Dogs.Domain.Entity;
using Dogs.Infrastructure.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Dogs.Application.Services
{
    public class DogService : IDogService
    {
        private readonly IDogRepository _dogRepository;

        public DogService(IDogRepository unitOfWork)
        {
            _dogRepository = unitOfWork;
        }

        public async Task<DogDTO> GetDogByIdAsync(int id)
        {
            var dogDb = await _dogRepository.GetEntityAsync(id);

            var dog = new DogDTO();

            if(dogDb != null)
            {
                dog.Name = dogDb.Name;
                dog.TailLength = dogDb.TailLength;
                dog.Color = dogDb.Color;
                dog.Weight = dogDb.Weight;

                return dog;
            }

            return default;
        }

        public async Task AddSync(DogDTO dog)
        {
            var dogDb = new Dog
            {
                Name = dog.Name,
                Color = dog.Color,
                TailLength = dog.TailLength,
                Weight = dog.Weight,
            };

            await _dogRepository.AddAsync(dogDb);
            await _dogRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var dog = await _dogRepository.GetEntityAsync(id);

            if(dog != null)
            {
                _dogRepository.Delete(dog);

                await _dogRepository.SaveChangesAsync();

                dog = await _dogRepository.GetEntityAsync(id);

                if(dog == null)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task UpdateAsync(int id, DogDTO dTO)
        {
            var dog = await _dogRepository.GetEntityAsync(id);

            if(dog != null)
            {
                dog.Color = dTO.Color;
                dog.Name = dTO.Name;
                dog.Color = dTO.Color;
                dog.TailLength = dTO.TailLength;
                dog.Weight = dTO.Weight;

                _dogRepository.Update(dog);

                await _dogRepository.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }

        public async Task<List<DogDTO>> GetAllDogsAsync(int pageNumber, int pageSize, string attribute = "", string order = "asc")
        {
            int skipElements = (pageNumber - 1) * pageSize;

            Expression<Func<Dog, object>> orderByExpression = GetOrderProperty(attribute);

            bool ordering = order == "asc" ? true : false;

            var dogsDb = await _dogRepository
                .TakeAsync(skipElements, pageSize, (orderByExpression, ordering));
            List<DogDTO> dogs = new List<DogDTO>();

            if(dogsDb.Any())
            {
                foreach (var item in dogsDb.ToList())
                {
                    dogs.Add(new DogDTO
                    {
                        Color = item.Color,
                        Name = item.Name,
                        TailLength = item.TailLength,
                        Weight = item.Weight
                    });
               }
            }

            return dogs;
        }

        public async Task<Dog> GetDogByNameAsync(string name)
        {
            var dogRep = _dogRepository as IDogRepository;

            var dbDog = await dogRep.GetByNameNoTracking(name);

            return dbDog;
        }

        private Expression<Func<Dog, object>> GetOrderProperty(string attribute)
        {
            Expression<Func<Dog, object>> orderByExpression = null;

            switch (attribute.ToLower())
            {
                case "color":
                    orderByExpression = dog => dog.Color;
                    break;
                case "name":
                    orderByExpression = dog => dog.Name;
                    break;
                case "taillength":
                    orderByExpression = dog => dog.TailLength;
                    break;
                case "weight":
                    orderByExpression = dog => dog.Weight;
                    break;
                default:
                    orderByExpression = dog => dog.Name;
                    break;
            }

            return orderByExpression;
        }
    }
}
