using System.Dynamic;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;

namespace OrchardCore.DynamicField.ViewModels;

public class DisplayDynamicFieldViewModel
{
    public ExpandoObject Value => Field.Value;
    public string RawValue { get; set; }
    public Fields.DynamicField Field { get; set; }
    public ContentPart Part { get; set; }
    public ContentPartFieldDefinition PartFieldDefinition { get; set; }
}
