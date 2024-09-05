namespace Polaby.Repositories.Models.DashboardModels;

public class AdminDashboardModel
{
    public int TotalAccount { get; set; }
    public int TotalSubscription { get; set; }
    public int TotalPost { get; set; }
    public float TotalEarning { get; set; }
    public List<Earning> Earnings { get; set; } 
    public List<Forum> Forums { get; set; }
}

public class Earning
{
    public DateOnly Date { get; set; }
    public float Amount { get; set; }
}

public class Forum
{
    public DateOnly Date { get; set; }
    public int TotalPost { get; set; }
    public int TotalComment { get; set; }
}