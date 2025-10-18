// Entities/Service.cs
namespace Booking.Domain.Entities;
public class Service
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public int DurationMin { get; private set; }
    public bool Active { get; private set; } = true;

    public Service(string name, int durationMin)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");
        if (durationMin <= 0) throw new ArgumentException("Duration must be > 0");
        Name = name; DurationMin = durationMin;
    }

    public void Deactivate() => Active = false;
}
