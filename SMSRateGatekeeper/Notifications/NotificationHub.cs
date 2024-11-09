using Microsoft.AspNetCore.SignalR;
using SMSRateGatekeeper.Models;

namespace SMSRateGatekeeper.Notifications
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(GateKeeperResult result)
        {
            await Clients.All.SendAsync("ReceiveNotification", result);
        }
    }

}
