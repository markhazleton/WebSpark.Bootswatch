# WebSpark.Bootswatch

A .NET Razor Class Library that provides seamless integration of [Bootswatch](https://bootswatch.com/) themes into ASP.NET Core applications. Built on Bootstrap 5, this library offers modern, responsive theming with dynamic theme switching, light/dark mode support, and comprehensive caching mechanisms.

**Framework Support**: Requires .NET 10.0 for access to the latest NuGet packages and framework features.

[![NuGet Version](https://img.shields.io/nuget/v/WebSpark.Bootswatch.svg)](https://www.nuget.org/packages/WebSpark.Bootswatch/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/WebSpark.Bootswatch.svg)](https://www.nuget.org/packages/WebSpark.Bootswatch/)
[![GitHub License](https://img.shields.io/github/license/MarkHazleton/WebSpark.Bootswatch)](https://github.com/MarkHazleton/WebSpark.Bootswatch/blob/main/LICENSE)
[![.NET](https://github.com/MarkHazleton/WebSpark.Bootswatch/actions/workflows/dotnet.yml/badge.svg)](https://github.com/MarkHazleton/WebSpark.Bootswatch/actions/workflows/dotnet.yml)
[![GitHub Stars](https://img.shields.io/github/stars/MarkHazleton/WebSpark.Bootswatch)](https://github.com/MarkHazleton/WebSpark.Bootswatch/stargazers)

**Live Site**: [https://bootswatch.makeboldspark.com/](https://bootswatch.makeboldspark.com/)

## About

WebSpark.Bootswatch is a .NET Razor Class Library that provides seamless integration of [Bootswatch](https://bootswatch.com/) themes into ASP.NET Core applications. See it live at [https://bootswatch.makeboldspark.com/](https://bootswatch.makeboldspark.com/).

> Built by [Mark Hazleton](https://markhazleton.com) — Mark Hazleton, Solutions Architect
> BootswatchSpark is part of the [Make Bold Spark](https://makeboldspark.com) portfolio of technical demonstrations.

**Latest Release**: v2.5.1 - Dependency Alignment Release

## 🚨 Version 2.0 Breaking Changes

**WebSpark.Bootswatch 2.0+ now targets .NET 10 exclusively.** This major version change reflects our commitment to supporting the latest NuGet packages and framework features over broad framework compatibility.

### Why .NET 10 Only?

We prioritized **latest packages over legacy support** for these reasons:

- ✅ **Security & Performance**: Access to latest security patches and .NET 10 performance improvements
- ✅ **Modern Dependencies**: Use current versions of Microsoft.Extensions.* packages (10.0.1+)
- ✅ **Simplified Maintenance**: Single target framework reduces complexity and testing burden
- ✅ **Future-Ready**: .NET 10 is the current release with latest features
- ⏰ **.NET 8 Approaching EOL**: Standard support ends November 2025
- ⏰ **.NET 9 Short Lifecycle**: STS (Standard Term Support) has shorter timeline

### Migration from 1.x to 2.0

```xml
<!-- Update your .csproj -->
<PropertyGroup>
  <TargetFramework>net10.0</TargetFramework>
</PropertyGroup>

<!-- Update package versions -->
<PackageReference Include="WebSpark.Bootswatch" Version="2.5.1" />
<PackageReference Include="WebSpark.HttpClientUtility" Version="2.5.1" />
```

**No code changes required** - All APIs remain backward compatible.

### Need .NET 8 or 9 Support?

Use **WebSpark.Bootswatch 1.34.0** which supports .NET 8.0, 9.0, and 10.0.

```bash
dotnet add package WebSpark.Bootswatch --version 1.34.0
dotnet add package WebSpark.HttpClientUtility --version 2.1.1
```

## 📋 Prerequisites

### Framework Requirements

| Framework | Version 2.x | Version 1.x |
|-----------|-------------|-------------|
| .NET 10.0 | ✅ Required | ✅ Supported |
| .NET 9.0  | ❌ Not Supported | ✅ Supported |
| .NET 8.0  | ❌ Not Supported | ✅ Supported |

**Version 2.0+** requires .NET 10.0 SDK installed on your development machine and build server.

**Version 1.34.0** supports .NET 8.0 (LTS), 9.0 (STS), and 10.0 if you need backward compatibility.

### Required Dependencies

```xml
<!-- Version 2.x (NET 10 only) -->
<PackageReference Include="WebSpark.Bootswatch" Version="2.5.1" />
<PackageReference Include="WebSpark.HttpClientUtility" Version="2.5.1" />

<!-- Version 1.x (NET 8/9/10) -->
<PackageReference Include="WebSpark.Bootswatch" Version="1.34.0" />
<PackageReference Include="WebSpark.HttpClientUtility" Version="2.1.1" />
```

### PackageReference

```xml
<!-- For .NET 10 projects -->
<PackageReference Include="WebSpark.Bootswatch" Version="2.5.1" />
<PackageReference Include="WebSpark.HttpClientUtility" Version="2.5.1" />
```

**Note**: Version 2.0+ targets .NET 10.0 exclusively. The package will automatically use the correct assembly for your .NET 10 project.

## ✨ Features

- **🎨 Complete Bootswatch Integration**: All official Bootswatch themes plus custom themes
- **🌓 Light/Dark Mode Support**: Automatic theme detection and switching
- **⚡ High Performance**: Built-in caching with `StyleCache` service
- **🔧 Easy Integration**: Single-line setup with extension methods
- **📱 Responsive Design**: Mobile-first Bootstrap 5 foundation
- **🎯 Tag Helper Support**: `<bootswatch-theme-switcher />` for easy UI integration
- **🔒 Production Ready**: Comprehensive error handling and fallback mechanisms
- **📖 Full Documentation**: IntelliSense support and XML documentation
- **🚀 .NET 10 Optimized**: Latest framework features and performance enhancements

**Verify Installation:**
Your `.csproj` should now include BOTH packages:

```xml
<PackageReference Include="WebSpark.Bootswatch" Version="2.5.1" />
<PackageReference Include="WebSpark.HttpClientUtility" Version="2.5.1" />
