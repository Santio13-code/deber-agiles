using MediatR;
using AutoMapper;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Booking.Application.DTOs;
using Booking.Infrastructure;
using Booking.Infrastructure.Persistence;
using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var cfg = builder.Configuration;

services.AddMediatR(typeof(AppointmentDto).Assembly);
services.AddAutoMapper(typeof(AppointmentDto).Assembly);
services.AddValidatorsFromAssembly(typeof(AppointmentDto).Assembly);

services.AddInfrastructure(cfg.GetConnectionString("Default")!);

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo{ Title="Booking API", Version="v1"}));
services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));



var app = builder.Build();
app.UseSwagger(); app.UseSwaggerUI(); 
app.UseCors();

// Endpoints básicos
app.MapGet("/api/appointments/day", async (DateTime dayUtc, IMediator med) =>
    await med.Send(new GetAppointmentsByDayQuery(dayUtc)));

app.MapPost("/api/appointments", async (CreateAppointmentRequest req, IMediator med) =>
    Results.Ok(await med.Send(new CreateAppointmentCommand(req))));

app.MapPut("/api/appointments/{id:guid}/reschedule", async (Guid id, DateTime newStartUtc, IMediator med) =>
    Results.Ok(await med.Send(new RescheduleAppointmentCommand(new(id, newStartUtc)))));

app.MapPut("/api/appointments/{id:guid}/cancel", async (Guid id, IMediator med) =>
    (await med.Send(new CancelAppointmentCommand(id))) ? Results.NoContent() : Results.NotFound());

app.MapPut("/api/appointments/{id:guid}/attend", async (Guid id, IMediator med) =>
    (await med.Send(new AttendAppointmentCommand(id))) ? Results.NoContent() : Results.NotFound());

app.MapPost("/api/seed", async (AppDbContext db) =>
{
    var client = new Client("Cliente Demo", "demo@mail.com", "0999999999");
    var s30 = new Service("Consulta 30m", 30);
    var s60 = new Service("Consulta 60m", 60);

    db.Add(client);
    db.AddRange(s30, s60);
    await db.SaveChangesAsync();

    return Results.Ok(new
    {
        Client = client.Id,
        Service30 = s30.Id,
        Service60 = s60.Id
    });
});

// ---- CLIENTES ----
app.MapGet("/api/clients", async (AppDbContext db) =>
    await db.Clients
        .OrderBy(c => c.Name)
        .Select(c => new { c.Id, c.Name, c.Email, c.Phone })
        .ToListAsync());

app.MapPost("/api/clients", async (AppDbContext db, ClientDto dto) =>
{
    var c = new Client(dto.Name, dto.Email, dto.Phone);
    db.Add(c);
    await db.SaveChangesAsync();
    return Results.Created($"/api/clients/{c.Id}", new { c.Id, c.Name, c.Email, c.Phone });
});



// ---- SERVICIOS ----
app.MapGet("/api/services", async (AppDbContext db) =>
    await db.Services
        .Where(s => s.Active)
        .OrderBy(s => s.Name)
        .Select(s => new { s.Id, s.Name, s.DurationMin, s.Active })
        .ToListAsync());

app.MapPost("/api/services", async (AppDbContext db, ServiceDto dto) =>
{
    if (dto.DurationMin <= 0) return Results.BadRequest("Duration must be > 0");
    var s = new Service(dto.Name, dto.DurationMin);
    db.Add(s);
    await db.SaveChangesAsync();
    return Results.Created($"/api/services/{s.Id}", new { s.Id, s.Name, s.DurationMin, s.Active });
});

// DTO para el historial


// GET /api/appointments/history?page=1&size=50
app.MapGet("/api/appointments/history", async (AppDbContext db, int page = 1, int size = 50) =>
{
    if (page < 1) page = 1;
    if (size < 1 || size > 200) size = 50;

    var baseQuery =
        from a in db.Appointments.AsNoTracking()
        join c in db.Clients.AsNoTracking()  on a.ClientId  equals c.Id
        join s in db.Services.AsNoTracking() on a.ServiceId equals s.Id
        orderby a.StartUtc descending
        select new AppointmentHistoryVm(
            a.Id,
            a.StartUtc,
            (byte)a.Status,      // ajusta el tipo si tu Status es enum distinto
            a.Notes,
            a.ClientId,
            c.Name,              // <- nombre del cliente
            a.ServiceId,
            s.Name               // <- nombre del servicio
        );

    var total = await baseQuery.CountAsync();
    var data  = await baseQuery
        .Skip((page - 1) * size)
        .Take(size)
        .ToListAsync();

    return Results.Ok(new { total, page, size, data });
});


app.Run();
record ClientDto(string Name, string? Email, string? Phone);
record ServiceDto(string Name, int DurationMin);

public record AppointmentHistoryVm(
    Guid Id,
    DateTime StartUtc,
    byte Status,
    string? Notes,
    Guid ClientId,
    string ClientName,
    Guid ServiceId,
    string ServiceName
);
