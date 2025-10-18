namespace Booking.Blazor.Models;
public record ServiceVm(Guid Id, string Name, int DurationMin, bool Active);
public record CreateServiceVm(string Name, int DurationMin);
