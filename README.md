# ShellMuse

This repository hosts the ShellMuse project. The goal is a local-first LLM coding assistant that runs tasks in a secure Docker sandbox.

The repository currently contains the basic solution scaffolding as defined in `SPEC.md`.

## Solution layout

```
ShellMuse.sln
src/
  ShellMuse.Core/
  ShellMuse.Cli/
  ShellMuse.Docker/
  ShellMuse.Runtime/
 tests/
  ShellMuse.Tests/
 runtimes/
 docs/
```

Each project targets **.NET 9.0** and will be expanded in future milestones.
