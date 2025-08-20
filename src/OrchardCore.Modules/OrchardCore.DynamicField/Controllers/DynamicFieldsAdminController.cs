using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DynamicField.Extensions;
using OrchardCore.DynamicField.Settings;

namespace OrchardCore.DynamicField.Controllers;

[Admin]
public sealed class DynamicFieldsAdminController : Controller
{
    private readonly IContentDefinitionManager _contentDefinitionManager;
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DynamicFieldsAdminController(
        IContentDefinitionManager contentDefinitionManager,
        IAuthorizationService authorizationService,
        IHttpContextAccessor httpContextAccessor)
    {
        _contentDefinitionManager = contentDefinitionManager;
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
    }

    [Admin("DynamicFields/SearchDynamicFields")]
    [HttpGet]
    public async Task<IActionResult> SearchDynamicFields()
    {

        var typesWithDynamicFields = await _contentDefinitionManager.ListTypesWithFieldAndSettings<Fields.DynamicField, DynamicFieldSettings>();

        var contentTypeDefinitions = await _contentDefinitionManager.ListTypeDefinitionsAsync();

        var typesWithDynamicFields2 = contentTypeDefinitions
            .Where(contentTypeDefinition =>
                contentTypeDefinition.Parts
                    .SelectMany(p => p.PartDefinition.Fields)
                    .Any(f => f.FieldDefinition.Name == nameof(Fields.DynamicField))
            )
            .ToDictionary(
                contentTypeDefinition => contentTypeDefinition.Name,
                contentTypeDefinition => contentTypeDefinition.Parts
                    .SelectMany(x => x.PartDefinition.Fields.Where(f => f.FieldDefinition.Name == nameof(Fields.DynamicField)))
                    .ToDictionary(x => x.Name, x => x.GetSettings<DynamicFieldSettings>())
            );

        return new ObjectResult(typesWithDynamicFields);
    }
}
