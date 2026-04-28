# Git Commit Agent — Semantic Commit Convention

## Overview

This agent enforces **semantic commits** on this repository. Every commit message must follow the structure below:

```
<type>(<scope>): <short description>

[optional body]

[optional footer]
```

- **type**: one of the types listed below (lowercase)
- **scope**: optional context (e.g. `auth`, `ui`, `api`, `deps`)
- **short description**: imperative, present tense, no period at the end
- **body**: explain *what* and *why*, not *how* (wrap at 72 chars)
- **footer**: reference issues/PRs (e.g. `Closes #42`, `BREAKING CHANGE: ...`)

---

## Incremental Development Philosophy

To maintain high code quality and agility, we adhere to an incremental programming strategy:

- **Small Pull Requests**: Break down large features into small, manageable PRs.
- **Minimum Testable Code**: Each PR should represent the smallest possible unit of work that can be tested and verified.
- **Always Deployable**: Code must be merged in a state that doesn't break the application. This allows for safe, continuous deployments.
- **Simplified Review**: Small changes allow reviewers to focus better and provide higher-quality feedback.
- **Quick Recovery**: If a bug is introduced, reverting or fixing a small change is significantly faster and safer than handling a massive update.

---


## Commit Types

| Type | When to use | Affects versioning |
|------|-------------|-------------------|
| `feat` | Adding a new feature | MINOR |
| `fix` | Fixing a bug | PATCH |
| `docs` | Documentation changes only (no code changes) | — |
| `test` | Adding or modifying tests (no production code changes) | — |
| `build` | Changes to build files or external dependencies | — |
| `perf` | Performance improvements | PATCH |
| `style` | Formatting, semicolons, trailing spaces, lint (no logic changes) | — |
| `refactor` | Code restructuring that neither fixes a bug nor adds a feature | — |
| `chore` | Build tasks, admin configs, package manager updates (no code changes) | — |
| `ci` | Changes to CI/CD configuration and scripts | — |
| `raw` | Changes to config files, data files, feature flags or parameters | — |
| `cleanup` | Removing commented-out code, dead code, unnecessary snippets | — |
| `remove` | Deleting obsolete files, directories or deprecated features | — |

---

## Examples

```bash
# New feature
feat(home): add hero section with animated headline

# Bug fix
fix(navbar): correct mobile menu z-index overlap

# Documentation
docs(readme): update setup instructions for Next.js 15

# Tests
test(contact): add unit tests for form validation logic

# Build / dependencies
build(deps): upgrade next to 15.3.1

# Performance
perf(images): replace <img> with next/image for LCP improvement

# Style / formatting
style(globals): remove unused css variables and fix indentation

# Refactor
refactor(layout): extract header into standalone component

# Chore
chore(gitignore): add .env.local to ignored files

# CI
ci(github-actions): add lint step to pull request workflow

# Raw config / data
raw(config): update environment variable keys for production

# Cleanup
cleanup(about): remove commented-out placeholder content

# Remove
remove(legacy): delete unused pages from old portfolio version
```

---

## Rules

1. **Always** use one of the types from the table above — no custom types.
2. The type and description must be **lowercase**.
3. The short description must be written in the **imperative mood** ("add", "fix", "update" — not "added", "fixes", "updating").
4. **Do not** end the subject line with a period `.`
5. Keep the subject line under **72 characters**.
6. Separate subject from body with a **blank line**.
7. Use the body to explain *what changed* and *why*, not how.
8. Reference related issues or PRs in the footer.
9. Breaking changes must be indicated with `BREAKING CHANGE:` in the footer.
10. **Commit Granularity**: Prefer small, frequent commits that represent a single logical change. This makes history easier to follow and PRs easier to review.

---

## Breaking Changes

When a commit introduces a breaking change, add a footer entry:

```
feat(api): replace REST endpoints with tRPC

BREAKING CHANGE: all /api/* routes have been removed.
Consumers must migrate to the new tRPC client.

Closes #15
```

---

## Semantic Versioning Relationship

| Commit type | Version bump |
|-------------|-------------|
| `feat` | `1.X.0` — MINOR |
| `fix` / `perf` | `1.0.X` — PATCH |
| `BREAKING CHANGE` (any type) | `X.0.0` — MAJOR |
| All others | No bump |