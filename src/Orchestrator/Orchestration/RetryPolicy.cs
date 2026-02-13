using McpNetwork.Orchestrator.Models.Enums;

namespace McpNetwork.Orchestrator.Orchestration
{
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

        public RetryPolicy() : this(3, TimeSpan.FromMilliseconds(100)) { }

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

        public bool ShouldRetry(EFeatureStatus status) => status == EFeatureStatus.Busy || status == EFeatureStatus.Rejected;
    }
}
