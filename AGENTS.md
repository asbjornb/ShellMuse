# ShellMuse Agents Guide

This file provides guidance for AI agents, including OpenAI Codex, contributing to this repository. The overall goals and solution layout are described in `SPEC.md`.

## Project Structure

- `/src` – C# source projects
  - `ShellMuse.Core` – domain services and APIs
  - `ShellMuse.Cli` – command line entry point
  - `ShellMuse.Docker` – sandbox orchestration helpers
  - `ShellMuse.Runtime` – tool executed inside the Docker container
- `/tests` – xUnit test projects
  - `ShellMuse.Tests` – unit and integration tests
- `/runtimes` – Dockerfiles and runtime assets
- `/docs` – documentation
- `ShellMuse.sln` – solution file referencing all projects

## Coding Conventions

- Use **C#** targeting **.NET 9** for all production code.
- Follow the existing style of short files and simple namespaces.
- Choose meaningful type, method, and variable names.
- Keep business logic in `ShellMuse.Core`; CLI and runtime projects should remain thin.

## Testing Requirements

This development environment does not include the .NET SDK, so running
`dotnet test` locally will fail. Instead, rely on the GitHub Actions
workflow, which automatically executes `dotnet test ShellMuse.sln` for
each pull request. You do not need to run tests manually.

## Pull Request Guidelines

1. Provide a clear description of the changes.
2. Reference any relevant issues, if applicable.
3. Keep PRs focused on a single concern.

## Programmatic Checks

There is no linter yet and no automated checks to run locally. The CI
pipeline will handle any build and test validation.
