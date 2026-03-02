# Color Conquest

A short, logic-based puzzle game for quick breaks. Tap a square to toggle it and its neighbors; get every square the same color to win.

Built with .NET 8, C#, and .NET MAUI (MVVM).

## Running tests locally

From the repository root, run:

```bash
make test
```

This runs the unit tests (Core + Tests projects only) and does not require MAUI workloads.

To run tests without Make:

```bash
dotnet test ColorConquest.Tests/ColorConquest.Tests.csproj
```

## Building the app locally

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Install MAUI workloads

Before building, install the workloads for the platform(s) you want to target:

```bash
dotnet workload restore
```

Or install a specific workload (e.g. Android, iOS, or Mac Catalyst):

```bash
dotnet workload install maui-android   # Android
dotnet workload install maui-ios       # iOS (macOS only)
dotnet workload install maui-maccatalyst # Mac Catalyst (macOS only)
dotnet workload install maui-windows   # Windows (Windows only)
```

### Build from the command line

From the repository root:

```bash
dotnet build ColorConquest.sln
```

To build for a specific platform, pass the target framework:

```bash
dotnet build ColorConquest/ColorConquest.csproj -f net8.0-android
dotnet build ColorConquest/ColorConquest.csproj -f net8.0-maccatalyst   # macOS
dotnet build ColorConquest/ColorConquest.csproj -f net8.0-ios          # macOS, for iOS
dotnet build ColorConquest/ColorConquest.csproj -f net8.0-windows10.0.19041.0   # Windows
```

### Run from IDE

Open `ColorConquest.sln` in Visual Studio or Rider, select the target platform (Android, iOS, Mac Catalyst, or Windows), and run the app from the IDE.
