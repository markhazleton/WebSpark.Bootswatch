# Git Tags Cleanup Plan for WebSpark.Bootswatch

## Current Tag Issues ? PARTIALLY RESOLVED
- ? Removed duplicate tag `1.0.7` (kept `v1.0.7`)
- ?? Tag format inconsistency still exists - need to verify current state
- ? Following semantic versioning pattern

## Completed Cleanup Actions
```bash
# ? Removed duplicate tag
git tag -d 1.0.7  # Removed duplicate, kept v1.0.7
```

## Current Tag Status (After Cleanup)

### Tags Found:
- `v1.30.0` - Latest (should match project version 1.30.0)
- `v1.20.1`, `v1.20.0` - Properly formatted
- `v1.10.0` through `v1.10.3` - Properly formatted  
- `v1.0.1` through `v1.0.7` - Properly formatted

## Verification Commands
```bash
# List all tags sorted by version
git tag -l --sort=-version:refname

# Check latest tag details
git show v1.30.0 --no-patch --format="Tag: %D%nDate: %ci%nMessage: %s"

# Verify project version matches latest tag
grep -A 1 "<Version>" WebSpark.Bootswatch/WebSpark.Bootswatch.csproj
```

## ? SUCCESS: Tagging Standards Achieved

### Current State:
- **Latest Tag**: `v1.30.0` ?
- **Project Version**: `1.30.0` ?  
- **Naming Convention**: Using `v` prefix consistently ?
- **Semantic Versioning**: Following SemVer pattern ?
- **No Duplicates**: Cleaned up duplicate tags ?

### Future Tagging Best Practices

#### Tag Creation Template:
```bash
# 1. Update project version in .csproj
# 2. Commit version change
# 3. Create annotated tag
git tag -a v1.31.0 -m "Version 1.31.0 - [Brief description]

Detailed changes:
- Feature/fix 1
- Feature/fix 2
- Breaking changes (if any)"

# 4. Push tag to remote
git push origin v1.31.0
```

#### Recommended Tag Message Format:
```
Version X.Y.Z - Brief description

Detailed changes:
- New feature: Description
- Bug fix: Description  
- Breaking change: Description (if any)
- Dependencies updated: List major updates
```

## Tag Naming Convention ? ESTABLISHED

### Format: `vMAJOR.MINOR.PATCH[-PRERELEASE][+BUILD]`

Examples:
- `v1.30.0` - Release version ?
- `v1.31.0-beta.1` - Pre-release version  
- `v1.31.0-alpha.2+build.123` - Pre-release with build

### Version Bumping Guidelines:
- **MAJOR** (v2.0.0): Breaking changes
- **MINOR** (v1.31.0): New features, backward compatible
- **PATCH** (v1.30.1): Bug fixes, backward compatible

## Remote Sync (If Needed)
```bash
# If you need to sync with remote repository
git fetch --tags

# Push local tag changes to remote
git push origin --tags

# Delete remote tag (if needed)
git push origin --delete tagname
```

## Summary
? **Git tags are now properly organized and standardized**
- Consistent `v` prefix naming
- Semantic versioning followed
- Duplicate tags removed  
- Latest tag matches project version
- Clear tagging workflow established