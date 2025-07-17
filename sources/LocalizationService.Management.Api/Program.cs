using LocalizationService.Data.Master;
using LocalizationService.Data.Read;
using LocalizationService.Management.Api.Api.Middlewares;
using LocalizationService.Management.Api.Application;
using LocalizationService.Management.Api.Workers;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(
    options => options.AddDefaultPolicy(
        policy => policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()));

builder.Services.AddDbContextPool<MasterDbContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("Master")));

builder.Services.AddDbContextPool<ReadDbContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("Read")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<Migrator>();
builder.Services.AddSingleton<ErrorMiddleware>();
builder.Services.AddApplicationServices();
builder.Services.AddRepositories();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorMiddleware>();
app.UseCors();
app.MapControllers();

app.Run();