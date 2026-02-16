using McpNetwork.Orchestrator.Interfaces;
using McpNetwork.Orchestrator.Models.Enums;
using McpNetwork.Orchestrator.Orchestration.Parallels;

namespace McpNetwork.Orchestrator.Orchestration;

/// <summary>
/// Orchestrator is a class that manages the execution of a series of orchestration steps defined by IOrchestrationStep instances.
/// </summary>
/// <typeparam name="TResult"></typeparam>
public sealed class Orchestrator<TResult>
{
    private readonly List<IOrchestrationStep> _steps = new();
    private Func<OrchestrationState, TResult>? _finalizer;

    /// <summary>
    /// Adds a step to the orchestrator.
    /// </summary>
    /// <param name="step"></param>
    /// <returns></returns>
    public Orchestrator<TResult> AddStep(IOrchestrationStep step)
    {
        _steps.Add(step);
        return this;
    }

    /// <summary>
    /// Ends the orchestration with a finalizer function that takes the final state and produces a result of type TResult.
    /// </summary>
    /// <param name="finalizer"></param>
    /// <returns></returns>
    public Orchestrator<TResult> EndsWith(Func<OrchestrationState, TResult> finalizer)
    {
        _finalizer = finalizer;
        return this;
    }

    /// <summary>
    /// Executes the orchestration steps in sequence, passing the OrchestrationContext and OrchestrationState to each step. If any step fails, it returns a failure result. If all steps succeed, it invokes the finalizer to produce the final result.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<OrchestratorResult<TResult>> ExecuteAsync(OrchestrationContext context, CancellationToken ct = default)
    {
        var state = new OrchestrationState();

        foreach (var step in _steps)
        {
            StepResult stepResult;

            try
            {
                stepResult = await step.ExecuteAsync(context, state);
            }
            catch (OperationCanceledException)
            {
                return OrchestratorResult<TResult>.Failure(EFeatureFailureReason.Cancelled);
            }
            catch (Exception ex)
            {
                return OrchestratorResult<TResult>.Failure(EFeatureFailureReason.UnhandledException);
            }

            if (!stepResult.Continue)
            {
                return OrchestratorResult<TResult>.Failure(stepResult.FailureReason ?? EFeatureFailureReason.Unknown);
            }
        }

        if (_finalizer is null)
        {
            return OrchestratorResult<TResult>.Failure(EFeatureFailureReason.InvalidConfiguration);
        }

        try
        {
            var businessResult = _finalizer(state);
            return OrchestratorResult<TResult>.Success(businessResult);
        }
        catch (Exception ex)
        {
            return OrchestratorResult<TResult>.Failure(EFeatureFailureReason.UnhandledException);
        }
    }

    /// <summary>
    /// Adds a parallel step to the orchestrator. 
    /// The build action allows you to configure multiple steps that will be executed in parallel. 
    /// The ParallelStepBuilder is used to define the steps that should run concurrently, and the 
    /// resulting ParallelOrchestrationStep is added to the orchestrator's steps.
    /// </summary>
    /// <param name="build"></param>
    /// <returns></returns>
    public Orchestrator<TResult> AddParallel(Action<ParallelStepBuilder> build)
    {
        var builder = new ParallelStepBuilder();
        build(builder);

        AddStep(new ParallelOrchestrationStep(builder.Build()));
        return this;
    }



}

