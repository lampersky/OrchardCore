using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DynamicField.Extensions;

namespace OrchardCore.DynamicField.Controllers;

[Admin]
public sealed class DynamicFieldsAdminController(IContentDefinitionManager contentDefinitionManager) : Controller
{

    [Admin("DynamicFields/SearchDynamicFields")]
    [HttpGet]
    public async Task<IActionResult> SearchDynamicFields()
    {
        var typesWithDynamicFields = await contentDefinitionManager.ListContentTypesWithFieldsNamesOfTypeAsync<Fields.DynamicField>();

        return new ObjectResult(typesWithDynamicFields);
    }
}
