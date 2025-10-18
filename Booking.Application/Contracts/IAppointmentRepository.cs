// Contracts/IAppointmentRepository.cs
using Booking.Domain.Entities;
namespace Booking.Application.Contracts;
public interface IAppointmentRepository
{
    Task<bool> ExistsOverlapAsync(Guid serviceId, DateTime startUtc, DateTime endUtc, Guid? excludeId, CancellationToken ct);
    Task<Appointment?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<Appointment>> GetByDayAsync(DateTime dayUtc, CancellationToken ct);
    Task AddAsync(Appointment a, CancellationToken ct);
    Task UpdateAsync(Appointment a, CancellationToken ct);
}
