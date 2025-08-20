using OrchardCore.ContentManagement.Metadata;

namespace OrchardCore.DynamicField.Extensions;
internal static class IContentDefinitionManagerExtensions
{
    public static async Task<Dictionary<string, Dictionary<string, TSettings>>> ListTypesWithFieldAndSettings<TField, TSettings>(this IContentDefinitionManager contentDefinitionManager) where TSettings : new()
    {
        var contentTypeDefinitions = await contentDefinitionManager.ListTypeDefinitionsAsync();
        var typesWithFieldAndSettings = contentTypeDefinitions
            .Where(contentTypeDefinition =>
                contentTypeDefinition.Parts
                    .SelectMany(p => p.PartDefinition.Fields)
                    .Any(f => f.FieldDefinition.Name == typeof(TField).Name)
            )
            .ToDictionary(
                contentTypeDefinition => contentTypeDefinition.Name,
                contentTypeDefinition => contentTypeDefinition.Parts
                    .SelectMany(x => x.PartDefinition.Fields.Where(f => f.FieldDefinition.Name == typeof(TField).Name))
                    .ToDictionary(x => x.Name, x => x.GetSettings<TSettings>())
            );

        return typesWithFieldAndSettings;
    }

    public static async Task<TSettings> GetFieldSettings<TField, TSettings>(this IContentDefinitionManager contentDefinitionManager, string typeName, string fieldName) where TSettings : new()
    {
        if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(fieldName))
        {
            return default;
        }

        var contentTypeDefinition = await contentDefinitionManager.GetTypeDefinitionAsync(typeName);
        var fieldSettings = contentTypeDefinition.Parts
            .SelectMany(x => x.PartDefinition.Fields.Where(f => f.FieldDefinition.Name == typeof(TField).Name && f.Name == fieldName))
            .Select(x => x.GetSettings<TSettings>())
            .FirstOrDefault();

        return fieldSettings;
    }
}
