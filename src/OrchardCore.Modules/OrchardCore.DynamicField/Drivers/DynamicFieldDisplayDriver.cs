using System.Dynamic;
using System.Text.Json;
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
            model.Value = JsonSerializer.Serialize(field.Value);
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
            model.Value = JsonSerializer.Serialize(field.Value);
            model.Field = field;
            model.Part = context.ContentPart;
            model.PartFieldDefinition = context.PartFieldDefinition;
        });
    }

    public override async Task<IDisplayResult> UpdateAsync(Fields.DynamicField field, UpdateFieldEditorContext context)
    {
        var model = new EditDynamicFieldViewModel();
        await context.Updater.TryUpdateModelAsync(model, Prefix, f => f.Value);
        field.Value = JsonSerializer.Deserialize<ExpandoObject>(model.Value);

        return Edit(field, context);
    }
}
