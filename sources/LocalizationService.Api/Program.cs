using LocalizationService.Api.Services;
using LocalizationService.Data.Read;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(options => options.AddConsole());

builder.Services.AddPooledDbContextFactory<ReadDbContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("Read")));

builder.Services.AddSingleton<ITranslationService, TranslationService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.MapGet(
    "/api/key_{key}/translation_{locale}",
    async (string key, string locale, [FromServices] ITranslationService service, CancellationToken cancellationToken) =>
    {
        var result = await service.GetTranslationAsync(key, locale, cancellationToken);
        return result is null ? Results.NoContent() : Results.Content(result, "text/plain");
    });

app.MapGet(
    "/api/key_{key}/translation",
    async (string key, [FromServices] ITranslationService service, CancellationToken cancellationToken) =>
    {
        var result = await service.GetTranslationsAsync(key, cancellationToken);
        return Results.Ok(result);
    });

app.MapGet(
    "/api/key/translation_{locale}",
    async (string locale, [FromServices] ITranslationService service, CancellationToken cancellationToken) =>
    {
        var result = await service.GetAllTranslationForLocale(locale, cancellationToken);
        return Results.Ok(result);
    });

app.Run();