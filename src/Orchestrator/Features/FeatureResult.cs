using McpNetwork.Orchestrator.Models.Enums;

namespace McpNetwork.Orchestrator.Features;

/// <summary>
/// Represents the result of a feature operation, indicating success or failure and providing an associated value or
/// failure reason.
/// </summary>
/// <remarks>Use this type to encapsulate the outcome of feature-related operations, allowing callers to
/// distinguish between successful and failed results without relying on exceptions. The result includes a value when
/// successful, or a failure reason when not. This pattern is useful for APIs where operations can fail for multiple
/// well-defined reasons.</remarks>
/// <typeparam name="TResult">The type of the value returned by the feature operation when successful.</typeparam>
public sealed class FeatureResult<TResult>
{
    /// <summary>
    /// Gets a value indicating whether the operation completed successfully.
    /// </summary>
    public bool IsSuccess { get; }
    /// <summary>
    /// Gets the result value contained in the current instance.
    /// </summary>
    public TResult Value { get; }
    /// <summary>
    /// Gets the reason for the feature failure, if available.
    /// </summary>
    public EFeatureFailureReason? Reason { get; }

    /// <summary>
    /// Creates a successful result containing the specified value.
    /// </summary>
    /// <param name="value">The value to associate with the successful result.</param>
    /// <returns>A <see cref="FeatureResult{TResult}"/> representing a successful operation with the provided value.</returns>
    public static FeatureResult<TResult> Success(TResult value) => new(true, value, null);

    /// <summary>
    /// Creates a failed feature result with the specified failure reason.
    /// </summary>
    /// <param name="reason">The reason for the feature failure. Specifies why the operation did not succeed.</param>
    /// <returns>A <see cref="FeatureResult{TResult}"/> instance representing a failed operation, with the specified failure
    /// reason.</returns>
    public static FeatureResult<TResult> Failure(EFeatureFailureReason reason) => new(false, default!, reason);

    /// <summary>
    /// Creates a failed feature result that indicates the operation could not be completed because the system is busy.
    /// </summary>
    /// <returns>A <see cref="FeatureResult{TResult}"/> representing a failure with the reason set to <see
    /// cref="EFeatureFailureReason.Busy"/>.</returns>
    public static FeatureResult<TResult> Busy() => Failure(EFeatureFailureReason.Busy);
    /// <summary>
    /// Creates a failed feature result that indicates the operation timed out.
    /// </summary>
    /// <returns>A <see cref="FeatureResult{TResult}"/> representing a failure due to a timeout condition.</returns>
    public static FeatureResult<TResult> Timeout() => Failure(EFeatureFailureReason.Timeout);
    /// <summary>
    /// Creates a result that indicates the feature operation failed due to an unknown reason.
    /// </summary>
    /// <remarks>Use this method when the cause of the feature failure cannot be determined or does not match
    /// any known failure reasons.</remarks>
    /// <returns>A <see cref="FeatureResult{TResult}"/> representing a failure with the reason set to <see
    /// cref="EFeatureFailureReason.Unknown"/>.</returns>
    public static FeatureResult<TResult> Unknown() => Failure(EFeatureFailureReason.Unknown);
    /// <summary>
    /// Creates a result that indicates the feature request was rejected.
    /// </summary>
    /// <returns>A <see cref="FeatureResult{TResult}"/> representing a rejected feature request. The result will indicate failure
    /// with the reason set to <see cref="EFeatureFailureReason.Rejected"/>.</returns>
    public static FeatureResult<TResult> Rejected() => Failure(EFeatureFailureReason.Rejected);
    /// <summary>
    /// Creates a result that represents a cancelled operation.
    /// </summary>
    /// <returns>A <see cref="FeatureResult{TResult}"/> indicating that the operation was cancelled.</returns>
    public static FeatureResult<TResult> Cancelled() => Failure(EFeatureFailureReason.Cancelled);
    /// <summary>
    /// Creates a failed feature result that indicates the operation could not be completed due to an invalid state.
    /// </summary>
    /// <returns>A <see cref="FeatureResult{TResult}"/> representing a failure caused by an invalid state.</returns>
    public static FeatureResult<TResult> InvalidState() => Failure(EFeatureFailureReason.InvalidState);
    /// <summary>
    /// Creates a result that represents a transport-level error, such as a network failure or connectivity issue.
    /// </summary>
    /// <remarks>Use this method to signal that an operation failed due to issues with the underlying
    /// transport layer, rather than application logic or data validation errors.</remarks>
    /// <returns>A <see cref="FeatureResult{TResult}"/> indicating a transport error has occurred.</returns>
    public static FeatureResult<TResult> TransportError() => Failure(EFeatureFailureReason.TransportError);
    /// <summary>
    /// Creates a failed feature result that indicates an unhandled exception has occurred.
    /// </summary>
    /// <returns>A <see cref="FeatureResult{TResult}"/> representing a failure due to an unhandled exception.</returns>
    public static FeatureResult<TResult> UnhandledException() => Failure(EFeatureFailureReason.UnhandledException);
    /// <summary>
    /// Creates a result that indicates the feature operation failed due to an invalid configuration.
    /// </summary>
    /// <returns>A <see cref="FeatureResult{TResult}"/> representing a failure caused by an invalid configuration.</returns>
    public static FeatureResult<TResult> InvalidConfiguration() => Failure(EFeatureFailureReason.InvalidConfiguration);

    private FeatureResult(bool isSuccess, TResult value, EFeatureFailureReason? reason)
    {
        IsSuccess = isSuccess;
        Value = value;
        Reason = reason;
    }

}
