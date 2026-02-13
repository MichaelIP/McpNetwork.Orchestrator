namespace McpNetwork.Orchestrator.Models.Enums;

public enum EFeatureFailureReason
{
    Busy,
    Timeout,
    Unknown,
    Rejected,
    Cancelled,
    InvalidState,
    TransportError,
    UnhandledException,
    InvalidConfiguration,
}
