# Publishing WebSpark.Bootswatch 1.31.0 to NuGet.org

## ?? Package Information

- **Package Name**: WebSpark.Bootswatch
- **Version**: 1.31.0
- **Package File**: `nupkgs/WebSpark.Bootswatch.1.31.0.nupkg` (12.4 MB)
- **Symbols File**: `nupkgs/WebSpark.Bootswatch.1.31.0.snupkg` (67 KB)
- **Target Frameworks**: .NET 8.0, 9.0, 10.0

## ?? Prerequisites

### 1. Get NuGet API Key

1. Go to [NuGet.org Account API Keys](https://www.nuget.org/account/apikeys)
2. Sign in with your Microsoft account
3. Create a new API key or use existing one
4. **Important**: Ensure the key has the following settings:
   - **Key Name**: WebSpark.Bootswatch (or your choice)
   - **Glob Pattern**: `WebSpark.Bootswatch`
   - **Scopes**: Push new packages and package versions
   - **Expiration**: Set appropriate expiration date

### 2. Set Environment Variable (Optional but Recommended)

```powershell
# Set for current session
$env:NUGET_API_KEY = "your-api-key-here"

# Or set permanently (Windows)
[Environment]::SetEnvironmentVariable("NUGET_API_KEY", "your-api-key-here", "User")
```

## ?? Publishing Methods

### Method 1: Using PowerShell Script (Recommended)

The repository includes a publishing script with safety checks and confirmation:

```powershell
# Preview what will be published (dry run)
.\publish-nuget-package.ps1 -WhatIf

# Publish using environment variable
.\publish-nuget-package.ps1

# Publish with API key parameter
.\publish-nuget-package.ps1 -ApiKey "your-api-key-here"

# Publish specific version
.\publish-nuget-package.ps1 -Version "1.31.0" -ApiKey "your-api-key"

# Skip duplicate check (useful for republishing)
.\publish-nuget-package.ps1 -SkipDuplicate
```

**Script Features**:
- ? API key validation
- ? Package file verification
- ? Size and version display
- ? Confirmation prompt
- ? Automatic symbols upload
- ? Colored output
- ? Post-publish instructions

### Method 2: Using .NET CLI Directly

```bash
# Publish main package
dotnet nuget push nupkgs/WebSpark.Bootswatch.1.31.0.nupkg \
    --api-key YOUR_API_KEY \
    --source https://api.nuget.org/v3/index.json

# Publish symbols package
dotnet nuget push nupkgs/WebSpark.Bootswatch.1.31.0.snupkg \
    --api-key YOUR_API_KEY \
    --source https://api.nuget.org/v3/index.json
```

### Method 3: Using NuGet CLI

```bash
nuget push nupkgs/WebSpark.Bootswatch.1.31.0.nupkg \
    -ApiKey YOUR_API_KEY \
    -Source https://api.nuget.org/v3/index.json
```

## ? Pre-Publication Checklist

Before publishing, verify:

- [x] **Version Updated**: Version is 1.31.0 in .csproj
- [x] **Package Built**: Successfully built for all target frameworks
- [x] **Tests Pass**: All 33 tests passing (11 tests × 3 frameworks)
- [x] **Documentation Updated**: README, CHANGELOG, and guides updated
- [x] **Release Notes**: Package release notes are accurate
- [x] **Package Created**: .nupkg and .snupkg files exist in nupkgs/
- [x] **Git Committed**: All changes committed to repository
- [x] **Git Tagged**: Consider creating a git tag for the version

## ?? Post-Publication Tasks

### Immediately After Publishing

1. **Wait for Indexing** (5-10 minutes)
   - NuGet.org needs time to process and index the package

2. **Verify Package**
   ```bash
   # Check package page
   https://www.nuget.org/packages/WebSpark.Bootswatch/1.31.0
   
   # Test installation
   mkdir test-install
   cd test-install
   dotnet new console
   dotnet add package WebSpark.Bootswatch --version 1.31.0
   ```

3. **Verify Multi-Framework Support**
   ```bash
   # Create test projects for each framework
   dotnet new console -n TestNet8 -f net8.0
   dotnet new console -n TestNet9 -f net9.0
   dotnet new console -n TestNet10 -f net10.0
   
   # Add package to each
   cd TestNet8 && dotnet add package WebSpark.Bootswatch --version 1.31.0
   cd ../TestNet9 && dotnet add package WebSpark.Bootswatch --version 1.31.0
   cd ../TestNet10 && dotnet add package WebSpark.Bootswatch --version 1.31.0
   ```

### Within 24 Hours

4. **Create GitHub Release**
   - Go to: https://github.com/MarkHazleton/WebSpark.Bootswatch/releases/new
   - Tag: `v1.31.0`
   - Title: `WebSpark.Bootswatch 1.31.0 - Multi-Framework Support`
   - Description: Copy from CHANGELOG.md
   - Attach files: .nupkg and .snupkg

5. **Update Demo Application**
   ```bash
   cd WebSpark.Bootswatch.Demo
   dotnet add package WebSpark.Bootswatch --version 1.31.0
   dotnet build
   dotnet run
   ```

6. **Merge to Main Branch**
   ```bash
   git checkout main
   git merge upgrade-to-NET10
   git push origin main
   git push origin --tags
   ```

7. **Update Documentation Sites**
   - Update demo site: https://bootswatch.markhazleton.com
   - Update GitHub wiki if applicable
   - Update any external documentation

8. **Announce Release**
   - GitHub Discussions announcement
   - Social media (if applicable)
   - Blog post (if applicable)

## ?? Verification Commands

### Check Package Information
```powershell
# View package contents
dotnet tool install -g NuGetPackageExplorer
NuGetPackageExplorer nupkgs/WebSpark.Bootswatch.1.31.0.nupkg

# Or use 7-Zip/WinRAR to inspect the .nupkg (it's a zip file)
```

### Verify Package Metadata
```powershell
# Download and inspect metadata
nuget list WebSpark.Bootswatch -PreRelease -AllVersions
```

### Check Framework Support
```bash
# After package is live, check supported frameworks
dotnet list package WebSpark.Bootswatch --framework net8.0
dotnet list package WebSpark.Bootswatch --framework net9.0
dotnet list package WebSpark.Bootswatch --framework net10.0
```

## ?? Troubleshooting

### Problem: "Package already exists"
**Solution**: 
- NuGet.org doesn't allow overwriting versions
- Either increment version or use `--skip-duplicate` flag
- Delete from NuGet.org (if within 24 hours and no downloads)

### Problem: "Package size too large"
**Solution**:
- Maximum package size is 250 MB
- Current package (12.4 MB) is well within limits
- If needed, exclude unnecessary files in .csproj

### Problem: "Invalid API key"
**Solution**:
- Verify API key hasn't expired
- Check API key permissions (needs Push scope)
- Regenerate API key on NuGet.org

### Problem: "Package validation failed"
**Solution**:
- Check package metadata in .csproj
- Ensure all required files are included
- Verify package signature if signing is enabled

### Problem: "Symbols upload failed"
**Solution**:
- Symbols are optional, main package still published
- Verify .snupkg format is correct
- Check if symbols server is available

## ?? Package Statistics

After publishing, monitor:
- **Downloads**: https://www.nuget.org/packages/WebSpark.Bootswatch/1.31.0
- **Dependencies**: Check reverse dependencies
- **Compatibility**: Framework compatibility reports
- **Issues**: GitHub issues related to the new version

## ?? Security Considerations

1. **API Key Security**
   - Never commit API keys to repository
   - Use environment variables
   - Rotate keys periodically
   - Use minimal required permissions

2. **Package Signing**
   - Package is signed with WebSpark.snk
   - Verify signature after publishing

3. **Dependency Security**
   - All dependencies are up to date
   - No known security vulnerabilities

## ?? Support

If you encounter issues during publishing:
- **NuGet Support**: https://www.nuget.org/policies/Contact
- **GitHub Issues**: https://github.com/MarkHazleton/WebSpark.Bootswatch/issues
- **Email**: mark@markhazleton.com

## ?? Quick Reference

```powershell
# Complete publishing workflow
cd C:\GitHub\MarkHazleton\WebSpark.Bootswatch

# 1. Clean and build
dotnet clean -c Release
dotnet build -c Release

# 2. Run tests
dotnet test

# 3. Pack
dotnet pack -c Release --no-build -o ./nupkgs

# 4. Publish (using script)
.\publish-nuget-package.ps1

# 5. Verify
Start-Process "https://www.nuget.org/packages/WebSpark.Bootswatch/1.31.0"
```

---

## ? Ready to Publish!

Your package is built and ready. Choose your preferred publishing method above and follow the steps.

**Recommended**: Use `.\publish-nuget-package.ps1` for the safest publishing experience with all checks and confirmations.
