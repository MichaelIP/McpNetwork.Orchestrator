using McpNetwork.Orchestrator.Interfaces;
using McpNetwork.Orchestrator.Models.Enums;
using McpNetwork.Orchestrator.Orchestration;

namespace McpNetwork.Orchestrator.Features;

internal sealed class CustomFeatureStep<TInput, TResult> : IOrchestrationStep
{
    private readonly IFeature<TInput, TResult> _feature;
    private readonly Func<OrchestrationState, TInput> _input;
    private readonly Action<OrchestrationState, TResult>? _output;

    public CustomFeatureStep(IFeature<TInput, TResult> feature, Func<OrchestrationState, TInput> input, Action<OrchestrationState, TResult>? output)
    {
        _feature = feature;
        _input = input;
        _output = output;
    }

    public async Task<StepResult> ExecuteAsync(OrchestrationContext context, OrchestrationState state)
    {
        var input = _input(state);
        FeatureResult<TResult> featureResult = await _feature.ExecuteAsync(input, context);

        if (!featureResult.IsSuccess)
            return StepResult.Fail(featureResult.Reason ?? EFeatureFailureReason.Unknown);

        _output?.Invoke(state, featureResult.Value!);
        return StepResult.Next();
    }
}
