using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.DynamicField.Extensions;

namespace OrchardCore.DynamicField.Settings;

public sealed class DynamicFieldSettingsDisplayDriver(IHttpContextAccessor _httpContextAccessor, IContentDefinitionManager _contentDefinitionManager) : ContentPartFieldDefinitionDisplayDriver<Fields.DynamicField>
{

    public override async Task<IDisplayResult> EditAsync(ContentPartFieldDefinition partFieldDefinition, BuildEditorContext context)
    {
        _httpContextAccessor.HttpContext.Request.Query.TryGetValue("contentType", out var contentType);
        _httpContextAccessor.HttpContext.Request.Query.TryGetValue("contentField", out var contentField);
        //"MyDynamicTestField", "WebComponent"
        var templateSettings = await _contentDefinitionManager.GetFieldSettingsAsync<Fields.DynamicField, DynamicFieldSettings>(contentType, contentField);

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
        var success = await context.Updater.TryUpdateModelAsync(model, Prefix);

        context.Builder.WithSettings(model);

        return Edit(partFieldDefinition, context);
    }
}
