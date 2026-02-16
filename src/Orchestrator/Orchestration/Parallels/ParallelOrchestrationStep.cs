using McpNetwork.Orchestrator.Interfaces;

namespace McpNetwork.Orchestrator.Orchestration.Parallels;

/// <summary>
/// ParallelOrchestrationStep allows multiple orchestration steps to be executed in parallel. 
/// It takes a collection of IOrchestrationStep instances and executes them concurrently. 
/// If any of the steps fail (i.e., return a StepResult with Continue set to false), the ParallelOrchestrationStep will return 
/// the first failure result. 
/// If all steps succeed, it returns a StepResult indicating to continue to the next step. 
/// This class is useful for scenarios where multiple independent steps can be executed simultaneously, improving 
/// overall execution time while still handling failures appropriately.
/// </summary>
public sealed class ParallelOrchestrationStep : IOrchestrationStep
{
    private readonly List<IOrchestrationStep> _steps;

    /// <summary>
    /// Initializes a new instance of the ParallelOrchestrationStep
    /// </summary>
    /// <param name="steps"></param>
    public ParallelOrchestrationStep(IEnumerable<IOrchestrationStep> steps)
    {
        _steps = steps.ToList();
    }

    /// <summary>
    /// Executes all the orchestration steps in parallel
    /// </summary>
    /// <param name="context"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    public async Task<StepResult> ExecuteAsync(OrchestrationContext context, OrchestrationState state)
    {
        var tasks = _steps.Select(step => step.ExecuteAsync(context, state)).ToList();
        var results = await Task.WhenAll(tasks);

        // If any step failed, return the first failure
        var failed = results.FirstOrDefault(r => !r.Continue);
        return failed ?? StepResult.Next();
    }
}
