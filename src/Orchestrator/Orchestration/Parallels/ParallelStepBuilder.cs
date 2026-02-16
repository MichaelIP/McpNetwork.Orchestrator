using McpNetwork.Orchestrator.Features;
using McpNetwork.Orchestrator.Interfaces;

namespace McpNetwork.Orchestrator.Orchestration.Parallels;

/// <summary>
/// ParallelStepBuilder is a builder class for constructing a collection of IOrchestrationStep instances 
/// that can be executed in parallel. 
/// It provides methods to add individual steps or features with specified input and output handling. 
/// The Build method returns the list of steps that have been added, which can then be used to create 
/// a ParallelOrchestrationStep for concurrent execution. 
/// This builder simplifies the process of defining parallel steps in an orchestration by allowing for a 
/// fluent API to add various types of steps and features seamlessly.
/// </summary>
public sealed class ParallelStepBuilder
{
    private readonly List<IOrchestrationStep> _steps = new();

    /// <summary>
    /// Adds an IOrchestrationStep to the builder's collection of steps
    /// </summary>
    /// <param name="step"></param>
    /// <returns></returns>
    public ParallelStepBuilder AddStep(IOrchestrationStep step)
    {
        _steps.Add(step);
        return this;
    }

    /// <summary>
    /// Adds a feature to the builder by specifying the feature instance, a function to extract the input from the orchestration state, and an action to handle the result upon successful execution. 
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="feature"></param>
    /// <param name="input"></param>
    /// <param name="onSuccess"></param>
    /// <returns></returns>
    public ParallelStepBuilder AddFeature<TInput, TResult>(IFeature<TInput, TResult> feature, Func<OrchestrationState, TInput> input, Action<OrchestrationState, TResult> onSuccess)
    {
        _steps.Add(new FeatureStep<TInput, TResult>(feature, input, onSuccess));
        return this;
    }

    internal IReadOnlyList<IOrchestrationStep> Build() => _steps;
}
