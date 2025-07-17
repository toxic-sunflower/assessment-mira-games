// using System.Transactions;
// using LocalizationService.Data.Master;
// using LocalizationService.Data.Read;
// using Microsoft.EntityFrameworkCore;
// using OneOf;
// using OneOf.Types;
//
// namespace LocalizationService.Management.Application.Publishing;
//
// public interface IPublishTranslationsHandler
// {
//     Task<OneOf<Success, Error>> PublishTranslationsAsync(CancellationToken cancellationToken);
// }
//
// public class PublishTranslationsHandler : IPublishTranslationsHandler
// {
//     private const int BatchSize = 100;
//     
//     private readonly MasterDbContext _masterDbContext;
//     private readonly ReadDbContext _readDbContext;
//     private readonly ILogger<PublishTranslationsHandler> _logger;
//
//     public PublishTranslationsHandler(
//         MasterDbContext masterDbContext,
//         ReadDbContext readDbContext,
//         ILogger<PublishTranslationsHandler> logger)
//     {
//         _masterDbContext = masterDbContext;
//         _readDbContext = readDbContext;
//         _logger = logger;
//     }
//
//     
//     public async Task<OneOf<Success, Error>> PublishTranslationsAsync(CancellationToken cancellationToken)
//     {
//         try
//         {
//             using var tx = new TransactionScope();
//
//             var processed = 0;
//             var fetchedCount = 0;
//             do
//             {
//                 var batch = await _masterDbContext.LocalizationKeys
//                     .Include(x => x.Translations)
//                     .OrderBy(x => x.Id)
//                     .Skip(processed * BatchSize)
//                     .Take(BatchSize)
//                     .ToListAsync(cancellationToken);
//                 
//                 fetchedCount = batch.Count;
//                 processed += fetchedCount;
//                 
// //                var r
//
//             } while (fetchedCount == BatchSize);
//             
//             tx.Complete();
//         }
//         catch (Exception exception)
//         {
//             
//         }
//     }
// }