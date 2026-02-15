namespace McpNetwork.Orchestrator.Helpers;

/// <summary>
/// Provides a disposable object that executes a specified action when disposed.
/// </summary>
/// <remarks>Use this class to ensure that a delegate is invoked when the object is disposed, typically in a using
/// statement. This is useful for executing cleanup logic or releasing resources in a deterministic manner.</remarks>
public sealed class DisposableAction : IDisposable
{
    private readonly Action _onDispose;

    /// <summary>
    /// Initializes a new instance of the DisposableAction class that executes the specified action when disposed.
    /// </summary>
    /// <remarks>Use this constructor to create a disposable object that performs a custom action when
    /// disposed, typically for resource cleanup or to execute code at the end of a using block.</remarks>
    /// <param name="onDispose">The action to execute when the object is disposed. Cannot be null.</param>
    public DisposableAction(Action onDispose)
    {
        _onDispose = onDispose;
    }

    public void Dispose()
    {
        _onDispose();
    }

}
