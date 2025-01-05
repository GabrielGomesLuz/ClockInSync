using ClockInSync.Repositories.Microsoft.DependencyInjection;
using ClockInSync.Services.PunchClockServices;
using ClockInSync.Services.TokenServices;
using Microsoft.Extensions.DependencyInjection;

namespace ClockInSync.Services.Microsoft.DependencyInjection
{
    public static class Extensions
    {
        public static IServiceCollection AddClockInSyncServices(this IServiceCollection services)
        {
            services.AddClockInSyncRepositories();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPunchClockService, PunchClockService>();
            return services;
        }
    }

}
