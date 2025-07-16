using LocalizationService.Data.Master;
using LocalizationService.Data.Read;
using Microsoft.EntityFrameworkCore;

namespace LocalizationService.Management.Api.Workers;

public class Migrator : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Migrator(IServiceScopeFactory serviceScopeFactory) =>
        _serviceScopeFactory = serviceScopeFactory;


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var masterDbContext = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
        await using var readDbContext = scope.ServiceProvider.GetRequiredService<ReadDbContext>();
        await masterDbContext.Database.MigrateAsync(cancellationToken);
        await readDbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}