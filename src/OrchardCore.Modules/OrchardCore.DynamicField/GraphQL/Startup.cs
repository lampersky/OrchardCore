using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Apis;
using OrchardCore.Modules;

namespace OrchardCore.DynamicField.GraphQL;

[RequireFeatures("OrchardCore.Apis.GraphQL")]
public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddObjectGraphType<Fields.DynamicField, DynamicFieldQueryObjectType>();
    }
}
