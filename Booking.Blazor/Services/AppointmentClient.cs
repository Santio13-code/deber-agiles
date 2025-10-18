using System.Net.Http.Json;   // <- ESTE
using Booking.Blazor.Models;

namespace Booking.Blazor.Services
{
    public class AppointmentClient
    {
        private readonly HttpClient _http;
        public AppointmentClient(HttpClient http) => _http = http;

        public Task<List<AppointmentVm>?> GetByDayAsync(DateTime dayUtc)
            => _http.GetFromJsonAsync<List<AppointmentVm>>($"/api/appointments/day?dayUtc={dayUtc:O}");

        public async Task<AppointmentVm?> CreateAsync(CreateAppointmentVm vm)
        {
            var resp = await _http.PostAsJsonAsync("/api/appointments", vm);
            return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<AppointmentVm>() : null;
        }

        public Task<HttpResponseMessage> RescheduleAsync(Guid id, DateTime newStartUtc)
            => _http.PutAsync($"/api/appointments/{id}/reschedule?newStartUtc={newStartUtc:O}", null);

        public Task<HttpResponseMessage> CancelAsync(Guid id)
            => _http.PutAsync($"/api/appointments/{id}/cancel", null);

        public Task<HttpResponseMessage> AttendAsync(Guid id)
            => _http.PutAsync($"/api/appointments/{id}/attend", null);
    }
}
