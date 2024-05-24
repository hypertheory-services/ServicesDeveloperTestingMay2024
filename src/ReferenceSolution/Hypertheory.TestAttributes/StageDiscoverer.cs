using Xunit.Abstractions;
using Xunit.Sdk;

namespace Hypertheory.TestAttributes;

public class StageDiscoverer : ITraitDiscoverer
{
  
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
        var attributeInfo = traitAttribute as ReflectionAttributeInfo;
        var category = attributeInfo?.Attribute as TestStageAttribute;
        var value = category?.Type ?? string.Empty;
        var key = category?.Key ?? string.Empty;
        yield return new KeyValuePair<string, string>(key, value);
    }
}
