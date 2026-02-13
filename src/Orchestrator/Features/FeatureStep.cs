using McpNetwork.Orchestrator.Interfaces;
using McpNetwork.Orchestrator.Models.Enums;
using McpNetwork.Orchestrator.Orchestration;

namespace McpNetwork.Orchestrator.Features;

public sealed class FeatureStep<TInput, TOutput> : IOrchestrationStep
{
    private readonly IFeature<TInput, TOutput> _feature;
    private readonly Func<OrchestrationState, TInput> _inputFactory;
    private readonly Action<OrchestrationState, TOutput> _outputHandler;

    public FeatureStep(IFeature<TInput, TOutput> feature, Func<OrchestrationState, TInput> inputFactory, Action<OrchestrationState, TOutput> outputHandler)
    {
        _feature = feature;
        _inputFactory = inputFactory;
        _outputHandler = outputHandler;
    }

    public async Task<StepResult> ExecuteAsync(OrchestrationContext context, OrchestrationState state)
    {
        var input = _inputFactory(state);
        var result = await _feature.ExecuteAsync(input, context);

        if (!result.IsSuccess)
            return StepResult.Fail(result.Reason! ?? EFeatureFailureReason.Unknown);

        _outputHandler(state, result.Value);
        return StepResult.Next();
    }
}


