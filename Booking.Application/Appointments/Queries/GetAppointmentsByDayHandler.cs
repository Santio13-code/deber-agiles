// Appointments/Queries/GetAppointmentsByDayHandler.cs
using MediatR;
using AutoMapper;
using Booking.Application.DTOs;
using Booking.Application.Contracts;

public class GetAppointmentsByDayHandler : IRequestHandler<GetAppointmentsByDayQuery, List<AppointmentDto>>
{
    private readonly IAppointmentRepository _repo;
    private readonly IMapper _mapper;
    public GetAppointmentsByDayHandler(IAppointmentRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<List<AppointmentDto>> Handle(GetAppointmentsByDayQuery q, CancellationToken ct)
    {
        var day = DateTime.SpecifyKind(q.DayUtc.Date, DateTimeKind.Utc);
        var list = await _repo.GetByDayAsync(day, ct);
        return list.Select(_mapper.Map<AppointmentDto>).ToList();
    }
}
