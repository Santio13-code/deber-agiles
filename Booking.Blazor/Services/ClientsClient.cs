using System.Net.Http.Json;
using Booking.Blazor.Models;

namespace Booking.Blazor.Services;

public class ClientsClient
{
    private readonly HttpClient _http;
    public ClientsClient(HttpClient http) => _http = http;

    public Task<List<ClientVm>?> GetAllAsync()
        => _http.GetFromJsonAsync<List<ClientVm>>("/api/clients");

    public async Task<ClientVm?> CreateAsync(CreateClientVm vm)
    {
        var resp = await _http.PostAsJsonAsync("/api/clients", vm);
        return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<ClientVm>() : null;
    }
}
