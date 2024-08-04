using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Enums
{
    public enum HealthUnit
    {
        kg,    // Cân nặng (Weight) - Kilôgam
        cm,    // Chiều cao (Height) và Size - Centimet
        mmHg,  // Huyết áp (BloodPressure) - Milimet thủy ngân
        BPM,   // Nhịp tim (Heartbeat) - Nhịp mỗi phút (Beats Per Minute)
        CPH    // Co bóp (Contractility) - Số lần co bóp mỗi giờ (Contractions Per Hour)
    }
}
