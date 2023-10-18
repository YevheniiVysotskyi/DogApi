using Dogs.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dogs.Infrastructure.Context.Config
{
    public class DogConfig : IEntityTypeConfiguration<Dog>
    {
        public void Configure(EntityTypeBuilder<Dog> builder)
        {
            builder.ToTable("Dog");

            builder.HasKey(x => x.Id);

            builder.HasIndex(item => item.Name);
        }
    }
}
