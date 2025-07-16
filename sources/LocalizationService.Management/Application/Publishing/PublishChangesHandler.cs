using OneOf;

namespace LocalizationService.Management.Api.Application;

public readonly record struct PublishSuccess;
public readonly record struct PublishFailure;


public interface IPublishChangesHandler
{
    Task<OneOf<PublishSuccess, PublishFailure>> PublishTranslationsAsync(CancellationToken cancellationToken);
}


public class PublishChangesHandler : IPublishChangesHandler
{
    public Task<OneOf<PublishSuccess, PublishFailure>> PublishTranslationsAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}