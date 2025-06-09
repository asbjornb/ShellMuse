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

## Quick start

1. Install the [.NET 9 SDK preview](https://dotnet.microsoft.com/download/dotnet/9.0).
2. Export your OpenAI API key:

   ```powershell
   $env:SHELLMUSE_OPENAIAPIKEY = "sk-..."
   ```

3. Build the solution (this runs the CSharpier formatter automatically):

   ```bash
   dotnet build
   ```

4. Ask a question:

   ```bash
   dotnet run --project src/ShellMuse.Cli -- ask "2+2"
   ```

5. Run a task inside the sandbox:

   ```bash
   dotnet run --project src/ShellMuse.Cli -- run "build the project"
   ```

## Usage

The `shellmuse` executable exposes two subcommands:

```bash
shellmuse [ask] <prompt>
shellmuse run <task> [--max-cost N --max-steps N] [-v]
```

The `run` command executes a simple planner loop with built-in tools
(`search`, `read_file`, `write_file`, `list_dir`, `build`, `test`, `commit`, `branch`, `finish`).

If the repository contains `.muse-rules.md` or `AGENTS.md`, their contents are
added to the prompt so the model follows your project-specific guidelines.

Configuration defaults come from `appsettings.json`. Secrets can live in Visual
Studio user secrets or environment variables prefixed with `SHELLMUSE_`.

