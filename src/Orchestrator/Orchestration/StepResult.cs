using McpNetwork.Orchestrator.Models.Enums;

namespace McpNetwork.Orchestrator.Orchestration;

/// <summary>
/// StepResult represents the outcome of a step in the orchestration process. 
/// It indicates whether to continue to the next step or to fail with a specific reason.
/// </summary>
public sealed class StepResult
{
    /// <summary>
    /// Continue indicates whether the orchestration should proceed to the next step.
    /// </summary>
    public bool Continue { get; }
    /// <summary>
    /// FailureReason provides the reason for failure if the orchestration should not continue.
    /// </summary>
    public EFeatureFailureReason? FailureReason { get; }

    /// <summary>
    /// Initializes a new instance of the StepResult class with the specified continuation status and failure reason.
    /// </summary>
    /// <param name="cont"></param>
    /// <param name="reason"></param>
    private StepResult(bool cont, EFeatureFailureReason? reason)
    {
        Continue = cont;
        FailureReason = reason;
    }

    /// <summary>
    /// Placeholder for the next step in the orchestration process. 
    /// It indicates that the process should continue without failure.
    /// </summary>
    /// <returns></returns>
    public static StepResult Next() => new(true, null);
    /// <summary>
    /// Placeholder for a failure in the orchestration process.
    /// </summary>
    /// <param name="reason"></param>
    /// <returns></returns>
    public static StepResult Fail(EFeatureFailureReason reason) => new(false, reason);
}
