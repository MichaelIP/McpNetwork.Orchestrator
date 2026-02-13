using McpNetwork.Orchestrator.Features;
using McpNetwork.Orchestrator.Interfaces;

namespace McpNetwork.Orchestrator.Orchestration.Parallels;

public sealed class ParallelStepBuilder
{
    private readonly List<IOrchestrationStep> _steps = new();

    public ParallelStepBuilder AddStep(IOrchestrationStep step)
    {
        _steps.Add(step);
        return this;
    }

    public ParallelStepBuilder AddFeature<TInput, TResult>(IFeature<TInput, TResult> feature, Func<OrchestrationState, TInput> input, Action<OrchestrationState, TResult> onSuccess)
    {
        _steps.Add(new FeatureStep<TInput, TResult>(feature, input, onSuccess));
        return this;
    }

    internal IReadOnlyList<IOrchestrationStep> Build() => _steps;
}
