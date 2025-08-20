using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ResourceManagement;

namespace OrchardCore.DynamicField.Settings;

public class DynamicFieldSettings
{
    public string Code { get; set; }
    public List<Resource> Resources { get; set; } = new();
}

public enum ResourceType
{
    Script,
    Style,
}

public class Resource
{
    public Resource()
    {
        IsModule = false;
        IsInline = false;
        IsDeferred = false;
        IsAsync = false;
    }

    public ResourceType Type { get; set; } = ResourceType.Script;
    public string Src { get; set; }
    public ResourceLocation At { get; set; } = ResourceLocation.Foot;

    [BindingBehavior(BindingBehavior.Optional)]
    [DefaultValue(false)]
    public bool IsModule { get; set; }

    [BindingBehavior(BindingBehavior.Optional)]
    [DefaultValue(false)]
    public bool IsInline { get; set; }

    [BindingBehavior(BindingBehavior.Optional)]
    [DefaultValue(false)]
    public bool IsDeferred { get; set; }

    [BindingBehavior(BindingBehavior.Optional)]
    [DefaultValue(false)]
    public bool IsAsync { get; set; }

    public string ScriptType => IsModule ? "module" : "";
}
