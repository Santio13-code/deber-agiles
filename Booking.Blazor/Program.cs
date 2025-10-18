using Booking.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Booking.Blazor.Services;  // <— importa tu namespace

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Apunta a TU API (usa el puerto real que te muestra Booking.Api al correr)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5081") });

// registra tu cliente
builder.Services.AddScoped<AppointmentClient>();   // <— ESTA ES LA CLAVE

builder.Services.AddScoped<ClientsClient>();
builder.Services.AddScoped<ServicesClient>();

await builder.Build().RunAsync();

