# McpNetwork.Orchestrator

A lightweight, strongly-typed orchestration engine for composing
independent features into explicit, state-driven workflows.

Designed for clarity, safety, and zero magic.

------------------------------------------------------------------------

## Why McpNetwork.Orchestrator?

Modern applications often require coordinating multiple features into a
predictable execution flow.

Common orchestration approaches tend to rely on:

-   Hidden reflection\
-   Implicit state mutation\
-   Convention-based magic\
-   Complex pipeline abstractions

**McpNetwork.Orchestrator** takes a different approach.

It provides:

-   Explicit orchestration steps\
-   Strongly-typed feature composition\
-   Deterministic, state-driven execution\
-   Controlled and safe state mutation\
-   No hidden behavior

Everything is visible. Everything is intentional.

------------------------------------------------------------------------

## Installation

``` bash
dotnet add package McpNetwork.Orchestrator
```

------------------------------------------------------------------------

## Core Concepts

### IFeature\<TIn, TOut\>

A feature represents a single, isolated unit of behavior.

It:

-   Takes an input\
-   Produces an output\
-   Does not know about orchestration or global state

------------------------------------------------------------------------

### Orchestration

An orchestration composes multiple features into a workflow.

Each step:

-   Executes a feature\
-   Optionally reads from state\
-   Optionally writes to state

Execution order is deterministic and explicit.

------------------------------------------------------------------------

### OrchestrationState

A strongly controlled key-value store used during execution.

It supports:

-   `Set` -- Define a value (write-once)\
-   `Update` -- Modify an existing value\
-   `Replace` -- Explicit overwrite\
-   `Delete` -- Remove a value

State keys cannot be accidentally reused.

This prevents subtle bugs caused by silent overwrites.

------------------------------------------------------------------------

## Minimal Example

``` csharp
            var orchestrator = new Orchestrator<InventoryResult>()
                .AddStep(Steps.NoInput(canStartFeature))

                .AddParallel(p => p
                    .AddStep(Steps.NoInput(getInventory1, (s, r) => s.Set("Inventory1Items", r.Items!)))
                    .AddStep(Steps.NoInput(getInventory2, (s, r) => s.Set("Inventory2Items", r.Items!)))
                )

                .AddStep(
                    Steps.FromState(
                        mergeInventoriesFeature,
                        state => new MergeInventoryRequest
                        {
                            Sources = new[]
                            {
                                state.Get<List<string>>("Inventory1Items"),
                                state.Get<List<string>>("Inventory2Items")
                            }
                        },
                        (state, result) => state.Set("MergedInventory", result.Items)
                    )
                )

                .AddStep(
                    Steps.FromState(
                        processingFeature, 
                        s => new ProcessingRequest { Items = s.Get<List<string>>("MergedInventory") }, 
                        (s, r) => s.Set("finalResult", r)
                    )
                )


                .EndsWith(state =>
                {
                    var items = state.Get<ProcessingResponse>("finalResult");
                    return InventoryResult.Success(items.Items);
                });


            var context = new OrchestrationContext(CancellationToken.None, logger);
            var orchestrationResult = await orchestrator.ExecuteAsync(context);
```

Each step is explicit.\
State mutation is controlled.\
No hidden behavior.

------------------------------------------------------------------------

## State Safety Philosophy

State management is intentionally strict.

-   `Set` throws if the key already exists.\
-   `Update` throws if the key does not exist.\
-   `Replace` is explicit.\
-   Delegates cannot be stored in state.

The goal is to prevent accidental data corruption and enforce clarity.

------------------------------------------------------------------------

## Design Goals

-   Explicit over implicit\
-   Strong typing everywhere\
-   Deterministic execution\
-   No reflection-based magic\
-   No hidden conventions\
-   No runtime code generation

This library is intentionally minimal.

------------------------------------------------------------------------

## When To Use

-   Coordinating application-layer features\
-   Clean Architecture projects\
-   Domain workflows\
-   Deterministic pipelines\
-   Explicit orchestration scenarios

------------------------------------------------------------------------

## When Not To Use

-   Simple CRUD operations\
-   Distributed workflow engines\
-   Background job scheduling systems\
-   Full BPM solutions

------------------------------------------------------------------------

## License

MIT
