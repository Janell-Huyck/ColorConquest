# Color Conquest

A short, logic-based puzzle game for quick breaks. Tap a square to toggle it and its neighbors; get every square the same color to win.

Built with .NET 8, C#, and .NET MAUI (MVVM).

## Run the app

### 1. Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (8.0 or later)
- **MAUI workload** for the platform you want to run (see step 2)
- **macOS** (for Mac Catalyst or iOS): Xcode from the App Store (for Mac Catalyst / iOS builds and signing)

### 2. Install the MAUI workload

From the **repository root** (`ColorConquest/`):

```bash
dotnet workload restore
```

If that does not install everything you need, install workloads explicitly:

| Platform        | Command |
|----------------|---------|
| Mac (Catalyst) | `dotnet workload install maui-maccatalyst` |
| iOS            | `dotnet workload install maui-ios` (macOS only) |
| Android        | `dotnet workload install maui-android` |
| Windows        | `dotnet workload install maui-windows` |

### 3. Start the app

**macOS — Mac Catalyst (simplest on a Mac)**  
This repo’s app project uses **only** `net8.0-maccatalyst` when you build on macOS, so you can run without choosing a framework:

```bash
cd ColorConquest
dotnet run
```

Or from the repository root:

```bash
dotnet run --project ColorConquest/ColorConquest.csproj
```

Same as **`make run`** from the repo root (see **Running tests locally** for **`make test`**).

The Mac app should launch as a desktop window.

**Android**  
Use an emulator or a device with USB debugging, then:

```bash
dotnet build ColorConquest/ColorConquest.csproj -f net8.0-android
dotnet run --project ColorConquest/ColorConquest.csproj -f net8.0-android
```

**iOS Simulator or device** (macOS only):

```bash
dotnet build ColorConquest/ColorConquest.csproj -f net8.0-ios
dotnet run --project ColorConquest/ColorConquest.csproj -f net8.0-ios
```

**Windows**  
Open the solution in Visual Studio and set the startup project to **Windows Machine**, or:

```bash
dotnet run --project ColorConquest/ColorConquest.csproj -f net8.0-windows10.0.19041.0
```

### 4. Run from an IDE (optional)

Open `ColorConquest.sln` in **Visual Studio** (Windows or Mac) or **JetBrains Rider**, choose the target (e.g. **Mac Catalyst**, **Android**, **iOS Simulator**), and press Run/Debug.

---

## Troubleshooting

### `dotnet workload` / MAUI workload errors

- Run **`dotnet workload restore`** from the repo root after cloning or upgrading the SDK.
- List installed workloads: **`dotnet workload list`**. You should see **maui** (or the split workloads you installed).
- If restore fails, install manually (see the table under **Install the MAUI workload**).
- On **macOS**, **Xcode** must be installed and its license accepted:  
  `sudo xcodebuild -license accept`  
  then open Xcode once so it finishes installing components.

### `dotnet run` fails or picks the wrong platform

- **macOS:** From the `ColorConquest/` app folder, `dotnet run` should target **Mac Catalyst** only. If you see framework errors, try:  
  `dotnet run --project ColorConquest/ColorConquest.csproj`
- **Windows / multi-target:** Pass **`-f`** explicitly, e.g.  
  `-f net8.0-windows10.0.19041.0` or `-f net8.0-android`.

### Android: emulator or device not found

- Create/start an AVD in **Android Studio** (Device Manager), or connect a device with **USB debugging** enabled.
- Ensure **ANDROID_HOME** (or **Android SDK** path) is set the same way Visual Studio / `dotnet` expects.

### iOS: signing / provisioning

- Open the project in **Visual Studio for Mac** or **Visual Studio** with the iOS workload; set **Signing** to your **Apple Developer** team for device builds.
- Simulator builds usually need fewer steps but still require Xcode.

### Windows: WinUI / developer mode

- Enable **Developer Mode** in Windows Settings if the build asks for it.
- Use a supported Windows 10/11 build (see the app’s target **Windows SDK** in `ColorConquest.csproj`).

### Mac Catalyst: app won’t open or is blocked

- **Gatekeeper:** Right-click the app → **Open**, or review **System Settings → Privacy & Security** for blocked apps.
- Clean and rebuild:  
  `dotnet clean` then `dotnet build` (or delete `bin` / `obj` under `ColorConquest/` if something is stuck).

### Still stuck

- Confirm **`dotnet --version`** is **8.0** or newer.
- Try **`dotnet build ColorConquest.sln -v n`** and read the first real error (not just warnings).

---

## Running tests locally

From the repository root, the **Makefile** provides:

| Command | What it does |
|---------|----------------|
| **`make test`** (or plain **`make`**) | Runs unit tests (`ColorConquest.Tests`). No full MAUI workload required for every platform. |
| **`make run`** | Runs the MAUI app (`dotnet run --project ColorConquest/ColorConquest.csproj`). Requires the workload for your platform (see **Run the app**). |

**`make help`** lists these targets.

Without Make — tests:

```bash
dotnet test ColorConquest.Tests/ColorConquest.Tests.csproj
```

Without Make — app (macOS example):

```bash
dotnet run --project ColorConquest/ColorConquest.csproj
```

---

## Building without running

From the repository root:

```bash
dotnet build ColorConquest.sln
```

**macOS** (single target in this project):

```bash
dotnet build ColorConquest/ColorConquest.csproj
```

**Explicit framework** (useful on Windows/Linux CI or when you have multiple targets):

```bash
dotnet build ColorConquest/ColorConquest.csproj -f net8.0-android
dotnet build ColorConquest/ColorConquest.csproj -f net8.0-maccatalyst
dotnet build ColorConquest/ColorConquest.csproj -f net8.0-ios
dotnet build ColorConquest/ColorConquest.csproj -f net8.0-windows10.0.19041.0
```
