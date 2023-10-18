using AspNetCoreRateLimit;
using Dogs.Application.Interfaces;
using Dogs.Application.Services;
using Dogs.Infrastructure.Context;
using Dogs.Infrastructure.Interfaces;
using Dogs.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Dogs.API.Config
{
    public static class Config
    {
        public static IServiceCollection AddingOwnDI(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<DogContext>();
            services.AddScoped<IDogRepository, DogRepository>();
            services.AddScoped<IDogService, DogService>();
            services.RateLimits();

            return services;
        }

        public static IServiceCollection RateLimits(this IServiceCollection services)
        {
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-IP";
                options.ClientIdHeader = "X-ClientId";
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "1s",
                        Limit = 25
                    }
                };
            });
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddInMemoryRateLimiting();

            return services;
        }
    }
}
