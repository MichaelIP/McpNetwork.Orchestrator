using McpNetwork.Orchestrator.Models.Enums;

namespace McpNetwork.Orchestrator.Orchestration;

/// <summary>
/// Retry policy defines the parameters for retrying a feature execution when it returns a status of Busy or Rejected.
/// </summary>
public sealed class RetryPolicy
{
    /// <summary>
    /// Maximum number of retry attempts (excluding the first attempt).
    /// </summary>
    public int MaxRetries { get; }

    /// <summary>
    /// Delay between retries.
    /// </summary>
    public TimeSpan DelayBetweenRetries { get; }

    /// <summary>
    /// Initializes a new instance of the RetryPolicy class with default values: 3 retries and 100 milliseconds delay between retries.
    /// </summary>
    public RetryPolicy() : this(3, TimeSpan.FromMilliseconds(100)) { }

    /// <summary>
    /// Initializes a new instance of the RetryPolicy class with the specified maximum number of retries and delay between retries.
    /// </summary>
    /// <param name="maxRetries"></param>
    /// <param name="delayBetweenRetries"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public RetryPolicy(int maxRetries, TimeSpan delayBetweenRetries)
    {
        if (maxRetries < 0) throw new ArgumentOutOfRangeException(nameof(maxRetries));
        if (delayBetweenRetries < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(delayBetweenRetries));

        MaxRetries = maxRetries;
        DelayBetweenRetries = delayBetweenRetries;
    }

    /// <summary>
    /// Default policy: 3 retries, 1 second delay.
    /// </summary>
    public static RetryPolicy Default { get; } = new RetryPolicy(3, TimeSpan.FromMilliseconds(100));

    /// <summary>
    /// ShouldRetry determines whether a retry should be attempted based on the feature status. Retries are attempted for Busy and Rejected statuses.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public bool ShouldRetry(EFeatureStatus status) => status == EFeatureStatus.Busy || status == EFeatureStatus.Rejected;
}
