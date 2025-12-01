# Merging dev into main

This branch (`copilot/start-pr-from-dev-to-main`) has been prepared to merge the `dev` branch into `main`.

## Current Status

The branch structure is:
- `main`: Contains the initial clean commit (984fedc)
- `dev`: Contains all the web application code and features (8028556 and 470 files of changes)
- `copilot/start-pr-from-dev-to-main`: Based on `main` with `dev` merged in

## Changes Being Merged

This PR brings the complete Lang3 Website from dev to main, including:

1. **Flask Web Application** (`app.py`, `wsgi.py`)
   - Documentation rendering system
   - Code runner functionality
   - Download and contact pages

2. **Code Runner** (`codeRunner.py`, `worker.sh`)
   - Integration with Lang3 compiler
   - Background job processing with Redis/RQ
   - File upload handling

3. **Frontend Assets** (`static/`, `templates/`)
   - ACE editor integration with 200+ language modes
   - Multiple editor themes
   - Bootstrap-based responsive layouts
   - Lang3-specific syntax highlighting

4. **Documentation** (`docs/`)
   - Markdown source files
   - Auto-generated HTML templates
   - Lang3 language documentation

5. **Infrastructure** (`Dockerfile`, `docker-compose.yml`, `.github/workflows/`)
   - Docker containerization
   - GitHub Actions workflows
   - Deployment automation

6. **Comprehensive README**
   - Project overview
   - Architecture documentation
   - Usage instructions

## Note on PR Configuration

The existing PR #1 may need its base branch changed from `dev` to `main` via the GitHub web interface to properly represent this merge. Alternatively, a new PR can be created with:
- **Base**: `main`
- **Head**: `dev` or this branch

## Merge Conflict Resolution

During the merge, there was one conflict in `README.md`:
- `main` had: "Lang3Website" (single line)
- `dev` had: Comprehensive documentation
- **Resolution**: Kept the dev version (comprehensive README)

This was the only conflict, and it has been resolved in favor of the more complete dev version.
