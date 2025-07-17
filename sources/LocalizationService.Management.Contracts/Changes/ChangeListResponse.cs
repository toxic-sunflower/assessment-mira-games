namespace LocalizationService.Management.Contracts.Changes;

public readonly record struct ChangeListResponse(List<ChangeModel> Changes);