using McpNetwork.Orchestrator.Interfaces;
using McpNetwork.Orchestrator.Models;
using McpNetwork.Orchestrator.Models.Enums;
using McpNetwork.Orchestrator.Orchestration;

namespace McpNetwork.Orchestrator.Features;

internal sealed class NoInputFeatureStep<TResult> : IOrchestrationStep
{
    private readonly IFeature<Unit, TResult> _feature;
    private readonly Action<OrchestrationState, TResult>? _output;

    public NoInputFeatureStep(IFeature<Unit, TResult> feature, Action<OrchestrationState, TResult>? output)
    {
        _feature = feature;
        _output = output;
    }

    public async Task<StepResult> ExecuteAsync(OrchestrationContext context, OrchestrationState state)
    {
        // Call the feature with Unit as input
        FeatureResult<TResult> featureResult = await _feature.ExecuteAsync(Unit.Value, context);

        if (!featureResult.IsSuccess)
            return StepResult.Fail(featureResult.Reason ?? EFeatureFailureReason.Unknown);

        _output?.Invoke(state, featureResult.Value!);

        return StepResult.Next();
    }
}
