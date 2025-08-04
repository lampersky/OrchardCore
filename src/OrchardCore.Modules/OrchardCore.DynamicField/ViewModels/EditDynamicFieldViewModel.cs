using System;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;

namespace OrchardCore.DynamicField.ViewModels;

public class EditDynamicFieldViewModel
{
    public string Value { get; set; }
    public Fields.DynamicField Field { get; set; }
    public ContentPart Part { get; set; }
    public ContentPartFieldDefinition PartFieldDefinition { get; set; }
}
