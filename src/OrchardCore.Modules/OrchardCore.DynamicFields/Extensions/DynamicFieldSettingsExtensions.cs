using OrchardCore.ContentManagement.Utilities;
using OrchardCore.DynamicFields.Fields;
using OrchardCore.DynamicFields.Settings;

namespace OrchardCore.DynamicFields.Extensions;
public static class DynamicFieldSettingsExtensions
{
    const string scriptPrefix = "dynamicFields.";
    const string stylePrefix = "#";

    public static DynamicFieldSettings Fix(this DynamicFieldSettings dynamicFieldSettings,
        string clonedContentType, string clonedContentField, string contentType, string contentField)
    {
        if (dynamicFieldSettings is null)
        {
            return dynamicFieldSettings;
        }

        var cloned = $"{clonedContentType.ToSafeName()}_{clonedContentField.ToSafeName()}_{nameof(DynamicField.Value)}";
        var newName = $"{contentType.ToSafeName()}_{contentField.ToSafeName()}_{nameof(DynamicField.Value)}";

        cloned = $"{scriptPrefix}{cloned}";
        newName = $"{scriptPrefix}{newName}";

        if (!string.IsNullOrEmpty(dynamicFieldSettings.Code))
        {
            dynamicFieldSettings.Code = dynamicFieldSettings.Code.Replace(cloned, newName);
            //dynamicFieldSettings.Code = dynamicFieldSettings.Code.Replace(cloned, newName);
        }

        foreach (var resource in dynamicFieldSettings.Resources)
        {
            if (resource.IsInline && !string.IsNullOrEmpty(resource.Src))
            {
                continue;
            }

            if (resource.IsScript)
            {
                resource.Src = resource.Src.Replace(cloned, newName);
            }
            else if (resource.IsStyle)
            {
                //resource.Src = resource.Src.Replace(cloned, newName);
            }
        }

        return dynamicFieldSettings;
    }
}
