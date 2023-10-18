using Dogs.Domain.Entity;
using Dogs.Infrastructure.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockUnitTest
{
    public class Factory
    {
        public static Mock<IDogRepository> CreateDogRepositoryMock()
        {
            var dogRepositoryMock = new Mock<IDogRepository>();
            return dogRepositoryMock;
        }

        public static Dog CreateDogEntity(int dogId, string name, string color, int tailLength, int weight)
        {
            return new Dog
            {
                Id = dogId,
                Name = name,
                Color = color,
                TailLength = tailLength,
                Weight = weight
            };
        }
    }
}
