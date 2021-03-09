using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using TestModule.Indexes;
using YesSql.Indexes;

namespace TestModule
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IIndexProvider, AnotherPersonAgeIndexProvider>();
            services.AddSingleton<IIndexProvider, PersonAgeIndexProvider>();

            services.AddTransient<IDataMigration, Migrations>();
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute
            (
                name: "addTestData",
                areaName: "TestModule",
                pattern: "/addTestData",
                defaults: new { controller = "Test", action = "AddTestData" }
            );

            routes.MapAreaControllerRoute
            (
                name: "getTestData",
                areaName: "TestModule",
                pattern: "/getTestData",
                defaults: new { controller = "Test", action = "GetTestData" }
            );
        }
    }
}
