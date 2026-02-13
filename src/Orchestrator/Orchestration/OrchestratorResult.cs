using McpNetwork.Orchestrator.Models.Enums;

namespace McpNetwork.Orchestrator.Orchestration
{
    public sealed class OrchestratorResult<TResult>
    {
        public bool IsSuccess { get; }
        public TResult? Value { get; }
        public EFeatureFailureReason? FailureReason { get; }

        private OrchestratorResult(bool success, TResult? value, EFeatureFailureReason? reason)
        {
            IsSuccess = success;
            Value = value;
            FailureReason = reason;
        }

        public static OrchestratorResult<TResult> Success(TResult value) => new(true, value, null);

        public static OrchestratorResult<TResult> Failure(EFeatureFailureReason reason) => new(false, default, reason);
    }

}
