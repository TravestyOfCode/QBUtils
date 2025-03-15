using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace QBUtils.Data.Services;

public static class RegisterDataServices
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connectionString))
            .AddDefaultIdentity<AppUser>()
            .AddRoles<IdentityRole>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppDbContext>();

        services.AddMediatR(config => config.RegisterServicesFromAssembly(System.Reflection.Assembly.GetExecutingAssembly()));

        return services;
    }
}
