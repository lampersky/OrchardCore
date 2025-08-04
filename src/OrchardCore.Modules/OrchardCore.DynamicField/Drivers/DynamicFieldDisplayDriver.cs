using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.DynamicField.ViewModels;

namespace OrchardCore.DynamicField.Drivers;

public sealed class DynamicFieldDisplayDriver : ContentFieldDisplayDriver<Fields.DynamicField>
{
    internal readonly IStringLocalizer S;

    public DynamicFieldDisplayDriver(IStringLocalizer<DynamicFieldDisplayDriver> localizer)
    {
        S = localizer;
    }

    public override IDisplayResult Display(Fields.DynamicField field, BuildFieldDisplayContext context)
    {
        return Initialize<DisplayDynamicFieldViewModel>(GetDisplayShapeType(context), model =>
        {
            model.Field = field;
            model.Part = context.ContentPart;
            model.PartFieldDefinition = context.PartFieldDefinition;
        })
        .Location("Detail", "Content")
        .Location("Summary", "Content");
    }

    public override IDisplayResult Edit(Fields.DynamicField field, BuildFieldEditorContext context)
    {
        return Initialize<EditDynamicFieldViewModel>(GetEditorShapeType(context), model =>
        {
            // var settings = context.PartFieldDefinition.GetSettings<DynamicFieldSettings>();
            // model.Text = context.IsNew && field.Value == null ? settings.DefaultValue : field.Value;
            // model.Field = field;
            // model.Part = context.ContentPart;
            // model.PartFieldDefinition = context.PartFieldDefinition;
        });
    }

    public override async Task<IDisplayResult> UpdateAsync(Fields.DynamicField field, UpdateFieldEditorContext context)
    {
        await context.Updater.TryUpdateModelAsync(field, Prefix, f => f.Value);
        // var settings = context.PartFieldDefinition.GetSettings<DynamicFieldSettings>();

        // if (settings.Required && field.Value == null)
        // {
        //     context.Updater.ModelState.AddModelError(Prefix, nameof(field.Value), S["A value is required for {0}.", context.PartFieldDefinition.DisplayName()]);
        // }

        return Edit(field, context);
    }
}
