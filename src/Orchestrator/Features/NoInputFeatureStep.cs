using McpNetwork.Orchestrator.Interfaces;
using McpNetwork.Orchestrator.Models;
using McpNetwork.Orchestrator.Models.Enums;
using McpNetwork.Orchestrator.Orchestration;

namespace McpNetwork.Orchestrator.Features;

/// <summary>
/// Represents an orchestration step that executes a feature with no input and processes its result within an
/// orchestration workflow.
/// </summary>
/// <remarks>This step is used when a feature does not require any input data and its result should be handled or
/// stored as part of the orchestration state. The feature is invoked with a unit value as input. If an output action is
/// provided, it is called with the orchestration state and the feature's result after successful execution.</remarks>
/// <typeparam name="TResult">The type of the result produced by the feature and passed to the output action.</typeparam>
internal sealed class NoInputFeatureStep<TResult> : IOrchestrationStep
{
    private readonly IFeature<Unit, TResult> _feature;
    private readonly Action<OrchestrationState, TResult>? _output;

    /// <summary>
    /// Initializes a new instance of the NoInputFeatureStep class with the specified feature and output action.
    /// </summary>
    /// <param name="feature">The feature to be executed as part of this step. Cannot be null.</param>
    /// <param name="output">An optional action to be invoked with the orchestration state and the feature result after execution. May be
    /// null if no output handling is required.</param>
    public NoInputFeatureStep(IFeature<Unit, TResult> feature, Action<OrchestrationState, TResult>? output)
    {
        _feature = feature;
        _output = output;
    }

    /// <summary>
    /// Executes the feature asynchronously within the specified orchestration context and updates the orchestration
    /// state based on the result.
    /// </summary>
    /// <param name="context">The orchestration context that provides runtime information and services for the execution.</param>
    /// <param name="state">The current state of the orchestration, which may be updated based on the feature's output.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a StepResult indicating the outcome
    /// of the execution.</returns>
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
