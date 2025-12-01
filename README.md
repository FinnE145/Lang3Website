Lang3 Website
=============

This repository contains the Flask-based web experience for **[Lang3](https://github.com/FinnE145/Lang3)**, a lightweight, statically typed, object-oriented scripting language that embraces flexible object structures, powerful default arguments for code blocks, and concise syntax for everyday scripting tasks. Lang3 is currently in active development (the name is still a placeholder!), and this site hosts the evolving documentation, download instructions, and a browser-based runner that lets you try the language on our servers.

## About Lang3
- **Language focus:** non-traditional OO model with static typing, comprehensive type prediction, and a flexible object/value system that covers classic OOP concepts without rigid class hierarchies.
- **Developer ergonomics:** helper operators for expansion/condensation, convenient defaults for functions and control structures, and quality-of-life improvements over mainstream scripting languages.
- **CLI usage:** `Lang3 [filepath] [args]` with flags such as `!t`/`!tokens`, `!a`/`!ast`, `!d`/`!debug`, and `!v`/`!verbose` for inspecting lexer/parser output.
- **Status:** early-stage, open to feedback—see the main repository for language source, compiler progress, and kernel experiments.

For an in-depth tour of syntax, structures, and roadmap items, read the Documentation notebook in the main repo and file enhancement suggestions there.

## What this site provides
- **Documentation hub:** Markdown files in `docs/` automatically render to HTML under `/docs/<page>` with Bootstrap-based templates. Set `ALWAYS_REGENERATE_HTML=False` in `app.py` if you want cached HTML during production deployments.
- **Run Lang3 in the browser:** `/run` exposes an Ace editor + file upload that submits code to an RQ queue (`RunCode`). Jobs execute `CompiledSource/Lang3` with the `!d` flag so users can inspect lexer and parser output.
- **Download/contact stubs:** `/download` and `/contact` pages let you share binaries or forms as they become available.
- **Deployment ready:** Docker images (Gunicorn + Flask + Redis + RQ worker) power production, with GitHub Actions + SSH already wired for continuous deployment.

## Architecture at a glance
- **Flask app (`app.py`):** Serves templates, converts Markdown docs to HTML on demand, and enqueues execution jobs.
- **Runner (`codeRunner.py`):** Invokes the Lang3 binary (`CompiledSource/Lang3`). Rebuild this binary from the main Lang3 repo whenever the compiler changes.
- **Background worker:** `rq worker RunCode` (see `worker.sh`) processes jobs, keeping long-running code off the web process.
- **Redis:** Shared queue backend; configure host/port/password via environment variables or Docker Compose.
- **Frontend assets:** `static/` includes the Lang3 Ace theme/mode (`static/run/src` and `static/run/theme.css`), Bootstrap styling, and file upload handling.

## Working with docs
1. Add or edit Markdown files in `docs/` (e.g., `docs/test.md`).
2. The `/docs/<name>` route will auto-render and cache them into `templates/docs/<name>.html` on first request (unless regeneration is forced).
3. Commit both the source `.md` and generated `.html` files if you want faster cold starts in production.

## Code execution flow
1. User submits code or uploads a `.l3`/`.lang3` file on `/run`.
2. Flask saves the code in `static/files/userCode/` for auditing.
3. The job queue stores work in Redis and the worker executes the Lang3 binary with `!d` (lexer + parser) output.
4. The browser polls `/run/getJob?jid=<id>` until completion, then displays stdout or friendly errors based on the return code.

## Contributing
- Issues/PRs that relate to the language/compiler should target the main [Lang3 repository](https://github.com/FinnE145/Lang3).
- For web-specific bugs (UI regressions, deployment, docs formatting), open an issue here.
