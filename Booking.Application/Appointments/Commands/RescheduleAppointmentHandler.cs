// Appointments/Commands/RescheduleAppointmentHandler.cs
using MediatR;
using AutoMapper;
using Booking.Application.Contracts;
using Booking.Application.DTOs;

public class RescheduleAppointmentHandler : IRequestHandler<RescheduleAppointmentCommand, AppointmentDto>
{
    private readonly IAppointmentRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public RescheduleAppointmentHandler(IAppointmentRepository repo, IUnitOfWork uow, IMapper mapper)
    { _repo = repo; _uow = uow; _mapper = mapper; }

    public async Task<AppointmentDto> Handle(RescheduleAppointmentCommand cmd, CancellationToken ct)
    {
        var appt = await _repo.GetByIdAsync(cmd.Request.Id, ct) ?? throw new InvalidOperationException("Not found");
        var (duration, exists) = await _uow.Services.GetDurationAsync(appt.ServiceId, ct);
        if (!exists) throw new InvalidOperationException("Service not found");

        var newStart = cmd.Request.NewStartUtc.ToUniversalTime();
        var newEnd = newStart.AddMinutes(duration);

        var overlap = await _repo.ExistsOverlapAsync(appt.ServiceId, newStart, newEnd, appt.Id, ct);
        if (overlap) throw new InvalidOperationException("Horario no disponible");

        appt.Reschedule(newStart, duration);
        await _repo.UpdateAsync(appt, ct);
        await _uow.SaveChangesAsync(ct);

        return _mapper.Map<AppointmentDto>(appt);
    }
}
