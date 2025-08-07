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
            model.InlineStyle = settings.InlineStyle;
            model.InlineScript = settings.InlineScript;
            model.ScriptUrls = settings.ScriptUrls;
            model.StyleUrls = settings.StyleUrls;
            model.ScriptModuleUrls = settings.ScriptModuleUrls;
            if (settings.Resources.Count == 0)
            {
                for (var i = 0; i < 5; i++)
                {
                    settings.Resources.Add(new());
                }
            }
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
