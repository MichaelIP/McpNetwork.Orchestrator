using McpNetwork.Orchestrator.Models.Enums;

namespace McpNetwork.Orchestrator.Orchestration;

/// <summary>
/// OrchestratorResult represents the outcome of executing an orchestration. 
/// It indicates whether the orchestration was successful, and if so, contains the resulting value of type TResult. 
/// If the orchestration failed, it contains a failure reason of type EFeatureFailureReason. 
/// </summary>
/// <typeparam name="TResult"></typeparam>
public sealed class OrchestratorResult<TResult>
{
    /// <summary>
    /// IsSuccess indicates whether the orchestration execution was successful. 
    /// If true, the Value property will contain the result. 
    /// If false, the FailureReason property will indicate why the orchestration failed.
    /// </summary>
    public bool IsSuccess { get; }
    /// <summary>
    /// Value contains the result of the orchestration execution if it was successful.
    /// </summary>
    public TResult? Value { get; }
    /// <summary>
    /// FailureReason contains the reason for orchestration failure if IsSuccess is false.
    /// </summary>
    public EFeatureFailureReason? FailureReason { get; }

    private OrchestratorResult(bool success, TResult? value, EFeatureFailureReason? reason)
    {
        IsSuccess = success;
        Value = value;
        FailureReason = reason;
    }

    /// <summary>
    /// Placeholder for a successful orchestration result.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static OrchestratorResult<TResult> Success(TResult value) => new(true, value, null);

    /// <summary>
    /// Placeholder for a failed orchestration result, with the specified failure reason.
    /// </summary>
    /// <param name="reason"></param>
    /// <returns></returns>
    public static OrchestratorResult<TResult> Failure(EFeatureFailureReason reason) => new(false, default, reason);
}
