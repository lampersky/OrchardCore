using System.Text.Json;
using GraphQL.Types;
using Microsoft.Extensions.Localization;
using OrchardCore.Apis.GraphQL;

namespace OrchardCore.DynamicField.GraphQL;

public class DynamicFieldQueryObjectType : ObjectGraphType<Fields.DynamicField>
{
    public DynamicFieldQueryObjectType(IStringLocalizer<DynamicFieldQueryObjectType> S)
    {
        Name = nameof(OrchardCore.DynamicField.Fields.DynamicField);

        Field<StringGraphType, string>("raw")
            .Description(S["the raw value of the dynamic field"])
            .PagingArguments()
            .Resolve(x =>
            {
                if (x.Source.Value is null)
                {
                    return null;
                }

                var json = JsonSerializer.Serialize(x.Source.Value);

                return json;
            });

        Field<JsonObjectGraphType>("value")
            .Description(S["The value of the dynamic field"])
            .Resolve(context =>
            {
                return context.Source.Value;
            });
    }
}
