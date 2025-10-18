using MediatR;
using Booking.Application.Contracts;

public class AttendAppointmentHandler : IRequestHandler<AttendAppointmentCommand, bool>
{
    private readonly IAppointmentRepository _repo;
    private readonly IUnitOfWork _uow;

    public AttendAppointmentHandler(IAppointmentRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<bool> Handle(AttendAppointmentCommand cmd, CancellationToken ct)
    {
        var appt = await _repo.GetByIdAsync(cmd.Id, ct);
        if (appt is null) return false;

        appt.Attend();
        await _repo.UpdateAsync(appt, ct);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
