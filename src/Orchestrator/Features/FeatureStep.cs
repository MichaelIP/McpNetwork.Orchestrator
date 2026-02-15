using McpNetwork.Orchestrator.Interfaces;
using McpNetwork.Orchestrator.Models.Enums;
using McpNetwork.Orchestrator.Orchestration;

namespace McpNetwork.Orchestrator.Features;

/// <summary>
/// Represents an orchestration step that executes a feature with input and output handling within an orchestration
/// workflow.
/// </summary>
/// <remarks>FeatureStep is typically used to compose complex workflows by chaining feature executions, where each
/// step can transform state and pass results to subsequent steps. The input and output handlers allow for flexible
/// integration with the orchestration state. This class is sealed and cannot be inherited.</remarks>
/// <typeparam name="TInput">The type of input provided to the feature for execution.</typeparam>
/// <typeparam name="TOutput">The type of output produced by the feature after execution.</typeparam>
public sealed class FeatureStep<TInput, TOutput> : IOrchestrationStep
{
    private readonly IFeature<TInput, TOutput> _feature;
    private readonly Func<OrchestrationState, TInput> _inputFactory;
    private readonly Action<OrchestrationState, TOutput> _outputHandler;

    /// <summary>
    /// Initializes a new instance of the FeatureStep class with the specified feature, input factory, and output
    /// handler.
    /// </summary>
    /// <param name="feature">The feature to be executed as part of this step. Cannot be null.</param>
    /// <param name="inputFactory">A function that produces the input for the feature based on the current orchestration state. Cannot be null.</param>
    /// <param name="outputHandler">An action that processes the feature's output and updates the orchestration state accordingly. Cannot be null.</param>
    public FeatureStep(IFeature<TInput, TOutput> feature, Func<OrchestrationState, TInput> inputFactory, Action<OrchestrationState, TOutput> outputHandler)
    {
        _feature = feature;
        _inputFactory = inputFactory;
        _outputHandler = outputHandler;
    }

    /// <summary>
    /// Executes the feature asynchronously using the provided orchestration context and state.
    /// </summary>
    /// <param name="context">The orchestration context that provides runtime information and services for the current execution.</param>
    /// <param name="state">The current orchestration state used to generate input and receive output for the feature execution.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="StepResult"/>
    /// indicating the outcome of the step execution.</returns>
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


