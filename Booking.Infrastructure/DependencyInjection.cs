// DependencyInjection.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Booking.Application.Contracts;
using Booking.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string conn)
    {
        services.AddDbContext<AppDbContext>(o => o.UseSqlServer(conn));
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
