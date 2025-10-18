// Appointments/Validators/CreateAppointmentValidator.cs
using FluentValidation;
using Booking.Application.DTOs;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentRequest>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.ServiceId).NotEmpty();
        RuleFor(x => x.StartUtc).NotEqual(default(DateTime));
    }
}
