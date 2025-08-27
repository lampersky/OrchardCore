using OrchardCore.DynamicFields.Settings;

namespace OrchardCore.DynamicFields.ViewModels;

public class EditDynamicFieldSettingsViewModel
{
    public bool IndexRawValue { get; set; } = true;
    public string Code { get; set; }
    public Dictionary<int, Resource> Resources { get; set; } = new();
}
