using LocalizationService.Management.Client;
using LocalizationService.Management.Contracts.Languages;
using Microsoft.AspNetCore.Components;

namespace LocalizationService.Management.App.Features.Languages;

public partial class AddLanguageDialog : ComponentBase
{
    [Inject] private ILanguageClient LanguageClient { get; set; } = default!;
    [Parameter] public EventCallback OnAdded { get; set; }

    protected bool Visible { get; set; }

    protected LanguageModel Model { get; set; } = new();
    private readonly CancellationTokenSource _cts = new();

    public void Open()
    {
        Model = new LanguageModel();
        Visible = true;
        StateHasChanged();
    }

    protected void Close() => Visible = false;

    protected async Task HandleValidSubmit()
    {
        await LanguageClient.AddLanguageAsync(new AddLanguageRequest(Model.Code, Model.DisplayName), _cts.Token);
        await OnAdded.InvokeAsync();
        Visible = false;
    }

    public void Dispose() => _cts.Cancel();
}

