# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

`Pozitron.QuerySpecification` is a .NET library for building query specifications (the Specification pattern). It ships as two NuGet packages:

- **`Pozitron.QuerySpecification`** (`src/QuerySpecification`) — core abstractions, the `Specification<T>` state container, builders, in-memory evaluators, validators, paging, and repository interfaces. No EF dependency.
- **`Pozitron.QuerySpecification.EntityFrameworkCore`** (`src/QuerySpecification.EntityFrameworkCore`) — EF Core plugin adding `IQueryable` evaluators (Include, tracking flags, split query, query tags, etc.) and the `WithSpecification` / `ToPagedResultAsync` extensions. References the base package and uses `InternalsVisibleTo` to reach its internals.

Both packages multi-target **net8.0; net9.0; net10.0** and share a version number (currently `11.5.0`, set per-`.csproj`).

## Commands

The solution file is `QuerySpecification.slnx` (the new XML solution format — there is no `.sln`). `dotnet` picks it up automatically in the repo root.

```bash
dotnet build                                  # build all (Debug)
dotnet build --configuration Release
dotnet test                                   # run all test projects
dotnet test --configuration Release --no-build --no-restore   # how CI runs it

# Run a single test project
dotnet test tests/QuerySpecification.Tests/QuerySpecification.Tests.csproj

# Run a single test or class (xunit filter)
dotnet test --filter "FullyQualifiedName~Builder_Where"
dotnet test --filter "DisplayName~specific_test_name"

# Coverage (installs ReportGenerator if missing, outputs HTML+badges to TestResults/)
./coverage.sh
./coverage.sh tests/QuerySpecification.Tests/QuerySpecification.Tests.csproj

# Clean bin/obj/.vs/TestResults; run `./clean.sh help` for options
./clean.sh all
```

Bash scripts (`*.sh`) are LF-only and assume a bash environment (Git Bash on Windows).

## Test infrastructure

- **xunit** + **FluentAssertions** (pinned to `[7.2.0]` — do not upgrade to 8.x, the API differs). Test projects target **net10.0 only**.
- `QuerySpecification.Tests` — unit tests for builders, in-memory evaluators, validators (no DB).
- `QuerySpecification.EntityFrameworkCore.Tests` — integration tests against a **real SQL Server**. `TestFactory` (in `Fixture/`) uses **SQL LocalDB** when installed; otherwise it falls back to a **Testcontainers Docker MSSQL** container. **Respawn** resets the DB between tests. Flip `_forceDocker` in `TestFactory.cs` to force Docker locally.
- `QuerySpecification.AutoDiscovery.Tests` — exercises the auto-discovery path (see below), which depends on assembly-level attributes, so it must be a separate project.
- `QuerySpecification.Benchmarks` — BenchmarkDotNet; run as a Release console app, not via `dotnet test`.

CI (`.github/workflows/ci.yml`) runs build + test on PRs to `main` using the .NET 10 SDK.

## Core architecture

### Specifications store all state in one `SpecItem[]`

The central design decision (rebuilt from the ground up in v11 for low allocation) is that a `Specification<T>` does **not** hold separate lists for Where/Order/Include/etc. Everything is packed into a single `SpecItem[] _items` array. `SpecItem` (`Internals/SpecItem.cs`) is a struct of `{ int Type; int Bag; object? Reference; }`.

- **`Type`** identifies the item kind. Built-in kinds use **negative** ids defined in `Internals/ItemType.cs` (`Where = -1`, `Order = -2`, `Include = -3`, `Like = -5`, `Select = -6`, `Compiled = -7`, `Paging`/`Flags = -8`, `QueryTag = -9`, `CacheKey = -10`). Note Paging and Flags deliberately share id `-8` — Paging lives in `Reference`, Flags in `Bag` — to save memory.
- **`Bag`** holds a small int payload (e.g. `OrderType`, `IncludeType`, `SpecFlags` bitmask, Like group number).
- **`Reference`** holds the expression/object.
- **Public `Add(type, value)` / `AddOrUpdate(type, value)`** let consumers attach arbitrary custom state using **positive** type ids (zero/negative are rejected). This is the extension mechanism — `FirstOrDefault<T>(type)` / `OfType<T>(type)` read it back.

`IsEmpty` (i.e. `_items is null`) is the fast-path guard checked everywhere; an empty spec is a no-op in evaluators. Array growth is deliberately small (initial size 2, grows by 4).

### Builders

`Specification<T>.Query` returns a fresh `SpecificationBuilder<T>` each access. The fluent API (`.Where`, `.Include`, `.OrderBy`, `.Like`, `.Skip`, etc.) is implemented as **extension methods** on `ISpecificationBuilder<T>` spread across `Builders/Builder_*.cs` (all part of `partial class SpecificationBuilderExtensions`). These call the internal `AddInternal` / `AddOrUpdateInternal` on the spec.

Conditional chains (`Include(...).ThenInclude(...)`, order chains) use a `[ThreadStatic] bool Specification<T>.IsChainDiscarded` flag instead of per-builder state — this is a memory optimization (avoids allocating order builders and shrinks include builders). Be careful preserving this when touching chain builders.

### Evaluators and validators (the two execution paths)

There are two parallel pipelines, each a list of small stateless singleton evaluators applied in order:

- **In-memory** (base package): `IMemoryEvaluator` over `IEnumerable<T>`, orchestrated by `SpecificationMemoryEvaluator` (`Evaluators/`). Entry points: `Specification<T>.Evaluate(IEnumerable<T>)`. Only Where/Like/Order apply in memory.
- **IQueryable / EF** (EF package): `IEvaluator` over `IQueryable<T>`, orchestrated by `SpecificationEvaluator`. Entry point: `IQueryable<T>.WithSpecification(spec)`. Adds Include (expression + string), query tags, and the EF flags.

Both orchestrators expose a static `.Default` singleton and a constructor taking a custom evaluator list. **Projections** (`Specification<T, TResult>` with `.Select`/`.SelectMany`) run the `Specification<T>` pipeline first, then apply the selector, then paging.

Validation (`IsSatisfiedBy`) is a third pipeline: `IValidator` + `SpecificationValidator` (Where/Like validators), evaluating a single entity in memory.

### Compiled expressions

`WhereExpressionsCompiled` / `OrderExpressionsCompiled` / `LikeExpressionsCompiled` lazily `.Compile()` the stored `Expression`s and cache the compiled delegates back into the spec as a single `ItemType.Compiled` item (an inner `SpecItem[]`). `GetCompiledItems()` in `Specification.cs` only recompiles when the compilable-item count changed, and groups compiled items contiguously by type. The in-memory evaluators use these compiled forms.

### Custom iterators

The `IEnumerable` projections over `_items` use hand-written iterators (`Internals/Iterator.cs`, `SpecIterator.cs`, `SpecSelectIterator.cs`) rather than `yield`/LINQ — a single object serves as both `IEnumerable` and `IEnumerator` (reused when safe via thread-id/state check) to avoid allocations on the hot path. Prefer these patterns over LINQ when adding new enumerations over spec items.

### Auto-discovery (opt-in)

By default the evaluator/validator lists are hard-coded in the orchestrator constructors. Auto-discovery is enabled **per consuming application** via an assembly attribute:

```csharp
[assembly: SpecAutoDiscovery("enable")]
```

When enabled, `Internals/TypeDiscovery.cs` scans loaded non-framework assemblies for `IEvaluator` / `IMemoryEvaluator` / `IValidator` implementations, ordered by `[EvaluatorDiscovery]` / `[ValidatorDiscovery]` (`DiscoveryAttribute.Order`, then type name). Implementations are resolved via parameterless ctor, a public static instance field, or a public static instance property. In Debug builds discovery failures throw `SpecAutoDiscoveryException`; in Release they fail silent (empty list).

### Repositories & paging

`IRepositoryBase<T>` / `IReadRepositoryBase<T>` / `IProjectionRepository` are abstractions only (no concrete EF implementation in the library — the EF tests provide a `Repository` fixture). Paging lives in `Paging/`: `PagingFilter`/`IPagingFilter` (input), `Pagination` (computed), `PagedResult<T>` (serializable output), `PaginationSettings`. `ToPagedResultAsync` (EF extension) does a `CountAsync` + page query.

## Conventions

- All public library code lives under namespace `Pozitron.QuerySpecification` (single namespace, regardless of folder). Tests use namespace `Tests`.
- `.editorconfig` is authoritative: **C# files use 4-space indent and UTF-8-BOM**; everything else 2-space. CRLF except shell scripts. Public APIs require XML doc comments (`GenerateDocumentationFile` is on — missing docs are warnings).
- Central package management: all versions are in `Directory.Packages.props`. EF versions are conditioned per target framework. Don't put `Version=` in individual `PackageReference`s.
- `exclusion.dic` is the spell-check dictionary (referenced by `.editorconfig`); add domain words there if the analyzer flags them.
- This is a performance-sensitive library — when modifying the `SpecItem` storage, builders, evaluators, or iterators, preserve the allocation-minimizing patterns and validate with the Benchmarks project.
