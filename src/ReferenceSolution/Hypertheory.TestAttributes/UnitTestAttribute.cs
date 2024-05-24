namespace Hypertheory.TestAttributes;

public class UnitTestAttribute : TestStageAttribute
{
    public override string Key => "TestStage";
    public override string Type => "Unit";
}
