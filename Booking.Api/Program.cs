using MediatR;
using AutoMapper;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Booking.Application.DTOs;
using Booking.Infrastructure;
using Booking.Infrastructure.Persistence;
using Booking.Domain.Entities;

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

app.Run();
