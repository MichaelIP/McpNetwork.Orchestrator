namespace McpNetwork.Orchestrator.Models;

/// <summary>
/// Unit is a struct that represents the absence of a value. 
/// It is used in scenarios where a method or operation needs to return a value, but there is no 
/// meaningful data to return. 
/// </summary>
public readonly struct Unit
{
    /// <summary>
    /// Value is a static readonly instance of the Unit struct, representing the single value of this type.
    /// </summary>
    public static readonly Unit Value = new();
}
