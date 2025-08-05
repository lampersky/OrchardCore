using Fluid;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DynamicField.Drivers;
using OrchardCore.DynamicField.Settings;
using OrchardCore.DynamicField.ViewModels;
using OrchardCore.Modules;

namespace OrchardCore.DynamicField;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddContentField<Fields.DynamicField>()
            .UseDisplayDriver<DynamicFieldDisplayDriver>();
        services.AddScoped<IContentPartFieldDefinitionDisplayDriver, DynamicFieldSettingsDisplayDriver>();

        services.Configure<TemplateOptions>(o =>
        {
            o.MemberAccessStrategy.Register<Fields.DynamicField>();
            o.MemberAccessStrategy.Register<DisplayDynamicFieldViewModel>();
        });
    }

    public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
        routes.MapAreaControllerRoute(
            name: "Home",
            areaName: "OrchardCore.DynamicField",
            pattern: "Home/Index",
            defaults: new { controller = "Home", action = "Index" }
        );
    }
}
