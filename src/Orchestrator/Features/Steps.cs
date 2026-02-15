using McpNetwork.Orchestrator.Interfaces;
using McpNetwork.Orchestrator.Models;
using McpNetwork.Orchestrator.Orchestration;

namespace McpNetwork.Orchestrator.Features;

/// <summary>
/// Provides factory methods for creating orchestration steps that execute features with various input and output
/// handling strategies.
/// </summary>
/// <remarks>The Steps class offers static methods to construct orchestration steps that integrate features into
/// an orchestration pipeline. These methods allow callers to specify how feature inputs are supplied—either from the
/// orchestration state, as a fixed value, or with no input—and optionally define how feature outputs are stored back
/// into the orchestration state. All methods return an IOrchestrationStep instance suitable for use in orchestrated
/// workflows.</remarks>
public static class Steps
{
    /// <summary>
    /// Creates a feature step whose input is computed from the orchestration state.
    /// </summary>
    /// <typeparam name="TInput">Type of the feature input.</typeparam>
    /// <typeparam name="TOutput">Type of the feature output.</typeparam>
    /// <param name="feature">The feature to execute.</param>
    /// <param name="inputFactory">Function to produce the feature input from orchestration state.</param>
    /// <param name="outputHandler">Optional action to store the feature output in orchestration state.</param>
    /// <returns>An orchestration step wrapping the feature.</returns>
    public static IOrchestrationStep FromState<TInput, TOutput>(IFeature<TInput, TOutput> feature, Func<OrchestrationState, TInput> inputFactory, Action<OrchestrationState, TOutput>? outputHandler = null)
        => new StateFeatureStep<TInput, TOutput>(feature, inputFactory, outputHandler);

    /// <summary>
    /// Creates a feature step whose input is provided explicitly at creation time.
    /// </summary>
    /// <typeparam name="TInput">Type of the feature input.</typeparam>
    /// <typeparam name="TOutput">Type of the feature output.</typeparam>
    /// <param name="feature">The feature to execute.</param>
    /// <param name="input">The fixed input value to pass to the feature.</param>
    /// <param name="outputHandler">Optional action to store the feature output in orchestration state.</param>
    /// <returns>An orchestration step wrapping the feature.</returns>
    public static IOrchestrationStep Create<TInput, TOutput>(IFeature<TInput, TOutput> feature, TInput input, Action<OrchestrationState, TOutput>? outputHandler = null)
        => new StateFeatureStep<TInput, TOutput>(feature, _ => input, outputHandler);

    /// <summary>
    /// Creates a feature step that requires no input (Unit) and optionally stores the output in orchestration state.
    /// </summary>
    /// <typeparam name="TOutput">Type of the feature output.</typeparam>
    /// <param name="feature">The feature to execute.</param>
    /// <param name="outputHandler">Optional action to store the feature output in orchestration state.</param>
    /// <returns>An orchestration step wrapping the feature.</returns>
    public static IOrchestrationStep NoInput<TOutput>(IFeature<Unit, TOutput> feature, Action<OrchestrationState, TOutput>? outputHandler = null)
        => new NoInputFeatureStep<TOutput>(feature, outputHandler);

}


