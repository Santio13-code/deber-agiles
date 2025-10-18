// Appointments/Commands/CancelAppointmentHandler.cs
using MediatR;
using Booking.Application.Contracts;

public class CancelAppointmentHandler : IRequestHandler<CancelAppointmentCommand, bool>
{
    private readonly IAppointmentRepository _repo;
    private readonly IUnitOfWork _uow;
    public CancelAppointmentHandler(IAppointmentRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<bool> Handle(CancelAppointmentCommand cmd, CancellationToken ct)
    {
        var appt = await _repo.GetByIdAsync(cmd.Id, ct);
        if (appt is null) return false;
        appt.Cancel();
        await _repo.UpdateAsync(appt, ct);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
