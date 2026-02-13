using McpNetwork.Orchestrator.Interfaces;
using McpNetwork.Orchestrator.Models.Enums;
using McpNetwork.Orchestrator.Orchestration;

namespace McpNetwork.Orchestrator.Features;

internal sealed class StateFeatureStep<TInput, TOutput> : IOrchestrationStep
{
    private readonly IFeature<TInput, TOutput> _feature;
    private readonly Func<OrchestrationState, TInput> _inputFactory;
    private readonly Action<OrchestrationState, TOutput>? _outputHandler;

    public StateFeatureStep(IFeature<TInput, TOutput> feature, Func<OrchestrationState, TInput> inputFactory, Action<OrchestrationState, TOutput>? outputHandler = null)
    {
        _feature = feature;
        _inputFactory = inputFactory;
        _outputHandler = outputHandler;
    }

    public async Task<StepResult> ExecuteAsync(OrchestrationContext context, OrchestrationState state)
    {
        // Prepare input from orchestration state
        TInput input = _inputFactory(state);

        // Call the feature
        FeatureResult<TOutput> featureResult = await _feature.ExecuteAsync(input, context);

        if (!featureResult.IsSuccess)
            return StepResult.Fail(featureResult.Reason ?? EFeatureFailureReason.Unknown);

        // Optionally store output in state
        _outputHandler?.Invoke(state, featureResult.Value!);

        return StepResult.Next();
    }
}
