using LocalizationService.Management.Client;
using LocalizationService.Management.Contracts;
using LocalizationService.Management.Contracts.Languages;
using Microsoft.AspNetCore.Components;

namespace LocalizationService.Management.Ui.Features.Languages;

public partial class EditLanguageDialog : ComponentBase
{
    [Inject] private ILanguageClient LanguageClient { get; set; } = default!;
    [Parameter] public EventCallback OnSaved { get; set; }

    protected bool Visible { get; set; }

    protected long Id { get; set; }
    protected string Code { get; set; } = "";
    protected string? DisplayName { get; set; } = "";

    public void Open(LanguageModel lang)
    {
        Id = lang.Id;
        Code = lang.Code;
        DisplayName = lang.DisplayName;
        Visible = true;
        StateHasChanged();
    }

    protected void Close() => Visible = false;

    protected async Task Save()
    {
        await LanguageClient.UpdateLanguageAsync(
            Code,
            new UpdateLanguageRequest(DisplayName), CancellationToken.None);
        await OnSaved.InvokeAsync();
        Visible = false;
    }
}
