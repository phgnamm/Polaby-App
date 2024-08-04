using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Enums
{
    public enum Diet
    {
        NoAnimalProducts,       // Không ăn thực phẩm có nguồn gốc động vật.
        ModerateAnimalProducts, // Ăn từ 30 - 90g thịt, cá... mỗi ngày.
        HighAnimalProducts      // Ăn > 90g thịt, cá... mỗi ngày.
    }
}
