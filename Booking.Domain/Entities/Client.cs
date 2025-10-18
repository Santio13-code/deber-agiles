// Entities/Client.cs
namespace Booking.Domain.Entities;
public class Client
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }

    public Client(string name, string? email = null, string? phone = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");
        Name = name; Email = email; Phone = phone;
    }
}
