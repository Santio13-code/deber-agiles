// Repositories/ServiceRepository.cs
using Microsoft.EntityFrameworkCore;
using Booking.Application.Contracts;
using Booking.Infrastructure.Persistence;

public class ServiceRepository : IServiceRepository
{
    private readonly AppDbContext _ctx;
    public ServiceRepository(AppDbContext ctx) { _ctx = ctx; }

    public async Task<(int durationMin, bool exists)> GetDurationAsync(Guid serviceId, CancellationToken ct)
    {
        var d = await _ctx.Services.Where(s => s.Id == serviceId).Select(s => (int?)s.DurationMin).FirstOrDefaultAsync(ct);
        return d is null ? (0, false) : (d.Value, true);
    }
}
