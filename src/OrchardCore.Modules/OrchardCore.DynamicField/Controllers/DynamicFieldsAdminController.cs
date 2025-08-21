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
        var typesWithDynamicFields = await _contentDefinitionManager.ListContentTypesWithFieldsNamesOfTypeAsync<Fields.DynamicField>();

        return new ObjectResult(typesWithDynamicFields);
    }
}
