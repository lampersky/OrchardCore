using System.Dynamic;
using OrchardCore.ContentManagement;

namespace OrchardCore.DynamicField.Fields;

public class DynamicField : ContentField
{
    public ExpandoObject Value { get; set; }
}