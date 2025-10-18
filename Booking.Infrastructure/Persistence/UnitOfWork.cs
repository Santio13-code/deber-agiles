// Persistence/UnitOfWork.cs
using Booking.Application.Contracts;

namespace Booking.Infrastructure.Persistence;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _ctx;
    public IServiceRepository Services { get; }
    public UnitOfWork(AppDbContext ctx, IServiceRepository services)
    { _ctx = ctx; Services = services; }
    public Task<int> SaveChangesAsync(CancellationToken ct) => _ctx.SaveChangesAsync(ct);
}
