using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;

namespace OrchardCore.DynamicField.Extensions;
internal static class IContentDefinitionManagerExtensions
{
    public static async Task<Dictionary<string, List<ContentPartFieldDefinition>>> ListContentTypesWithFieldsOfType<TField>(this IContentDefinitionManager contentDefinitionManager)
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
                    .ToList()
            );

        return typesWithFieldAndSettings;
    }

    public static async Task<Dictionary<string, List<string>>> ListContentTypesWithFieldsNamesOfType<TField>(this IContentDefinitionManager contentDefinitionManager)
    {
        var typesWithField = await contentDefinitionManager.ListContentTypesWithFieldsOfType<TField>();

        return typesWithField.ToDictionary(x => x.Key, x => x.Value.Select(y => y.Name).ToList());
    }

    public static async Task<Dictionary<string, Dictionary<string, TSettings>>> ListTypesWithFieldsNamesAndSettingsOfType<TField, TSettings>(this IContentDefinitionManager contentDefinitionManager) where TSettings : new()
    {
        var typesWithField = await contentDefinitionManager.ListContentTypesWithFieldsOfType<TField>();

        return typesWithField.ToDictionary(x => x.Key, x => x.Value.ToDictionary(y => y.Name, y => y.GetSettings<TSettings>()));
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
