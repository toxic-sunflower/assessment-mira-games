using LocalizationService.Data.Master;
using Microsoft.EntityFrameworkCore;

namespace LocalizationService.Management.Api.Services;

public class Migrator : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Migrator(IServiceScopeFactory serviceScopeFactory) =>
        _serviceScopeFactory = serviceScopeFactory;


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<MasterDbContext>();    
        await dbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}