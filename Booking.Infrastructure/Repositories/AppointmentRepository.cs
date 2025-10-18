// Repositories/AppointmentRepository.cs
using Microsoft.EntityFrameworkCore;
using Booking.Application.Contracts;
using Booking.Domain.Entities;
using Booking.Domain.Enums;
using Booking.Infrastructure.Persistence;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _ctx;
    public AppointmentRepository(AppDbContext ctx) { _ctx = ctx; }

    public async Task<bool> ExistsOverlapAsync(Guid serviceId, DateTime startUtc, DateTime endUtc, Guid? excludeId, CancellationToken ct)
    {
        return await _ctx.Appointments
            .Where(a => a.ServiceId == serviceId && a.Status != AppointmentStatus.Cancelled)
            .Where(a => excludeId == null || a.Id != excludeId)
            .AnyAsync(a => startUtc < a.EndUtc && endUtc > a.StartUtc, ct);
    }

    public Task<Appointment?> GetByIdAsync(Guid id, CancellationToken ct)
        => _ctx.Appointments.FirstOrDefaultAsync(a => a.Id == id, ct);

    public Task<List<Appointment>> GetByDayAsync(DateTime dayUtc, CancellationToken ct)
    {
        var next = dayUtc.AddDays(1);
        return _ctx.Appointments
            .Where(a => a.StartUtc >= dayUtc && a.StartUtc < next)
            .OrderBy(a => a.StartUtc)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(Appointment a, CancellationToken ct) { _ctx.Add(a); await _ctx.SaveChangesAsync(ct); }
    public async Task UpdateAsync(Appointment a, CancellationToken ct) { _ctx.Update(a); await _ctx.SaveChangesAsync(ct); }
}
