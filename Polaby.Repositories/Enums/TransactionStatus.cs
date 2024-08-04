namespace Polaby.Repositories.Enums
{
    public enum TransactionStatus
    {
        Pending, // Giao dịch đang chờ xử lý
        Completed, // Giao dịch đã hoàn tất
        Failed, // Giao dịch thất bại
        Cancelled // Giao dịch đã bị hủy
    }
}