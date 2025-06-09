# ShellMuse MVP – Specification

## 1. Purpose

ShellMuse is a local-first LLM coding agent that can:

- Read & reason about the caller's Git repository
- Plan multi-step tool invocations (search, edit, build, test, commit)
- Execute those steps inside a disposable Docker sandbox
- Stream progress back to the user

The MVP targets Windows 10/11 with PowerShell as the primary shell, implemented in .NET 9. Execution happens inside Linux Docker containers. While the first UI is a command-line app, all domain logic lives in `ShellMuse.Core` so that a future web/TUI front-end can reuse it.

## 2. Platforms & Runtime

| Item | Requirement |
|------|-------------|
| Host OS | Windows 10+ (x64) |
| Host Shell | PowerShell 7+ |
| CLI | .NET 9 single-file publish (shellmuse.exe) |
| Container OS | Linux (Docker Desktop ≥ 4.30) |
| CI/CD | GitHub Actions on Linux for builds/tests |

## 3. Functional Requirements

| ID | Description | Acceptance Criteria |
|----|-------------|-------------------|
| F-0 | Core API layer – All services (planner, executor, provider abstraction, sandbox orchestration) reside in ShellMuse.Core. UI projects reference this package only. | Web or CLI sample can call the same Core methods. |
| F-1 | Single CLI entry-point `shellmuse [ask\|run]` | Running `shellmuse ask "hello"` streams an answer. |
| F-2 | Config discovery (flags > env > TOML) | Changing `SHELLMUSE_MODEL` affects next run. |
| F-3 | OpenAI provider with streaming | Successful chat completion returned. |
| F-4 | Rules file `.muse-rules.md` auto-injected | Model response reflects rule changes. |
| F-5 | Context capture (branch, diff, file list) | Prompt log shows captured context. |
| F-6 | Git branch workflow – Always work on a Git branch, mount repo to container with RW access | Agent creates/switches to branch before making changes |
| F-7 | Docker sandbox execution (see §6) | Host FS Git repo modified only through container |
| F-8 | Planner–executor loop (≤ 10 iterations) | Demo task finishes within step limit |
| F-9 | Built-in tool palette: search, read_file, write_file, list_dir, build, test, commit, outdated_packages | Invalid tool aborts with error; commits allowed on branch |
| F-10 | Cost & iteration guardrails (--max-cost, --max-steps) | Exceeding guard exits with code 2 |
| F-11 | Windows/PowerShell compatibility – All paths, temp dirs work on Windows with PowerShell | CI job on Linux passes build/test acceptance |
| F-12 | Repository `AGENTS.md` auto-injected | Model responses follow repo instructions. |

## 4. Security Requirements

| ID | Description |
|----|-------------|
| S-1 | Docker run flags: `--network none --read-only --cap-drop ALL --pids-limit 200 --tmpfs /tmp` |
| S-2 | Mount Git repo `C:\src\project` ➜ `/workspace` with RW access for branch operations |
| S-3 | Git safety – Always work on a branch, never directly on main/master |
| S-4 | Strip all env vars except allow-list before container entry |
| S-5 | Redact secrets (***) in logs |
| S-6 | Container auto-removed (--rm); no daemon-side caches |

## 5. Non-Functional Requirements

- **Performance** – < 10s p95 latency for 100-token ask call
- **Extensibility** – Adding a new LLM provider or front-end does not modify existing code; compile-time DI via IServiceCollection
- **Observability** – Structured logs (Serilog in JSON); optional Prometheus metrics
- **Testing** – xUnit test project `ShellMuse.Tests` must exercise planner, sandbox, and provider mocks; unit + integration tests run in GitHub Actions on Linux for CI/CD
- **Documentation** – `docs/` contains setup, FAQ, architecture diagrams, and a "How to write plugins" guide

## 6. Docker Sandbox Flow

```
+--------------+            docker run                         +-------------------+
| Windows Host |---------------------------------------------->|  Linux Container  |
| PowerShell   |   bind-mount git repo at /workspace (RW)      |  /bin/agent-tool  |
+--------------+  json tool request  ───────────────▶          +-------------------+
       ▲              ◀── json result / stdout / stderr               │
       └──────────── results streamed to user ─────────────────────────┘
                               │
                    Git operations on branch
                    (commits allowed)
```

**Base image**: `mcr.microsoft.com/dotnet/nightly/sdk:9.0` (SDK, git, ripgrep)

## 7. Interfaces

### 7.1 CLI

```bash
shellmuse [ask] <prompt>
shellmuse run  <task> [--max-cost 1.0 --max-steps 8] [--model gpt-4o] [-v]
```

### 7.2 Config (~/.config/shellmuse/config.toml)

```toml
model         = "gpt-4o"
temperature   = 0.2
max_tokens    = 2048
docker_image  = "mcr.microsoft.com/dotnet/nightly/sdk:9.0"
```

### 7.3 Tool JSON Schema (sent to model)

```json
{
  "type": "object",
  "properties": {
    "tool": { "enum": [ "search","read_file","write_file","list_dir","build","test","commit","branch","outdated_packages","finish" ] },
    "args": { "type": "object" }
  },
  "required": ["tool"]
}
```

## 8. Solution Layout

```
/shellmuse
 ├─ src/
 │   ├─ ShellMuse.Core/         # domain & services
 │   ├─ ShellMuse.Cli/          # thin console front-end
 │   ├─ ShellMuse.Docker/       # sandbox orchestration helpers
 │   └─ ShellMuse.Runtime/      # agent-tool for container
 ├─ tests/
 │   └─ ShellMuse.Tests/
 ├─ runtimes/                   # Dockerfiles
 └─ docs/
```

## 9. Acceptance Tests

- **Smoke**: `shellmuse ask "2+2"` → 4, exit 0
- **Sandbox build**: In sample repo failing test, `shellmuse run "fix test"` → passes inside container, commits to branch
- **Budget abort**: `--max-cost 0.0001` triggers abort, exit 2
- **Rules honored**: `.muse-rules.md` forbids deletions; plan with deletion rejected
- **Git workflow**: Agent creates branch, makes changes, commits successfully
- **Windows PowerShell**: Temp directories created under `$env:TEMP` without path issues

## 10. Out-of-Scope (MVP)

- Local Llama / Claude providers
- Vector-store embeddings  
- TUI / Web front-end

## 11. Milestones

| Deliverable |
|-------------|
| Use local OLlama llm for some stuff? |
| Nuget tool to find latest package versions? |
| Include AGENTS.md instructions in prompt |
