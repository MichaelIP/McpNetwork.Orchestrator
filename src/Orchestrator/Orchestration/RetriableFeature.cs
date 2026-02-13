using McpNetwork.Orchestrator.Features;
using McpNetwork.Orchestrator.Interfaces;

namespace McpNetwork.Orchestrator.Orchestration
{
    public abstract class RetriableFeature<TInput, TResult> : IRetriableFeature<TInput, TResult> 
    {
        
        public virtual RetryPolicy RetryPolicy { get; }

        protected RetriableFeature(RetryPolicy? retryPolicy = null)
        {
            RetryPolicy = retryPolicy ?? new RetryPolicy();
        }

        public async Task<FeatureResult<TResult>> ExecuteAsync(TInput input, OrchestrationContext context)
        {
            int attempt = 0;

            while (true)
            {
                attempt++;
                try
                {
                    context.Tracer?.TraceInfo(context.OrchestrationId,GetType().Name, $"Attempt {attempt}");

                    return await ExecuteCoreAsync(input, context);
                }
                catch (Exception ex) when (attempt <= RetryPolicy.MaxRetries)
                {
                    context.Tracer?.TraceError(context.OrchestrationId, GetType().Name, ex);

                    await Task.Delay(RetryPolicy.DelayBetweenRetries, context.CancellationToken);
                }
            }
        }

        protected abstract Task<FeatureResult<TResult>> ExecuteCoreAsync(TInput input, OrchestrationContext context);

    }

}
