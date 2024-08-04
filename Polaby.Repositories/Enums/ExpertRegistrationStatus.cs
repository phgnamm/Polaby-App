namespace Polaby.Repositories.Enums
{
    public enum ExpertRegistrationStatus
    {
        Pending, // Đăng ký đã được tạo nhưng chưa được xem xét.
        UnderReview, // Đăng ký đang được xem xét bởi Admin.
        Approved, // Đăng ký đã được chấp thuận và xác nhận.
        Rejected, // Đăng ký đã bị từ chối.
        Incomplete, // Đăng ký không đầy đủ, cần bổ sung thông tin.
        Withdrawn // Chuyên gia đã tự rút lại đơn đăng ký.
    }
}