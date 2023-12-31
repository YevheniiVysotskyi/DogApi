using AspNetCoreRateLimit;
using Dogs.API.Config;
using Dogs.API.Seed;
using Dogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Dogs.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
            builder.Services.AddDbContext<DogContext>(options =>
                options.UseSqlServer(connectionString).EnableSensitiveDataLogging());

            builder.Services.AddingOwnDI();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseIpRateLimiting();

            app.UseHttpsRedirection();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<DogContext>();

                dbContext.Database.EnsureCreated();

                var dogEntities = Seeding.Seed();

                if (!await dbContext.Dogs.AnyAsync())
                {
                    await dbContext.AddRangeAsync(dogEntities);
                }

                await dbContext.SaveChangesAsync();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/ping", async context =>
                {
                    await context.Response.WriteAsync("Dogshouseservice.Version1.0.1");
                });
                endpoints.MapControllers();
            });


            app.Run();
        }
    }
}