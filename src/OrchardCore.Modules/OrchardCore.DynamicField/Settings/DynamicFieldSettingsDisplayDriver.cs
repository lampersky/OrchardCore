using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.DynamicField.Settings;

public sealed class DynamicFieldSettingsDisplayDriver : ContentPartFieldDefinitionDisplayDriver<Fields.DynamicField>
{
    public override IDisplayResult Edit(ContentPartFieldDefinition partFieldDefinition, BuildEditorContext context)
    {
        return Initialize<DynamicFieldSettings>("DynamicFieldSettings_Edit", model =>
        {
            var settings = partFieldDefinition.GetSettings<DynamicFieldSettings>();
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
