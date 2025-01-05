using ClockInSync.Repositories.ClockInSync.Repositories;
using ClockInSync.Repositories.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ClockInSync.Repositories.Microsoft.DependencyInjection
{
    public static class Extensions
    {
        public static void AddClockInSyncRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IPunchClockRepository,PunchClockRepository>();
        }
    }
}
