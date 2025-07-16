using LocalizationService.Management.Client;
using LocalizationService.Management.Contracts.Languages;
using Microsoft.AspNetCore.Components;

namespace LocalizationService.Management.Ui.Features.Languages;

public partial class Languages : ComponentBase
{
    [Inject] private ILanguageClient LanguageClient { get; set; } = default!;

    protected IEnumerable<LanguageModel> LanguagesList { get; set; }

    private AddLanguageDialog? _addDialog;
    private EditLanguageDialog? _editDialog;

    protected override async Task OnInitializedAsync()
    {
        await Reload();
    }

    protected async Task Reload()
    {
        var apiResponse = await LanguageClient.GetLanguageListAsync();
        if (apiResponse.TryGetData(out var data, out _))
        {
            LanguagesList = data.Languages;
        } 
    }

    protected void OpenAddDialog() => _addDialog?.Open();

    protected void OpenEditDialog(LanguageModel lang) => _editDialog?.Open(lang);

    protected async Task Delete(string code)
    {
        var apiResponse = await LanguageClient.DeleteLanguageAsync(code, CancellationToken.None);

        if (apiResponse.TryGetData(out var data, out _))
        {
            return;
        }
        
        await Reload();
    }
}
