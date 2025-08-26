using Fluid;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DynamicFields.Drivers;
using OrchardCore.DynamicFields.Fields;
using OrchardCore.DynamicFields.Settings;
using OrchardCore.DynamicFields.TagHelpers;
using OrchardCore.DynamicFields.ViewModels;
using OrchardCore.Modules;

namespace OrchardCore.DynamicFields;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddContentField<DynamicField>()
            .UseDisplayDriver<DynamicFieldDisplayDriver>();
        services.AddScoped<IContentPartFieldDefinitionDisplayDriver, DynamicFieldSettingsDisplayDriver>();

        services.Configure<TemplateOptions>(o =>
        {
            o.MemberAccessStrategy.Register<DynamicField>();
            o.MemberAccessStrategy.Register<DisplayDynamicFieldViewModel>();
        });

        services.AddTagHelpers<ExtendedScriptTagHelper>();
        services.AddTagHelpers<CustomAspForHelper>();
    }
}
