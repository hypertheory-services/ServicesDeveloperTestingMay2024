namespace Hypertheory.TestAttributes;

public class IntegratedTestAttribute : TestStageAttribute
{
    public override string Key => "TestStage";
    public override string Type => "SystemIntegration";
}