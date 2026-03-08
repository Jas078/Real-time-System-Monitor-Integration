using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Real_time_System_Monitor_Integration.Services;
using System.Security.Principal;

namespace Real_time_System_Monitor_Integration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int DefaultIntervalSeconds = 1;

        private HardwareMonitorService _service;
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            if (!IsAdministrator())
            {
                MessageBox.Show(
                    "Warning: Application is not running with Administrator privileges.\n" +
                    "Some sensor data may not be available.",
                    "Administrator Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            _service = new HardwareMonitorService();

            _timer = new DispatcherTimer();

            _timer.Tick += Timer_Tick;
            _timer.Interval = TimeSpan.FromSeconds(DefaultIntervalSeconds);
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(IntervalTextBox.Text, out int seconds) && seconds >= 1 && seconds <= 60)
            {
                _timer.Interval = TimeSpan.FromSeconds(seconds);
            }
            else
            {
                MessageBox.Show(
                    "Please enter a valid interval between 1 and 60 seconds.",
                    "Invalid Input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            _timer.Start();

            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            IntervalTextBox.IsEnabled = false;
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            var metrics = _service.GetMetrics();

            CpuLoadText.Text = $"CPU Load: {metrics.CpuLoad:F1} %";
            CpuTempText.Text = $"CPU Temp: {metrics.CpuTemp:F1} °C";
            GpuLoadText.Text = $"GPU Load: {metrics.GpuLoad:F1} %";
            LastUpdateText.Text = $"Last Update: {metrics.Timestamp:HH:mm:ss}";
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();

            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            IntervalTextBox.IsEnabled = true;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _timer?.Stop();
            _service?.Dispose();
        }
        private bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}