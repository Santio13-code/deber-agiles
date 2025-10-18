using System.Net.Http.Json;
using Booking.Blazor.Models;

namespace Booking.Blazor.Services;

public class ServicesClient
{
    private readonly HttpClient _http;
    public ServicesClient(HttpClient http) => _http = http;

    public Task<List<ServiceVm>?> GetAllAsync()
        => _http.GetFromJsonAsync<List<ServiceVm>>("/api/services");

    public async Task<ServiceVm?> CreateAsync(CreateServiceVm vm)
    {
        var resp = await _http.PostAsJsonAsync("/api/services", vm);
        return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<ServiceVm>() : null;
    }
}