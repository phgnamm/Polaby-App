using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Enums
{
    public enum Level
    {
        Undergraduate, // Sinh viên đại học hoặc học vấn cấp bậc thấp hơn.
        Master,        // Thạc sĩ hoặc học vấn cấp cao hơn đại học.
        Doctorate,     // Tiến sĩ hoặc học vấn cấp cao nhất, bao gồm PGS, TS.
        Professor,     // Giáo sư hoặc chức danh cao nhất trong lĩnh vực học thuật.
        Specialist     // Chuyên gia hoặc các trình độ khác có thể không thuộc hệ thống học vấn chính thức.
    }
}
