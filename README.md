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

## Usage

The `shellmuse` executable exposes two subcommands:

```bash
shellmuse [ask] <prompt>
shellmuse run <task> [--max-cost N --max-steps N] [-v]
```

The `run` command executes a simple planner loop with built-in tools
(`search`, `read_file`, `write_file`, `build`, `test`, `commit`, `branch`, `finish`).

Configuration defaults come from `appsettings.json`. Secrets can live in Visual
Studio user secrets or environment variables prefixed with `SHELLMUSE_`.

