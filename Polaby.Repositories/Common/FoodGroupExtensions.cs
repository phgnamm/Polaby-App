
using Polaby.Repositories.Enums;

public static class FoodGroupExtensions
{
    private static readonly Dictionary<FoodGroup, string> FoodGroupToString = new()
    {
        { FoodGroup.NgucCocVaSanPhamCheBien, "NGŨ CỐC VÀ SẢN PHẨM CHẾ BIẾN" },
        { FoodGroup.KhoaiCuVaSanPhamCheBien, "KHOAI CỦ VÀ SẢN PHẨM CHẾ BIẾN" },
        { FoodGroup.HatQuaGiauDamChatBeoVaSanPhamCheBien, "HẠT, QUẢ GIÀU ĐẠM, CHẤT BÉO VÀ SẢN PHẨM CHẾ BIẾN" },
        { FoodGroup.RauQuaCuDungLamRau, "RAU, QUẢ, CỦ DÙNG LÀM RAU" },
        { FoodGroup.QuaChin, "QUẢ CHÍN" },
        { FoodGroup.DauMoBo, "DẦU, MỠ, BƠ" },
        { FoodGroup.ThitVaSanPhamCheBien, "THỊT VÀ SẢN PHẨM CHẾ BIẾN" },
        { FoodGroup.ThuySanVaSanPhamCheBien, "THỦY SẢN VÀ SẢN PHẨM CHẾ BIẾN" },
        { FoodGroup.TrungVaSanPhamCheBien, "TRỨNG VÀ SẢN PHẨM CHẾ BIẾN" },
        { FoodGroup.SuaVaSanPhamCheBien, "SỮA VÀ SẢN PHẨM CHẾ BIẾN" },
        { FoodGroup.DoHop, "ĐỒ HỘP" },
        { FoodGroup.DoNgotDuongBanhMutKeo, "ĐỒ NGỌT (ĐƯỜNG, BÁNH, MỨT, KẸO)" },
        { FoodGroup.GiaViNuocCham, "GIA VỊ, NƯỚC CHẤM" },
        { FoodGroup.NuocGiaiKhatBiaRượu, "NƯỚC GIẢI KHÁT, BIA, RƯỢU" },
        { FoodGroup.ThucAnTruyenThong, "THỨC ĂN TRUYỀN THỐNG" },
        { FoodGroup.MotSoThucAnNhanh, "MỘT SỐ THỨC ĂN NHANH" },
        { FoodGroup.ThucPhamTheThao, "THỰC PHẨM THỂ THAO" },
        { FoodGroup.ThucPhamBenhDaiThaoDuong, "THỰC PHẨM BỆNH ĐÁI THÁO ĐƯỜNG" },
        { FoodGroup.ThucPhamBenhSuyThan, "THỰC PHẨM BỆNH SUY THẬN" },
        { FoodGroup.ThucPhamBenhUngThu, "THỰC PHẨM BỆNH UNG THƯ" },
        { FoodGroup.ThucPhamDinhDuongBoSung, "THỰC PHẨM DINH DƯỠNG BỔ SUNG" }
    };

    public static string ToFriendlyString(this FoodGroup foodGroup)
    {
        return FoodGroupToString[foodGroup];
    }

    public static FoodGroup FromFriendlyString(string value)
    {
        return FoodGroupToString.FirstOrDefault(x => x.Value.Equals(value, StringComparison.OrdinalIgnoreCase)).Key;
    }
}
