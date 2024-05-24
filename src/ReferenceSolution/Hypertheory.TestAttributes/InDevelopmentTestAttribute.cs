namespace Hypertheory.TestAttributes;

public class InDevelopmentTestAttribute: TestStageAttribute
{
    public bool ReadyForReview { get; set; } = false;
    public string? Feature { get; set; }
    public override string Key => "TestStage";
    public override string Type => "Development";
    
}
