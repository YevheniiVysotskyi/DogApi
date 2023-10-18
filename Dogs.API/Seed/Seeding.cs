using Dogs.Domain.Entity;

namespace Dogs.API.Seed
{
    public static class Seeding
    {
        public static List<Dog> Seed()
        {
            var dogs = new List<Dog>()
            {
                new Dog
                {
                    Name = "Neo",
                    Color = "red & amber",
                    TailLength = 22,
                    Weight = 32,
                },
                new Dog
                {
                    Name = "Jessy",
                    Color = "black & white",
                    TailLength = 7,
                    Weight = 14,
                },
            };

            return dogs;
        }
    }
}
