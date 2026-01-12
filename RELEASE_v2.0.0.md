# Git Tagging and Release Instructions for v2.0.0

## Version 2.0.0 - .NET 10 Exclusive Release

This guide provides the commands needed to tag and push version 2.0.0 to GitHub.

---

## Pre-Release Checklist

- [x] Updated version to 2.0.0 in WebSpark.Bootswatch.csproj
- [x] Updated README.md with breaking changes and migration guide
- [x] Updated CHANGELOG.md with comprehensive v2.0.0 entry
- [x] Updated .github/copilot-instructions.md
- [x] Build verified successful
- [x] NuGet package created: WebSpark.Bootswatch.2.0.0.nupkg

---

## Git Commands

### Step 1: Verify Current Status

```bash
# Check which files have been modified
git status

# Review changes
git diff
```

### Step 2: Stage and Commit Changes

```bash
# Stage all documentation and project files
git add README.md
git add CHANGELOG.md
git add .github/copilot-instructions.md
git add WebSpark.Bootswatch/WebSpark.Bootswatch.csproj
git add WebSpark.Bootswatch.Demo/WebSpark.Bootswatch.Demo.csproj
git add WebSpark.Bootswatch.Tests/WebSpark.Bootswatch.Tests.csproj

# Commit with descriptive message
git commit -m "Release v2.0.0 - .NET 10 Exclusive

BREAKING CHANGES:
- Dropped .NET 8.0 and .NET 9.0 support
- Now targets .NET 10.0 exclusively
- Requires WebSpark.HttpClientUtility 2.2.0+

Updated all Microsoft.Extensions.* packages to 10.0.1+
Comprehensive documentation updates explaining version strategy

We chose to support latest NuGet packages over broad framework 
compatibility. This prioritizes security, performance, and 
future-ready architecture as .NET 8 approaches EOL."
```

### Step 3: Create Annotated Tag

```bash
# Create annotated tag with detailed message
git tag -a v2.0.0 -m "Version 2.0.0 - .NET 10 Exclusive Release

BREAKING CHANGES:
- Dropped .NET 8.0 and .NET 9.0 support
- Now exclusively targets .NET 10.0
- Requires WebSpark.HttpClientUtility 2.2.0+

RATIONALE:
We prioritized latest NuGet packages over broad framework compatibility:
- Access to latest security patches and performance improvements
- All Microsoft.Extensions.* packages updated to 10.0.1+
- Simplified maintenance with single target framework
- .NET 8 approaching end of standard support (Nov 2025)
- .NET 9 has shorter STS lifecycle

MIGRATION:
1. Update to .NET 10: <TargetFramework>net10.0</TargetFramework>
2. Update packages: WebSpark.Bootswatch 2.0.0, WebSpark.HttpClientUtility 2.2.0
3. No code changes required - all APIs remain backward compatible

NEED LEGACY SUPPORT?
Use WebSpark.Bootswatch 1.34.0 for .NET 8/9/10 compatibility

Package Updates:
- Microsoft.Extensions.FileProviders.Embedded: 10.0.1
- Microsoft.Extensions.DependencyInjection: 10.0.1  
- Microsoft.Extensions.Logging: 10.0.1
- Microsoft.AspNetCore.Mvc.Testing: 10.0.0

Documentation:
- Updated README.md with breaking changes section
- Added comprehensive CHANGELOG.md entry
- Updated copilot-instructions.md
- Added migration guide and version support matrix"
```

### Step 4: Push to GitHub

```bash
# Push the commit
git push origin main

# Push the tag
git push origin v2.0.0
```

### Step 5: Verify Tag on GitHub

Visit: https://github.com/MarkHazleton/WebSpark.Bootswatch/tags

---

## Create GitHub Release

After pushing the tag, create a release on GitHub:

1. Go to: https://github.com/MarkHazleton/WebSpark.Bootswatch/releases/new
2. Select tag: `v2.0.0`
3. Release title: `v2.0.0 - .NET 10 Exclusive Release`
4. Copy the release notes below:

### Release Notes Template

```markdown
## ?? Version 2.0.0 - Breaking Changes

**WebSpark.Bootswatch 2.0 now exclusively targets .NET 10.**

### Why This Change?

We chose to **support the latest packages over broad framework compatibility**:

? **Security & Performance**: Latest .NET 10 security patches and performance improvements
? **Modern Dependencies**: All Microsoft.Extensions.* packages at 10.0.1+
? **Simplified Maintenance**: Single target framework reduces complexity
? **Future-Ready**: .NET 10 is current with longest support timeline
? **.NET 8 Approaching EOL**: Standard support ends November 2025
? **.NET 9 Short Lifecycle**: STS has shorter timeline

### Breaking Changes

- ? Dropped .NET 8.0 and .NET 9.0 support
- ? Now requires .NET 10.0
- ? Requires WebSpark.HttpClientUtility 2.2.0+

### Migration Guide

**1. Update Target Framework:**
```xml
<PropertyGroup>
  <TargetFramework>net10.0</TargetFramework>
</PropertyGroup>
```

**2. Update Package References:**
```xml
<PackageReference Include="WebSpark.Bootswatch" Version="2.0.0" />
<PackageReference Include="WebSpark.HttpClientUtility" Version="2.2.0" />
```

**3. No Code Changes Required**
All public APIs remain backward compatible!

### Need .NET 8 or 9 Support?

Continue using **WebSpark.Bootswatch 1.34.0**:

```bash
dotnet add package WebSpark.Bootswatch --version 1.34.0
dotnet add package WebSpark.HttpClientUtility --version 2.1.1
```

### Package Updates

- Microsoft.Extensions.FileProviders.Embedded: 10.0.1
- Microsoft.Extensions.DependencyInjection: 10.0.1
- Microsoft.Extensions.Logging: 10.0.1
- Microsoft.AspNetCore.Mvc.Testing: 10.0.0

### Documentation

- ? Updated README.md with breaking changes notice
- ? Comprehensive CHANGELOG.md
- ? Migration guide and version support matrix
- ? Updated all project references

### Installation

```bash
dotnet add package WebSpark.Bootswatch --version 2.0.0
dotnet add package WebSpark.HttpClientUtility --version 2.2.0
```

### Links

- ?? [NuGet Package](https://www.nuget.org/packages/WebSpark.Bootswatch/2.0.0)
- ?? [Demo Site](https://bootswatch.markhazleton.com/)
- ?? [Documentation](https://github.com/MarkHazleton/WebSpark.Bootswatch/wiki)
- ?? [CHANGELOG](https://github.com/MarkHazleton/WebSpark.Bootswatch/blob/main/CHANGELOG.md)
```

---

## NuGet Package Publishing

### Package Location

```
.\nupkg\WebSpark.Bootswatch.2.0.0.nupkg
```

### Publish to NuGet.org

```bash
# Option 1: Using dotnet CLI with API key
dotnet nuget push .\nupkg\WebSpark.Bootswatch.2.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json

# Option 2: Upload manually at https://www.nuget.org/packages/manage/upload
```

---

## Verification Steps

After release:

1. ? Verify tag appears on GitHub: https://github.com/MarkHazleton/WebSpark.Bootswatch/tags
2. ? Verify release is published: https://github.com/MarkHazleton/WebSpark.Bootswatch/releases
3. ? Verify NuGet package is live: https://www.nuget.org/packages/WebSpark.Bootswatch/
4. ? Test installation in a new .NET 10 project:
   ```bash
   dotnet new webapp -n TestWebSparkBootswatch
   cd TestWebSparkBootswatch
   dotnet add package WebSpark.Bootswatch --version 2.0.0
   dotnet add package WebSpark.HttpClientUtility --version 2.2.0
   dotnet build
   ```

---

## Rollback Plan

If issues are discovered after release:

1. **Critical Issue**: Unlist v2.0.0 from NuGet (doesn't delete, just hides)
2. **Fix and Release**: Create v2.0.1 with fixes
3. **Documentation**: Update CHANGELOG.md with v2.0.1 entry

---

## Version Support Matrix

| Version | .NET Target | Support Status | End of Support |
|---------|-------------|----------------|----------------|
| 2.0.x   | .NET 10     | ? Active      | TBD            |
| 1.34.x  | .NET 8/9/10 | ?? Maintenance | Nov 2025       |

---

## Communication

Consider announcing v2.0.0 release:

- ?? GitHub Discussions
- ?? Twitter/X
- ?? LinkedIn
- ?? Dev.to or Medium blog post
- ?? Update demo site with version banner

---

**Ready to Release? Follow Steps 1-4 above!**
