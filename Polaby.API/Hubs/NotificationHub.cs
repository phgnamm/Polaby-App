using Microsoft.AspNetCore.SignalR;

namespace Polaby.API.Hubs
{
    public class NotificationHub : Hub
    {
        //private readonly ILogger<NotificationHub> _logger;

        //public NotificationHub(ILogger<NotificationHub> logger)
        //{
        //    _logger = logger;
        //}

        public override async Task OnConnectedAsync()
        {
            //_logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
            //await Clients.All.SendAsync("ReceiveMessage", message);
            await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has connected");
        }

        //public async Task SendNotification(string message)
        //{
        //    await Clients.All.SendAsync("ReceiveNotification", message);
        //}
        //public async Task SendNotification(string message)
        //{
        //    await Clients.All.SendAsync(message);
        //}
    }
}
