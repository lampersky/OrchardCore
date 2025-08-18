using OrchardCore.ResourceManagement;

namespace OrchardCore.DynamicField.Settings;

public class DynamicFieldSettings
{
    public string Code { get; set; }
    public string InlineScript { get; set; }
    public string InlineStyle { get; set; }
    public string ScriptUrls { get; set; }
    public string ScriptModuleUrls { get; set; }
    public string StyleUrls { get; set; }
    public List<Resource> Resources { get; set; } = new();
}

public enum ResourceType
{
    Script,
    Style,
}

public class Resource
{
    public ResourceType Type { get; set; } = ResourceType.Script;
    public string Src { get; set; }
    public ResourceLocation At { get; set; } = ResourceLocation.Foot;
    public string IsModule { get; set; }
    public string IsInline { get; set; }
}
