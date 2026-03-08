# Real-time System Monitor Integration

A WPF desktop application that retrieves and displays live CPU and GPU hardware metrics using the [LibreHardwareMonitor](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor) library.

---

## Requirements

| Item | Version |
|------|---------|
| .NET | 10.0 |
| OS | Windows 10 / 11 |
| IDE | Visual Studio 2022+ (recommended) |
| Privileges | **Administrator** (required for sensor access) |

---

## How to Run

### 1. Clone the repository

```bash
git clone https://github.com/Jas078/Real-time-System-Monitor-Integration.git
cd Real-time-System-Monitor-Integration
```

### 2. Restore NuGet packages

```bash
dotnet restore
```

### 3. Build the project

```bash
dotnet build
```

### 4. Run as Administrator

>  LibreHardwareMonitor requires Administrator privileges to access hardware sensor data.

**Visual Studio:**
Right-click Visual Studio icon → **Run as administrator** → Open the solution

---

## Features

-  Displays real-time CPU Load (%), CPU Temperature (°C), and GPU Load (%)
-  Configurable refresh interval (1–60 seconds), adjustable from the UI
-  Start / Stop monitoring toggle
-  Detects missing Administrator privileges and shows a warning at startup
-  Properly disposes hardware resources on application close

---

## Project Structure

```
Real-time-System-Monitor-Integration/
├── Models/
│   └── HardwareMetrics.cs         # Data model for sensor values
├── Services/
│   └── HardwareMonitorService.cs  # LibreHardwareMonitor wrapper (IDisposable)
├── MainWindow.xaml                # WPF UI layout
├── MainWindow.xaml.cs             # UI logic and timer control
└── README.md
```

---

## Design Decisions

### WPF
Chosen for its native Windows integration and straightforward data-binding support, which suits a real-time monitoring dashboard well.

### DispatcherTimer
Used instead of `System.Timers.Timer` because `DispatcherTimer` runs on the UI thread, allowing direct UI updates without requiring `Dispatcher.Invoke()`.

### Configurable Interval
The refresh interval defaults to 1 second but can be changed in the UI before starting the monitor. Input is validated to accept only integers between 1 and 60 seconds.

### IDisposable on HardwareMonitorService
`_computer.Close()` is called via `Dispose()` to ensure LibreHardwareMonitor properly releases WMI and kernel driver handles when the application exits.

---

## Dependencies

- [LibreHardwareMonitor](https://www.nuget.org/packages/LibreHardwareMonitorLib) — installed via NuGet
