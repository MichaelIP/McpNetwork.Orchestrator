using McpNetwork.Orchestrator.Models.Enums;

namespace McpNetwork.Orchestrator.Features;

public sealed class FeatureResult<TResult>
{
    public bool IsSuccess { get; }
    public TResult Value { get; }
    public EFeatureFailureReason? Reason { get; }

    private FeatureResult(bool isSuccess, TResult value, EFeatureFailureReason? reason)
    {
        IsSuccess = isSuccess;
        Value = value;
        Reason = reason;
    }

    // --------------------
    // Success
    public static FeatureResult<TResult> Success(TResult value) => new(true, value, null);

    // --------------------
    // Generic failure
    public static FeatureResult<TResult> Failure(EFeatureFailureReason reason) => new(false, default!, reason);

    // --------------------
    // Semantic helpers
    public static FeatureResult<TResult> Busy() => Failure(EFeatureFailureReason.Busy);
    public static FeatureResult<TResult> Timeout() => Failure(EFeatureFailureReason.Timeout);
    public static FeatureResult<TResult> Unknown() => Failure(EFeatureFailureReason.Unknown);
    public static FeatureResult<TResult> Rejected() => Failure(EFeatureFailureReason.Rejected);
    public static FeatureResult<TResult> Cancelled() => Failure(EFeatureFailureReason.Cancelled);
    public static FeatureResult<TResult> InvalidState() => Failure(EFeatureFailureReason.InvalidState);
    public static FeatureResult<TResult> TransportError() => Failure(EFeatureFailureReason.TransportError);
    public static FeatureResult<TResult> UnhandledException() => Failure(EFeatureFailureReason.UnhandledException);
    public static FeatureResult<TResult> InvalidConfiguration() => Failure(EFeatureFailureReason.InvalidConfiguration);

}
