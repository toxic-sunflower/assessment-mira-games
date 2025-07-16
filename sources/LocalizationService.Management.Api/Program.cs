using LocalizationService.Data.Master;
using LocalizationService.Management.Api.Middlewares;
using LocalizationService.Management.Api.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
builder.Services.AddSingleton<ErrorMiddleware>();

builder.Services.AddDbContextPool<MasterDbContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("Master")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<Migrator>();

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