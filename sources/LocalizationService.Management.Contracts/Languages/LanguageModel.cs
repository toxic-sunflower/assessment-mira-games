namespace LocalizationService.Management.Contracts.Languages;

public class LanguageModel
{
    public LanguageModel() { }
    
    public LanguageModel(long id, string code, string displayName) =>
        (Id, Code, DisplayName) = (id, code, displayName);
    
    public long Id { get; set; }
    public string Code { get; set; }
    public string DisplayName { get; set; }
}