using System;
using System.Collections.Generic;
using System.Text;

namespace Real_time_System_Monitor_Integration.Models
{
    public class HardwareMetrics
    {
        public float CpuLoad { get; set; }
        public float CpuTemp { get; set; }
        public float GpuLoad { get; set; }

        public DateTime Timestamp { get; set; }
    }
}