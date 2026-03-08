using System;
using System.Collections.Generic;
using System.Text;
using LibreHardwareMonitor.Hardware;
using Real_time_System_Monitor_Integration.Models;

namespace Real_time_System_Monitor_Integration.Services
{
    public class HardwareMonitorService
    {
        private Computer _computer;
        private bool _disposed = false;

        public HardwareMonitorService()
        {
            _computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true
            };
            _computer.Open();
        }

        public HardwareMetrics GetMetrics()
        {
            var metrics = new HardwareMetrics();

            foreach (var hardware in _computer.Hardware)
            {
                hardware.Update();

                foreach (var sensor in hardware.Sensors)
                {
                    if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("CPU Total"))
                        metrics.CpuLoad = sensor.Value ?? 0;
                  
                    if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("Package"))
                        metrics.CpuTemp = sensor.Value ?? 0;
                    
                    if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("GPU"))
                        metrics.GpuLoad = sensor.Value ?? 0;
                }
            }

            metrics.Timestamp = DateTime.Now;
            return metrics;
        }
        public void Dispose()
        {
            if (!_disposed)
            {
                _computer?.Close();
                _disposed = true;
            }
        }
    }
}