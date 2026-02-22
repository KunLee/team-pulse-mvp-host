using Microsoft.EntityFrameworkCore;
using MyMirror.TeamPulse.Api.Middleware;
using MyMirror.TeamPulse.Application.Features;
using MyMirror.TeamPulse.Application.Interfaces;
using MyMirror.TeamPulse.Infrastructure.Persistence;
using MyMirror.TeamPulse.Infrastructure.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TeamPulseDbContext>(opt =>
    opt.UseInMemoryDatabase("TeamPulseDb"));

builder.Services.AddScoped<IPulseRepository, PulseRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<SubmitPulseCommand>());

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TeamPulseDbContext>();
    DataSeeder.Seed(db);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.Run();
