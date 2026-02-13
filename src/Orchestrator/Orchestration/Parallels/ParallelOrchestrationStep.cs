using McpNetwork.Orchestrator.Interfaces;

namespace McpNetwork.Orchestrator.Orchestration.Parallels;

public sealed class ParallelOrchestrationStep : IOrchestrationStep
{
    private readonly List<IOrchestrationStep> _steps;

    public ParallelOrchestrationStep(IEnumerable<IOrchestrationStep> steps)
    {
        _steps = steps.ToList();
    }

    public async Task<StepResult> ExecuteAsync(OrchestrationContext context, OrchestrationState state)
    {
        var tasks = _steps.Select(step => step.ExecuteAsync(context, state)).ToList();
        var results = await Task.WhenAll(tasks);

        // If any step failed, return the first failure
        var failed = results.FirstOrDefault(r => !r.Continue);
        return failed ?? StepResult.Next();
    }
}
