using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Enums
{
    public enum ReportStatus
    {
        Pending,        // Báo cáo đang chờ xử lý
        UnderReview,    // Báo cáo đang được xem xét
        Resolved,       // Báo cáo đã được giải quyết
        Dismissed       // Báo cáo đã bị bỏ qua
    }
}
