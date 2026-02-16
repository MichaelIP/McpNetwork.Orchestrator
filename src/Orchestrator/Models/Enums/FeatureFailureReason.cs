namespace McpNetwork.Orchestrator.Models.Enums;

/// <summary>
/// Indicates the reason for a feature failure
/// </summary>
public enum EFeatureFailureReason
{
    /// <summary>
    /// Busy indicates that the feature is currently unable to process the request
    /// </summary>
    Busy,
    /// <summary>
    /// Timeout indicates that the feature execution exceeded the allowed time limit for completion
    /// </summary>
    Timeout,
    /// <summary>
    /// Unknown indicates that the reason for the feature failure is not specified or cannot be determined
    /// </summary>
    Unknown,
    /// <summary>
    /// Rejected indicates that the feature execution was rejected
    /// </summary>
    Rejected,
    /// <summary>
    /// Cancelled indicates that the feature execution was cancelled
    /// </summary>
    Cancelled,
    /// <summary>
    /// InvalidState indicates that the feature execution failed due to being in an invalid state
    /// </summary>
    InvalidState,
    /// <summary>
    /// TransportError indicates that the feature execution failed due to a transport error
    /// </summary>
    TransportError,
    /// <summary>
    /// UnhandledException indicates that the feature execution failed due to an unhandled exception
    /// </summary>
    UnhandledException,
    /// <summary>
    /// InvalidConfiguration indicates that the feature execution failed due to an invalid configuration
    /// </summary>
    InvalidConfiguration,
}
