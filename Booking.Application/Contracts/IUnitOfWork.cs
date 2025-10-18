// Contracts/IUnitOfWork.cs
namespace Booking.Application.Contracts;
public interface IUnitOfWork
{
    IServiceRepository Services { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}

public interface IServiceRepository
{
    Task<(int durationMin, bool exists)> GetDurationAsync(Guid serviceId, CancellationToken ct);
}
