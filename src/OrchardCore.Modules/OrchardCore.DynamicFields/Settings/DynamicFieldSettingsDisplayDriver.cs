using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.DynamicFields.Extensions;
using OrchardCore.DynamicFields.Fields;
using OrchardCore.DynamicFields.ViewModels;

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
        templateSettings = templateSettings.Fix(contentType, contentField,
            partFieldDefinition.PartDefinition.Name,
            partFieldDefinition.Name);

        return Initialize<EditDynamicFieldSettingsViewModel>("DynamicFieldSettings_Edit", model =>
        {
            var settings = templateSettings ?? partFieldDefinition.GetSettings<DynamicFieldSettings>();
            model.Code = settings.Code;
            model.IndexRawValue = settings.IndexRawValue;
            model.Resources = settings.Resources.Select((item, index) => new { item, index }).ToDictionary(x => x.index, x => x.item);
        }).Location("Content");
    }

    public override async Task<IDisplayResult> UpdateAsync(ContentPartFieldDefinition partFieldDefinition, UpdatePartFieldEditorContext context)
    {
        var model = new EditDynamicFieldSettingsViewModel();
        await context.Updater.TryUpdateModelAsync(model, Prefix);

        var settings = new DynamicFieldSettings()
        {
            Code = model.Code,
            IndexRawValue = model.IndexRawValue,
            Resources = model.Resources.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList(),
        };


        context.Builder.WithSettings(settings);

        return Edit(partFieldDefinition, context);
    }
}
