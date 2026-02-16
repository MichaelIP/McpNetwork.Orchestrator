namespace McpNetwork.Orchestrator.Models.Enums;

/// <summary>
/// The status of a feature execution
/// </summary>
public enum EFeatureStatus
{
    /// <summary>
    /// Success indicates that the feature execution completed successfully
    /// </summary>
    Success,
    /// <summary>
    /// Busy indicates that the feature is currently unable to process the request
    /// </summary>
    Busy,
    /// <summary>
    /// Rejected indicates that the feature execution was rejected
    /// </summary>
    Rejected,
    /// <summary>
    /// Failed indicates that the feature execution encountered an error or exception
    /// </summary>
    Failed,
}
