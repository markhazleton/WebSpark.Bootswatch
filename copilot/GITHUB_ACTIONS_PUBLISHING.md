# Publishing with GitHub Actions - Complete Guide

## ?? Overview

WebSpark.Bootswatch uses GitHub Actions to automatically build, test, and publish NuGet packages. This guide explains how the automation works and how to trigger a new release.

## ?? Prerequisites

### 1. NuGet API Key Setup

You need to configure your NuGet API key as a GitHub secret:

1. **Get NuGet API Key**:
   - Go to https://www.nuget.org/account/apikeys
   - Create a new API key with:
     - **Name**: WebSpark.Bootswatch GitHub Actions
     - **Glob Pattern**: `WebSpark.Bootswatch`
     - **Scopes**: Push new packages and package versions
     - **Expiration**: 365 days (or as appropriate)

2. **Add Secret to GitHub**:
   - Go to https://github.com/MarkHazleton/WebSpark.Bootswatch/settings/secrets/actions
   - Click "New repository secret"
   - Name: `NUGET_API_KEY`
   - Value: Paste your NuGet API key
   - Click "Add secret"

### 2. Verify GitHub Actions Permissions

Ensure GitHub Actions has necessary permissions:
- Go to Settings ? Actions ? General
- Under "Workflow permissions", select:
  - ? Read and write permissions
  - ? Allow GitHub Actions to create and approve pull requests

## ?? Automated Workflows

### Workflow 1: Build and Publish (.github/workflows/dotnet.yml)

**Triggers**:
- Push to `main` branch (build only)
- Push tags matching `v*` (build, test, and publish)
- Pull requests to `main` (build and test)
- Manual trigger via workflow_dispatch

**Jobs**:

#### 1. Build Job
- Runs on Ubuntu Latest
- Matrix strategy: Tests all frameworks (net8.0, net9.0, net10.0)
- Sets up .NET 8.0, 9.0, and 10.0 SDKs
- Restores dependencies
- Builds each framework separately
- Runs tests for each framework
- Uploads test results as artifacts

#### 2. Publish Job (Tag Triggers Only)
- Waits for build job to complete
- Only runs when a version tag is pushed
- Sets up all .NET SDKs
- Extracts version from git tag
- Updates version in project file
- Builds multi-framework package
- Validates package structure
- Pushes to NuGet.org
- Creates GitHub Release
- Attaches .nupkg and .snupkg files

#### 3. Test Summary Job
- Downloads all test results
- Displays test matrix in GitHub Actions summary

### Workflow 2: Multi-Framework Tests (.github/workflows/multi-framework-tests.yml)

**Triggers**:
- Push to `main` or `upgrade-to-NET10` branches
- Pull requests to `main`
- Manual trigger

**Jobs**:
- Separate job for each framework (.NET 8.0, 9.0, 10.0)
- Comprehensive test execution
- Test result artifacts per framework
- Summary matrix

## ?? Publishing a New Version

### Step 1: Update Version

Edit `WebSpark.Bootswatch/WebSpark.Bootswatch.csproj`:

```xml
<PropertyGroup>
  <Version>1.32.0</Version>
  <AssemblyVersion>1.32.0</AssemblyVersion>
  <FileVersion>1.32.0</FileVersion>
</PropertyGroup>
```

### Step 2: Update CHANGELOG.md

Add a new section for the version:

```markdown
## [1.32.0] - 2025-01-13

### Added
- Feature 1
- Feature 2

### Changed
- Change 1

### Fixed
- Bug fix 1
```

### Step 3: Commit and Push

```bash
git add WebSpark.Bootswatch/WebSpark.Bootswatch.csproj CHANGELOG.md
git commit -m "chore: bump version to 1.32.0"
git push origin main
```

### Step 4: Create and Push Tag

```bash
# Create annotated tag
git tag -a v1.32.0 -m "Release version 1.32.0"

# Push tag to GitHub
git push origin v1.32.0
```

### Step 5: Monitor GitHub Actions

1. Go to https://github.com/MarkHazleton/WebSpark.Bootswatch/actions
2. Watch the ".NET Build and Publish" workflow
3. Monitor the build, test, and publish jobs

## ?? Workflow Stages

### Stage 1: Build and Test (All Commits)

```
???????????????????????
?  Checkout Code      ?
???????????????????????
           ?
           ?
???????????????????????
?  Setup .NET SDKs    ?
?  (8.0, 9.0, 10.0)   ?
???????????????????????
           ?
           ?
???????????????????????
?  Restore & Build    ?
?  (All Frameworks)   ?
???????????????????????
           ?
           ?
???????????????????????
?  Run Tests          ?
?  (Matrix: 3 FWs)    ?
???????????????????????
           ?
           ?
???????????????????????
?  Upload Artifacts   ?
???????????????????????
```

### Stage 2: Publish (Tag Triggers Only)

```
???????????????????????
?  Extract Version    ?
?  from Tag           ?
???????????????????????
           ?
           ?
???????????????????????
?  Update Project     ?
?  File Version       ?
???????????????????????
           ?
           ?
???????????????????????
?  Build & Pack       ?
?  Multi-Framework    ?
???????????????????????
           ?
           ?
???????????????????????
?  Validate Package   ?
?  (All 3 Frameworks) ?
???????????????????????
           ?
           ?
???????????????????????
?  Push to NuGet.org  ?
???????????????????????
           ?
           ?
???????????????????????
?  Create GitHub      ?
?  Release            ?
???????????????????????
           ?
           ?
???????????????????????
?  Attach Packages    ?
?  to Release         ?
???????????????????????
```

## ? Package Validation

The workflow validates:

### Multi-Framework Support
- ? Package contains lib/net8.0/ folder with WebSpark.Bootswatch.dll
- ? Package contains lib/net9.0/ folder with WebSpark.Bootswatch.dll
- ? Package contains lib/net10.0/ folder with WebSpark.Bootswatch.dll
- ? Each assembly has appropriate size
- ? XML documentation present for each framework

### Package Metadata
- ? README.md included
- ? LICENSE included
- ? NOTICE.txt included
- ? Package icon (WebSpark.png) included
- ? NuSpec file valid

### Symbols Package
- ? .snupkg file created
- ? Contains debugging symbols for all frameworks

## ?? GitHub Release Format

The workflow automatically creates a GitHub Release with:

```markdown
# WebSpark.Bootswatch {version}

Multi-framework support for .NET 8.0, 9.0, and 10.0.

## ?? Multi-Framework Support

- ? .NET 8.0 (LTS)
- ? .NET 9.0 (STS)
- ? .NET 10.0 (Current)

## ?? Installation

dotnet add package WebSpark.Bootswatch --version {version}

## ?? Testing

- 11 unit tests
- 33 total test executions across 3 frameworks

## ?? Documentation

- Links to README, CHANGELOG, Demo Site

## ?? Resources

- NuGet package URL
- Repository URL
- Issues URL
```

**Attached Files**:
- WebSpark.Bootswatch.{version}.nupkg
- WebSpark.Bootswatch.{version}.snupkg

## ?? Monitoring and Verification

### During Workflow Execution

1. **Build Progress**: Check each framework builds successfully
2. **Test Results**: Review test pass/fail for each framework
3. **Package Validation**: Verify all checks pass
4. **NuGet Push**: Confirm successful upload
5. **Release Creation**: Verify GitHub release created

### After Workflow Completion

1. **NuGet.org** (wait 5-10 minutes):
   ```
   https://www.nuget.org/packages/WebSpark.Bootswatch/{version}
   ```

2. **GitHub Release**:
   ```
   https://github.com/MarkHazleton/WebSpark.Bootswatch/releases/tag/v{version}
   ```

3. **Test Installation**:
   ```bash
   mkdir test-install && cd test-install
   dotnet new console -f net8.0
   dotnet add package WebSpark.Bootswatch --version {version}
   dotnet build
   ```

## ?? Troubleshooting

### Problem: Workflow fails on test stage

**Solution**:
1. Check test results artifacts
2. Run tests locally: `.\run-multi-framework-tests.ps1`
3. Fix failing tests
4. Commit and push fixes
5. Delete and recreate tag if needed

### Problem: NuGet push fails

**Possible Causes**:
- Invalid API key ? Check secret configuration
- Version already exists ? Increment version
- Package validation failed ? Check build logs

**Solution**:
```bash
# Delete tag locally and remotely
git tag -d v1.32.0
git push origin :refs/tags/v1.32.0

# Fix issues, increment version if needed
# Create and push new tag
git tag -a v1.32.1 -m "Release version 1.32.1"
git push origin v1.32.1
```

### Problem: GitHub Release not created

**Possible Causes**:
- Insufficient GitHub Actions permissions
- GitHub token expired

**Solution**:
1. Check workflow permissions in Settings ? Actions
2. Enable "Read and write permissions"
3. Re-run workflow or create release manually

### Problem: Package validation fails

**Check**:
- Are all three framework assemblies present?
- Is package metadata correct?
- Are required files (README, LICENSE) included?

**Solution**:
1. Review build logs for specific validation errors
2. Test package creation locally: `dotnet pack -c Release`
3. Inspect package: `unzip -l WebSpark.Bootswatch.{version}.nupkg`

## ?? Security Best Practices

1. **API Key Rotation**:
   - Rotate NuGet API key annually
   - Update GitHub secret after rotation

2. **Secret Management**:
   - Never commit API keys
   - Use GitHub Secrets for sensitive data
   - Limit key permissions to minimum required

3. **Workflow Permissions**:
   - Review and minimize permissions
   - Use `permissions` key in workflows
   - Enable branch protection on `main`

## ?? Pre-Release Checklist

Before creating a release tag:

- [ ] Version updated in .csproj
- [ ] CHANGELOG.md updated with new version
- [ ] All tests passing locally
- [ ] Documentation updated
- [ ] README.md reflects new version features
- [ ] NuGet API key secret is valid
- [ ] GitHub Actions permissions are correct
- [ ] No pending changes in working directory

## ?? Manual Trigger

To manually trigger a workflow without creating a tag:

1. Go to Actions tab
2. Select ".NET Build and Publish" workflow
3. Click "Run workflow"
4. Select branch
5. Click "Run workflow" button

**Note**: Manual triggers will NOT publish to NuGet. Publishing only happens on tag pushes.

## ?? Support

For workflow issues:
- **GitHub Actions Logs**: Review detailed logs in Actions tab
- **GitHub Issues**: Report workflow problems
- **Repository Discussions**: Ask questions about CI/CD process

---

## ? Quick Reference

```bash
# Complete release process
git checkout main
git pull origin main

# Update version and docs
# Edit WebSpark.Bootswatch.csproj
# Edit CHANGELOG.md

git add .
git commit -m "chore: bump version to 1.32.0"
git push origin main

# Create and push release tag
git tag -a v1.32.0 -m "Release version 1.32.0"
git push origin v1.32.0

# Monitor GitHub Actions
# https://github.com/MarkHazleton/WebSpark.Bootswatch/actions

# Verify after 10 minutes
# https://www.nuget.org/packages/WebSpark.Bootswatch/1.32.0
```

---

**Ready to release!** Follow the steps above to publish version 1.32.0 using GitHub Actions automation.
