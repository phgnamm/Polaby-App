using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polaby.Services.Notification;
using System.Text;

namespace Polaby.API.Helper
{
    
    public class OneSignalPushNotificationService : IOneSignalPushNotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _oneSignalAppId;
        private readonly string _oneSignalRestApiKey;

        public OneSignalPushNotificationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _oneSignalAppId = configuration["OneSignal:AppId"];
            _oneSignalRestApiKey = configuration["OneSignal:RestApiKey"];
        }

        public async Task<bool> SendNotificationAsync(string heading, string content, string playerId)
        {
            var requestUri = "https://onesignal.com/api/v1/notifications";

            // Create the payload
            var payload = new
            {
                app_id = _oneSignalAppId,
                headings = new { en = heading },
                contents = new { en = content },
                include_player_ids = new[] { playerId } // Target specific users by their OneSignal player ID
            };

            var jsonPayload = JsonConvert.SerializeObject(payload);
            var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Add Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _oneSignalRestApiKey);

            // Send the request
            var response = await _httpClient.PostAsync(requestUri, requestContent);

            // Check if the request was successful
            return response.IsSuccessStatusCode;
        }

        //public static async Task<string> OneSignalPushNotification(CreateNotificationModel request, Guid appId, string restKey)
        //{
        //    OneSignalClient client = new OneSignalClient(restKey);
        //    var opt = new NotificationCreateOptions()
        //    {
        //        AppId = appId,
        //        IncludePlayerIds = request.PlayerIds
        //        //SendAfter = DateTime.Now.AddSeconds(10)
        //    };
        //    opt.Headings.Add(LanguageCodes.Vietnamese, request.Title);
        //    opt.Contents.Add(LanguageCodes.Vietnamese, request.Content);
        //    NotificationCreateResult result = await client.Notifications.CreateAsync(opt);
        //    return result.Id;
        //}
        //public static async Task<string> OneSignalCancelPushNotification(string id, string appId, string restKey)
        //{
        //    var client = new OneSignalClient(restKey);
        //    var opt = new NotificationCancelOptions()
        //    {
        //        AppId = appId,
        //        Id = id
        //    };
        //    NotificationCancelResult result = await client.Notifications.CancelAsync(opt);
        //    return result.Success;
        //}
        //public static async Task<NotificationDislayedRequest> WebhooksDisplayed(NotificationDislayedRequest request)
        //{
        //    await Task.Yield();
        //    //TODO: code here
        //    return request;
        //}
    }
}
