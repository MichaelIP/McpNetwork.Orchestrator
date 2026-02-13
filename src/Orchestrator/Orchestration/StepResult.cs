using McpNetwork.Orchestrator.Models.Enums;

namespace McpNetwork.Orchestrator.Orchestration;

public sealed class StepResult
{
    public bool Continue { get; }
    public EFeatureFailureReason? FailureReason { get; }

    private StepResult(bool cont, EFeatureFailureReason? reason)
    {
        Continue = cont;
        FailureReason = reason;
    }

    public static StepResult Next() => new(true, null);
    public static StepResult Fail(EFeatureFailureReason reason) => new(false, reason);
}
