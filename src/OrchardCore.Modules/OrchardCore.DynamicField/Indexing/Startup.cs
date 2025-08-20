using Microsoft.Extensions.DependencyInjection;
using OrchardCore.DynamicField.Indexing.SQL;
using OrchardCore.Modules;
using OrchardCore.Data;
using OrchardCore.Data.Migration;

namespace OrchardCore.DynamicField.Indexing;

[Feature("OrchardCore.DynamicField.Indexing.SQL")]
[RequireFeatures("OrchardCore.ContentFields.Indexing.SQL")]
public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddDataMigration<Migrations>();
        services.AddScopedIndexProvider<DynamicFieldIndexProvider>();
    }
}
