using McpNetwork.Orchestrator.Interfaces;
using McpNetwork.Orchestrator.Models.Enums;
using McpNetwork.Orchestrator.Orchestration.Parallels;

namespace McpNetwork.Orchestrator.Orchestration;

public sealed class Orchestrator<TResult>
{
    private readonly List<IOrchestrationStep> _steps = new();
    private Func<OrchestrationState, TResult>? _finalizer;

    public Orchestrator<TResult> AddStep(IOrchestrationStep step)
    {
        _steps.Add(step);
        return this;
    }

    public Orchestrator<TResult> EndsWith(Func<OrchestrationState, TResult> finalizer)
    {
        _finalizer = finalizer;
        return this;
    }

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

    public Orchestrator<TResult> AddParallel(Action<ParallelStepBuilder> build)
    {
        var builder = new ParallelStepBuilder();
        build(builder);

        AddStep(new ParallelOrchestrationStep(builder.Build()));
        return this;
    }



}

