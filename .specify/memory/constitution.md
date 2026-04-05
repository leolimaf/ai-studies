<!--
SYNC IMPACT REPORT
==================
Version change: N/A (initial ratification) → 1.0.0
Modified principles: N/A — first fill of template placeholders
Added sections: Core Principles (5), Development Constraints, Development Workflow, Governance
Removed sections: None

Templates status:
  ✅ .specify/templates/tasks-template.md — updated: removed test task phase note; tests marked NOT applicable
  ✅ .specify/templates/plan-template.md — updated: Testing field guidance reflects no automated tests
  ✅ .specify/templates/spec-template.md — no structural change required; "Independent Test" in user stories
      refers to manual validated scenarios, acceptable as-is

Deferred TODOs: None
-->

# AI Studies Constitution

## Core Principles

### I. Clarity Over Cleverness

Code MUST be written to be read by humans first, machines second.
Naming must be explicit and domain-aligned: variables, methods, and classes MUST reveal
intent without requiring comments to explain what they do.
Comments are permitted only to explain *why*, never *what*.
Tricky or overly compact expressions MUST be replaced with readable equivalents.

### II. Single Responsibility

Every class and every method MUST have exactly one reason to change.
Methods MUST be short enough to be understood at a glance (target: ≤20 lines).
Classes MUST not mix concerns (e.g., data access and business logic MUST be separate).
When a unit grows beyond its single purpose, it MUST be split immediately.

### III. Simplicity First (YAGNI)

The simplest solution that satisfies the current requirement MUST be chosen.
Abstractions, interfaces, and generic patterns MUST NOT be introduced speculatively.
No feature, field, or parameter shall exist without an immediate, validated use case.
Flat structures are preferred over deep hierarchies.

### IV. Consistent Naming and Style

Language-idiomatic conventions (C# PascalCase/camelCase) MUST be followed consistently
throughout the codebase.
Abbreviations are forbidden unless they are universally established domain acronyms
(e.g., `AI`, `LLM`, `HTTP`).
Formatting MUST be enforced via an automated formatter (e.g., `dotnet format`) so that
stylistic decisions never become a discussion point in reviews.

### V. No Automated Testing

This project does not include automated test suites (unit, integration, or contract tests).
Validation is performed manually through direct execution and exploratory interaction.
Test projects, mocking frameworks, and test runners MUST NOT be added to the solution.
Acceptance of a feature is confirmed by running the program and observing correct output.

## Development Constraints

- No test projects or test-related NuGet packages shall be added to the solution.
- Dependencies MUST be kept minimal: prefer standard library / framework capabilities before
  pulling in external packages.
- Complexity introduced beyond what the current feature demands MUST be explicitly justified
  in the PR description or commit message.
- Console output used for debugging MUST be removed before a feature is considered complete.

## Development Workflow

Before committing, the author MUST verify:

1. All identifiers (variables, methods, classes) clearly express their purpose.
2. No method exceeds its single responsibility.
3. No speculative abstractions were introduced.
4. Code compiles and runs without errors via `dotnet run`.
5. No leftover debug/temporary output remains.

Code reviews MUST focus on readability and simplicity. Style violations caught by
`dotnet format` are not valid review comments — run the formatter before opening a review.

## Governance

This constitution supersedes all other development guidelines for this repository.
Amendments require:
1. A documented rationale (why the principle is changing).
2. A version increment following semantic versioning:
   - MAJOR: removal or incompatible redefinition of a principle.
   - MINOR: new principle or section added.
   - PATCH: wording clarification or non-semantic refinement.
3. An update to `LAST_AMENDED_DATE`.

All feature planning (`plan.md`, `spec.md`, `tasks.md`) MUST be consistent with
the principles stated here. The Constitution Check gate in implementation plans
MUST reference principles by Roman numeral (e.g., "Principle V — No Automated Testing").

**Version**: 1.0.0 | **Ratified**: 2026-04-05 | **Last Amended**: 2026-04-05
