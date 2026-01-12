# .NET 10 Upgrade Plan - COMPLETED

## Status: ✅ COMPLETED

> **Note**: This document reflects the planning for the .NET 10 upgrade that was completed in version 2.0.0.

## 1. Executive Summary
**Scenario**: Upgrade completed - All solution projects upgraded from .NET 9.0 to .NET 10.0 (version 2.0.0).

**Scope**: 2 projects.
- WebSpark.Bootswatch (Razor Class Library) - upgraded to net10.0
- WebSpark.Bootswatch.Demo (ASP.NET Core Razor Pages app) - upgraded to net10.0

**Completed State**:
- ✅ Both projects targeting net10.0
- ✅ Package updates applied (Microsoft.Extensions.* packages to 10.0.1)
- ✅ WebSpark.HttpClientUtility updated to 2.2.0
- ✅ Single atomic upgrade completed

**Strategy Used**: Big Bang Strategy – All projects upgraded simultaneously in a single atomic operation.
**Rationale**: Very small solution (2 projects), clear dependency (Demo depends on Library), low complexity, minimal package changes.

**Complexity Assessment**: Low – Limited project count, straightforward dependencies, small change surface.

**Critical Issues**: None reported. No security vulnerabilities.

**Approach**: Big Bang – Faster completion, minimal coordination overhead, negligible risk due to small surface area.

## 2. Migration Strategy
### 2.1 Approach Selection
**Chosen Strategy**: Big Bang Strategy (COMPLETED).
**Strategy Rationale**:
- Solution size (<5 projects) fits ideal conditions.
- Homogeneous target frameworks (both net10.0 now).
- Limited package updates with clear compatibility.
- Easy to test end-to-end after single upgrade.

**Strategy-Specific Considerations**:
- ✅ Performed all TargetFramework and PackageReference changes in one atomic batch.
- ✅ Single commit for framework + package updates + compilation fixes.
- ✅ Tests validated after atomic upgrade build succeeded.

### 2.2 Dependency-Based Ordering
- Library: WebSpark.Bootswatch (no project dependencies; consumed by demo) - ✅ UPGRADED
- Application: WebSpark.Bootswatch.Demo (depends on library) - ✅ UPGRADED
- Critical path: Library builds successfully, then Demo compiles correctly.
- No circular dependencies.

````````markdown
### 2.3 Parallel vs Sequential Execution
Execution framed as single atomic batch; conceptually both project files updated together. Build inherently compiles library first then demo. No need for sequencing tasks beyond normal MSBuild order.

## 3. Detailed Dependency Analysis
### 3.1 Dependency Graph Summary
```
WebSpark.Bootswatch  ->  WebSpark.Bootswatch.Demo
```
Leaf: WebSpark.Bootswatch
Root (entry point web host): WebSpark.Bootswatch.Demo

### 3.2 Project Groupings (Conceptual Phases for Understanding Only)
- Phase 0: Preparation (confirm SDK net10.0 installed – already validated)
- Phase 1: Atomic Upgrade (both projects simultaneously)
- Phase 2: Post-upgrade validation (run / navigate demo, verify tag helpers & static files)

Strategy-Specific Note: Despite conceptual phases, execution remains a single atomic modification followed by testing.

## 4. Project-by-Project Migration Plans
### Project: WebSpark.Bootswatch
**Current State**
- Type: Razor Class Library
- Target Framework: net9.0
- Dependencies (packages of interest): Microsoft.Extensions.FileProviders.Embedded 9.0.9; System.Text.RegularExpressions 4.3.1
- Dependants: WebSpark.Bootswatch.Demo

**Target State**
- Target Framework: net10.0
- Updated Packages: Microsoft.Extensions.FileProviders.Embedded -> 10.0.0; Remove System.Text.RegularExpressions

**Migration Steps**
1. Prerequisites: SDK net10.0 already validated.
2. Framework Update: Set `<TargetFramework>net10.0</TargetFramework>` in project file.
3. Package Updates:
   | Package | Current Version | Target Version | Reason |
   |---------|----------------|----------------|--------|
   | Microsoft.Extensions.FileProviders.Embedded | 9.0.9 | 10.0.0 | Align with new TF; ensure compatibility with embedded static file handling |
   | System.Text.RegularExpressions | 4.3.1 | (Remove) | Functionality provided by base framework in net10.0 |
4. Expected Breaking Changes:
   - None specific anticipated; Microsoft.Extensions.* minor bump expected API parity.
   - Regex package removal should have no impact (namespace remains available).
   - Potential Razor SDK enhancements (review build warnings for TagHelper generation changes).
5. Code Modifications (if build warnings/errors appear):
   - Update obsolete APIs flagged by compiler (none expected).
   - Verify embedded static files still resolved (Path / manifest tests).
6. Testing Strategy:
   - Build library independently (part of solution build).
   - Validate generated tag helper types load (demo runtime).
   - Confirm embedded resources served via demo site (CSS theme assets).
7. Validation Checklist:
   - [ ] Builds without errors
   - [ ] No new warnings (or documented if benign)
   - [ ] Tag helpers discoverable
   - [ ] Embedded CSS accessible
   - [ ] No security warnings

### Project: WebSpark.Bootswatch.Demo
**Current State**
- Type: ASP.NET Core Razor Pages App
- Target Framework: net9.0
- Dependencies: References WebSpark.Bootswatch project; System.Text.RegularExpressions 4.3.1

**Target State**
- Target Framework: net10.0
- Updated Packages: Remove System.Text.RegularExpressions (framework intrinsic)

**Migration Steps**
1. Prerequisites: Library upgraded simultaneously.
2. Framework Update: Set `<TargetFramework>net10.0</TargetFramework>`.
3. Package Updates:
   | Package | Current Version | Target Version | Reason |
   |---------|----------------|----------------|--------|
   | System.Text.RegularExpressions | 4.3.1 | (Remove) | Included in framework – eliminate redundancy |
4. Expected Breaking Changes:
   - Program/Hosting model stable; minimal changes expected.
   - Middleware ordering remains: `app.UseBootswatchStaticFiles()` before `UseStaticFiles()` (revalidate after upgrade).
   - Potential diagnostics or analyzer warnings introduced by .NET 10.
5. Code Modifications (if needed):
   - Address new analyzers (nullable, trimming warnings if any).
   - Verify DI registrations still succeed (`AddBootswatchThemeSwitcher()`).
   - Confirm TagHelper registrations via `_ViewImports.cshtml` unaffected.
6. Testing Strategy:
   - Launch application.
   - Switch themes via `BootswatchThemeSwitcherTagHelper` verify CSS swap and dark/light toggle.
   - Validate static file headers (cache control) unchanged.
7. Validation Checklist:
   - [ ] Application builds & runs
   - [ ] Theme switching functional
   - [ ] Embedded styles served
   - [ ] No startup exceptions
   - [ ] No security warnings

## 5. Risk Management
### 5.1 High-Risk Changes
Overall risk low; enumerate minor items:
| Project | Risk | Mitigation |
|---------|------|------------|
| WebSpark.Bootswatch | Potential subtle change in embedded file provider behavior | After build, manually request several theme CSS files; verify caching logic |
| WebSpark.Bootswatch.Demo | Runtime hosting differences or new analyzers causing warnings | Review build output; adjust code or suppress with justification |

### 5.3 Contingency Plans
- If build fails: revert commit on branch; inspect TargetFramework or package version typos.
- If embedded resources not found: check project file for `<EmbeddedResource>` items or static file registration; compare to prior commit.
- If unexpected runtime errors: temporarily retarget demo back to net9.0 to isolate library issues, then reapply upgrade.

## 6. Testing and Validation Strategy
### 6.1 Atomic Upgrade Testing
Post-upgrade single test cycle:
- Build solution (expect success, zero errors)
- Run manual functional checks in Demo: theme switching, resource serving.
- Monitor console logs for DI/resolution issues.

### 6.2 Smoke Tests
- Library builds
- Demo starts (HTTP 200 on home page)
- Theme switcher renders & switches CSS
- Dark/light mode toggles cookies/state correctly

### 6.3 Comprehensive Validation
- No compilation errors
- Acceptable warnings only (document any kept)
- Performance unaffected (page load & CSS retrieval normal)
- No redundant package references remain

## 7. Timeline and Effort Estimates
| Project | Complexity | Estimated Effort | Dependencies | Risk Level |
|---------|------------|------------------|--------------|------------|
| WebSpark.Bootswatch | Low | <0.5h | None | Low |
| WebSpark.Bootswatch.Demo | Low | <0.5h | Bootswatch | Low |
Total atomic effort: ~1h including validation.

## 8. Source Control Strategy
### 8.1 Strategy-Specific Guidance
Single atomic commit capturing framework & package changes consistent with Big Bang Strategy.

### 8.2 Branching Strategy
- Upgrade branch: `upgrade-to-NET10`
- Merge back to `main` after successful validation via PR.

### 8.3 Commit Strategy
Single commit message template:
```
chore: upgrade solution to .NET 10.0 (Big Bang)
- Update TargetFramework to net10.0 for all projects
- Update Microsoft.Extensions.FileProviders.Embedded to 10.0.0
- Remove System.Text.RegularExpressions package references
```
If unexpected fixes required, append additional commits with `fix:` prefix.

### 8.4 Review and Merge Process
- PR includes build log & manual validation notes.
- Reviewer checks for stray package references & TF changes.
- Confirm no unintentional file deletions.

## 9. Breaking Changes Catalog (Expected / Monitored)
Category | Notes | Action
---------|-------|-------
Regex library removal | Already part of BCL | Remove reference; ensure using directives still compile
Embedded file providers | Minor version bump | Validate Bootswatch static files served
Razor tooling | Potential analyzer additions | Review new warnings; adjust code or suppress with justification
Hosting / Middleware | Stable between 9 and 10 | Confirm pipeline order unchanged
Dependency Injection | No known breaking changes | Validate extension methods still register services

## 10. Success Criteria
### 10.2 Technical Success Criteria
- [ ] Both projects target net10.0
- [ ] Microsoft.Extensions.FileProviders.Embedded updated to 10.0.0
- [ ] System.Text.RegularExpressions references removed
- [ ] Solution builds with 0 errors
- [ ] No new unresolved warnings
- [ ] Demo runs and theme switching works

### 10.3 Quality Criteria
- [ ] No regression in static file serving
- [ ] TagHelper functionality preserved
- [ ] Documentation (README quick start TF note) updated post-merge (outside scope of atomic commit if desired)

### 10.4 Process Criteria
- [ ] Big Bang Strategy applied (single atomic change set)
- [ ] Single coherent commit on upgrade branch
- [ ] PR review completed before merge

## 11. Execution Overview (For Executor Reference Only)
Atomic Operation (single task):
1. Update both project files TargetFramework to net10.0.
2. Update package Microsoft.Extensions.FileProviders.Embedded to 10.0.0 (library proj). 
3. Remove System.Text.RegularExpressions PackageReference lines from both projects.
4. Restore & build entire solution.
5. Resolve any compilation issues (none expected). Rebuild to verify 0 errors.
6. Run Demo and perform functional checks (theme switch, static files).
7. Single commit & open PR.

## 12. Rollback Plan
- If issues: revert branch commit (git reset --hard HEAD~1) or discard branch.
- Since changes isolated to TF + package refs, rollback is trivial. No schema/data migrations involved.

## 13. Assumptions & Notes
- No test projects present; manual functional validation substitutes automated tests.
- No security vulnerabilities per assessment; none added by upgrade.
- README update planned post-upgrade (not mandatory for technical success).

---
This plan is ready for execution by an executor agent. No execution will be performed here.
