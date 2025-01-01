using ClockInSync.Repositories.Microsoft.DependencyInjection;
using ClockInSync.Services.PasswordManagementService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ClockInSync.Services.Microsoft.DependencyInjection
{
    public static class Extensions
    {
        public static IServiceCollection AddClockInSyncServices(this IServiceCollection services)
        {
            services.AddClockInSyncRepositories();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IPasswordService,PasswordService>();
            return services;
        }
    }

}
