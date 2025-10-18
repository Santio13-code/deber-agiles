// Entities/Appointment.cs
using Booking.Domain.Enums;

namespace Booking.Domain.Entities;
public class Appointment
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ClientId { get; private set; }
    public Guid ServiceId { get; private set; }
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public AppointmentStatus Status { get; private set; } = AppointmentStatus.Pending;
    public string? Notes { get; private set; }

    private Appointment() { } // EF
    public Appointment(Guid clientId, Guid serviceId, DateTime startUtc, int durationMin, string? notes = null)
    {
        if (durationMin <= 0) throw new ArgumentException("Invalid duration");
        ClientId = clientId; ServiceId = serviceId; StartUtc = startUtc.ToUniversalTime();
        EndUtc = StartUtc.AddMinutes(durationMin); Notes = notes;
    }

    public void Reschedule(DateTime newStartUtc, int durationMin)
    {
        if (Status != AppointmentStatus.Pending) throw new InvalidOperationException("Only pending can be rescheduled");
        StartUtc = newStartUtc.ToUniversalTime();
        EndUtc = StartUtc.AddMinutes(durationMin);
    }
    public void Cancel()  { if (Status == AppointmentStatus.Pending) Status = AppointmentStatus.Cancelled; }
    public void Attend()  { if (Status == AppointmentStatus.Pending) Status = AppointmentStatus.Attended; }
}
