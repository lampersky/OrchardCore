using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DynamicField.Extensions;
using YesSql.Indexes;

namespace OrchardCore.DynamicField.Indexing.SQL;

public class DynamicFieldIndexProvider : ContentFieldIndexProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HashSet<string> _ignoredTypes = [];
    private IContentDefinitionManager _contentDefinitionManager;

    public DynamicFieldIndexProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override void Describe(DescribeContext<ContentItem> context)
    {
        context.For<TextFieldIndex>()
            .Map(async contentItem =>
            {
                // Remove index records of soft deleted items.
                if (!contentItem.Published && !contentItem.Latest)
                {
                    return null;
                }

                // Can we safely ignore this content item?
                if (_ignoredTypes.Contains(contentItem.ContentType))
                {
                    return null;
                }

                // Lazy initialization because of ISession cyclic dependency
                _contentDefinitionManager ??= _serviceProvider.GetRequiredService<IContentDefinitionManager>();

                // Search for TextField
                var contentTypeDefinition = await _contentDefinitionManager.GetTypeDefinitionAsync(contentItem.ContentType);

                // This can occur when content items become orphaned, particularly layer widgets when a layer is removed, before its widgets have been unpublished.
                if (contentTypeDefinition == null)
                {
                    _ignoredTypes.Add(contentItem.ContentType);
                    return null;
                }

                var fieldDefinitions = contentTypeDefinition
                    .Parts.SelectMany(x => x.PartDefinition.Fields.Where(f => f.FieldDefinition.Name == nameof(Fields.DynamicField)))
                    .ToArray();

                // This type doesn't have any TextField, ignore it
                if (fieldDefinitions.Length == 0)
                {
                    _ignoredTypes.Add(contentItem.ContentType);
                    return null;
                }

                return fieldDefinitions
                    .GetContentFields<Fields.DynamicField>(contentItem)
                    .Select(pair => {

                        var flattened = pair.Field.Value.Flatten();

                        var grouped = flattened.GroupBy(item => item.Value.GetType());

                        foreach (var group in grouped)
                        {
                            var type = group.Key;
                            foreach (var item in group)
                            {
                                var path = item.Key;
                                var value = item.Value;
                            }
                        }

                        return (TextFieldIndex)null;

                        //return new TextFieldIndex
                        //{
                        //    Latest = contentItem.Latest,
                        //    Published = contentItem.Published,
                        //    ContentItemId = contentItem.ContentItemId,
                        //    ContentItemVersionId = contentItem.ContentItemVersionId,
                        //    ContentType = contentItem.ContentType,
                        //    ContentPart = pair.Definition.ContentTypePartDefinition.Name,
                        //    ContentField = pair.Definition.Name,
                        //    Text = "todo", //pair.Field.Text?[..Math.Min(pair.Field.Text.Length, TextFieldIndex.MaxTextSize)],
                        //    BigText = "todo" //pair.Field.Text,
                        //};
                    });
            });
    }
}
