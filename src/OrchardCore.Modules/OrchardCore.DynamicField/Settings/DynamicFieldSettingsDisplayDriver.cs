using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.DynamicFields.Extensions;
using OrchardCore.DynamicFields.Fields;

namespace OrchardCore.DynamicFields.Settings;

public sealed class DynamicFieldSettingsDisplayDriver(
    IHttpContextAccessor httpContextAccessor,
    IContentDefinitionManager contentDefinitionManager
    ) : ContentPartFieldDefinitionDisplayDriver<DynamicField>
{

    public override async Task<IDisplayResult> EditAsync(ContentPartFieldDefinition partFieldDefinition, BuildEditorContext context)
    {
        httpContextAccessor.HttpContext.Request.Query.TryGetValue("contentType", out var contentType);
        httpContextAccessor.HttpContext.Request.Query.TryGetValue("contentField", out var contentField);
        var templateSettings = await contentDefinitionManager.GetFieldSettingsAsync<DynamicField, DynamicFieldSettings>(contentType, contentField);

        return Initialize<DynamicFieldSettings>("DynamicFieldSettings_Edit", model =>
        {
            var settings = templateSettings ?? partFieldDefinition.GetSettings<DynamicFieldSettings>();
            model.Code = settings.Code;
            model.Resources = settings.Resources;
        }).Location("Content");
    }

    public override async Task<IDisplayResult> UpdateAsync(ContentPartFieldDefinition partFieldDefinition, UpdatePartFieldEditorContext context)
    {
        var model = new DynamicFieldSettings();
        await context.Updater.TryUpdateModelAsync(model, Prefix);
        context.Builder.WithSettings(model);

        return Edit(partFieldDefinition, context);
    }
}
