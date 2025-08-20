using System.Dynamic;

namespace OrchardCore.DynamicField.Extensions;

public static class ExpandoObjectExtensions
{
    public static Dictionary<string, object> Flatten(this ExpandoObject expando)
    {
        var result = new Dictionary<string, object>();
        if (expando != null)
        {
            FlattenExpando(expando, result, null);
        }
        return result;
    }

    private static void FlattenExpando(IDictionary<string, object> expando, Dictionary<string, object> result, string parentKey)
    {
        foreach (var kvp in expando)
        {
            string key = parentKey == null ? kvp.Key : $"{parentKey}.{kvp.Key}";

            if (kvp.Value is ExpandoObject nested)
            {
                FlattenExpando((IDictionary<string, object>)nested, result, key);
            }
            else if (kvp.Value is IDictionary<string, object> nestedDict)
            {
                FlattenExpando(nestedDict, result, key);
            }
            else
            {
                result[key] = kvp.Value;
            }
        }
    }
}
