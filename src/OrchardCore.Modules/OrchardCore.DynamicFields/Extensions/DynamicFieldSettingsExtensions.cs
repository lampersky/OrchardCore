using OrchardCore.ContentManagement.Utilities;
using OrchardCore.DynamicFields.Fields;
using OrchardCore.DynamicFields.Settings;

namespace OrchardCore.DynamicFields.Extensions;
public static class DynamicFieldSettingsExtensions
{
    public static DynamicFieldSettings Fix(this DynamicFieldSettings dynamicFieldSettings,
        string clonedContentType, string clonedContentField, string contentType, string contentField)
    {
        if (dynamicFieldSettings is null)
        {
            return dynamicFieldSettings;
        }

        var cloned = $"dynamicField.{clonedContentType.ToSafeName()}_{clonedContentField.ToSafeName()}_{nameof(DynamicField.Value)}";
        var newName = $"dynamicField.{contentType.ToSafeName()}_{contentField.ToSafeName()}_{nameof(DynamicField.Value)}";

        if (!string.IsNullOrEmpty(dynamicFieldSettings.Code))
        {
            dynamicFieldSettings.Code = dynamicFieldSettings.Code.Replace(cloned, newName);
        }

        foreach (var resource in dynamicFieldSettings.Resources)
        {
            if (resource.IsScript && resource.IsInline && !string.IsNullOrEmpty(resource.Src))
            {
                resource.Src = resource.Src.Replace(cloned, newName);
            }
        }

        return dynamicFieldSettings;
    }
}
