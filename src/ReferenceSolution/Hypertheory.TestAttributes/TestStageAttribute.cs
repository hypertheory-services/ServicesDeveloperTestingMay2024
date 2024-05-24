using Xunit.Sdk;

namespace Hypertheory.TestAttributes;

[TraitDiscoverer("Hypertheory.TestAttributes.StageDiscoverer", "Hypertheory.TestAttributes")]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public abstract class TestStageAttribute : Attribute, ITraitAttribute
{
    public abstract string Key { get; }
    public abstract string Type { get; }
}
