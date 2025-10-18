// Appointments/Commands/CreateAppointmentHandler.cs
using MediatR;
using AutoMapper;
using FluentValidation;
using Booking.Application.Contracts;
using Booking.Application.DTOs;
using Booking.Domain.Entities;

public class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, AppointmentDto>
{
    private readonly IAppointmentRepository _repo;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateAppointmentRequest> _validator;
    private readonly IUnitOfWork _uow;

    public CreateAppointmentHandler(IAppointmentRepository repo, IMapper mapper,
        IValidator<CreateAppointmentRequest> validator, IUnitOfWork uow)
    { _repo = repo; _mapper = mapper; _validator = validator; _uow = uow; }

    public async Task<AppointmentDto> Handle(CreateAppointmentCommand cmd, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(cmd.Request, ct);

        // duración viene de Service (lo resolvemos en Infra vía repo de Services)
        var (duration, exists) = await _uow.Services.GetDurationAsync(cmd.Request.ServiceId, ct);
        if (!exists) throw new InvalidOperationException("Service not found");

        var start = cmd.Request.StartUtc.ToUniversalTime();
        var end = start.AddMinutes(duration);

        var overlap = await _repo.ExistsOverlapAsync(cmd.Request.ServiceId, start, end, null, ct);
        if (overlap) throw new InvalidOperationException("Horario no disponible");

        var a = new Appointment(cmd.Request.ClientId, cmd.Request.ServiceId, start, duration, cmd.Request.Notes);
        await _repo.AddAsync(a, ct);
        await _uow.SaveChangesAsync(ct);

        return _mapper.Map<AppointmentDto>(a);
    }
}
