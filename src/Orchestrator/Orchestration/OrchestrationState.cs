namespace McpNetwork.Orchestrator.Orchestration;

/// <summary>
/// Represents a strongly-typed, in-memory key-value store for managing orchestration state entries.
/// </summary>
/// <remarks>Use this class to store, retrieve, and update state values associated with unique string keys during
/// the execution of an orchestration. Each entry is type-safe and can be individually set, updated, or removed.
/// Attempting to store delegates as state values is not supported and will result in an exception. This class is not
/// thread-safe; concurrent access should be synchronized externally if used in multi-threaded scenarios.</remarks>
public sealed class OrchestrationState
{
    private readonly Dictionary<string, object> _state = new();

    /// <summary>
    /// Adds a new state entry with the specified key and value. Throws an exception if the key already exists.
    /// </summary>
    /// <typeparam name="T">The type of the value to associate with the specified key.</typeparam>
    /// <param name="key">The unique identifier for the state entry to add. Cannot be null.</param>
    /// <param name="value">The value to associate with the specified key.</param>
    /// <exception cref="InvalidOperationException">Thrown if an entry with the specified key already exists. Use Update() to modify an existing entry.</exception>
    public void Set<T>(string key, T value)
    {
        if (_state.ContainsKey(key))
        {
            throw new InvalidOperationException($"State '{key}' already exists. Use Update() if this is intentional.");
        }

        GuardAgainstDelegate(key, value);
        _state[key] = value!;
    }

    /// <summary>
    /// Updates the value associated with the specified key by applying the provided updater function.
    /// </summary>
    /// <remarks>Use this method to modify an existing state entry by providing a transformation function. The
    /// updater function receives the current value and should return the new value to store.</remarks>
    /// <typeparam name="T">The type of the value to update.</typeparam>
    /// <param name="key">The key of the state entry to update. Cannot be null.</param>
    /// <param name="updater">A function that takes the current value and returns the updated value. Cannot be null.</param>
    /// <exception cref="KeyNotFoundException">Thrown if the specified key does not exist in the state.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the value associated with the key is not of the expected type <typeparamref name="T"/>.</exception>
    public void Update<T>(string key, Func<T, T> updater)
    {
        if (!_state.TryGetValue(key, out var existing))
        {
            throw new KeyNotFoundException($"State '{key}' does not exist. Use Set() first.");
        }

        if (existing is not T typed)
        {
            throw new InvalidOperationException($"State '{key}' is of type {existing.GetType().Name}, expected {typeof(T).Name}.");
        }

        var updated = updater(typed);
        GuardAgainstDelegate(key, updated);
        _state[key] = updated!;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Replace<T>(string key, T value)
    {
        GuardAgainstDelegate(key, value);
        _state[key] = value!;
    }

    /// <summary>
    /// Retrieves the value associated with the specified key and casts it to the specified type.
    /// </summary>
    /// <typeparam name="T">The expected type of the value to retrieve.</typeparam>
    /// <param name="key">The key whose associated value is to be retrieved. Cannot be null.</param>
    /// <returns>The value associated with the specified key, cast to type T.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the specified key does not exist in the state.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the value associated with the specified key cannot be cast to type T.</exception>
    public T Get<T>(string key)
    {
        if (!_state.TryGetValue(key, out var value))
        {
            throw new KeyNotFoundException($"State '{key}' not found.");
        }

        if (value is not T typed)
        {
            throw new InvalidOperationException($"State '{key}' is of type {value.GetType().Name}, expected {typeof(T).Name}.");
        }

        return typed;
    }

    //// 5️⃣ Optional
    //public bool TryGet<T>(string key, out T value)
    //{
    //    if (_state.TryGetValue(key, out var raw) && raw is T typed)
    //    {
    //        value = typed;
    //        return true;
    //    }

    //    value = default!;
    //    return false;
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    public void Remove(string key) => _state.Remove(key);

    private static void GuardAgainstDelegate(string key, object? value)
    {
        if (value is Delegate)
        {
            throw new InvalidOperationException($"Attempted to store a delegate in orchestration state under key '{key}'.");
        }
    }
}
