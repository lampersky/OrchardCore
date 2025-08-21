using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DynamicFields.Extensions;
using OrchardCore.DynamicFields.Fields;

namespace OrchardCore.DynamicFields.Controllers;

[Admin]
public sealed class DynamicFieldsAdminController(IContentDefinitionManager contentDefinitionManager) : Controller
{

    [Admin("DynamicFields/SearchDynamicFields")]
    [HttpGet]
    public async Task<IActionResult> SearchDynamicFields()
    {
        var typesWithDynamicFields = await contentDefinitionManager.ListContentTypesWithFieldsNamesOfTypeAsync<DynamicField>();

        return new ObjectResult(typesWithDynamicFields);
    }
}
