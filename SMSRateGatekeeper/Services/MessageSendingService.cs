using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SMSRateGatekeeper.Abstractions;
using SMSRateGatekeeper.MessagePostResults;
using SMSRateGatekeeper.Models;
using SMSRateGatekeeper.Notifications;

namespace SMSRateGatekeeper.Services
{
    /// <summary>
    /// This class, registered with the scoped life time will be instantiated for each request.
    /// It will be injected with the IProviderLimitsGuard instance, registered as a singleton 
    /// which is tracking the limits for businesses and entire account through all requests posted.
    /// ----
    /// This class will use the IProviderLimitsGuard to identify if message post is allowed to do.
    /// And if yes, it will delegate the message post action to the isntance of the ISMSProviderService,
    /// that encapsulates the logic and a usage of the providers SDK types to do the actual message posts.
    /// </summary>
    public class MessageSendingService : IMessageSendingService
    {
        private readonly ISMSProviderService _smsProvider;
        private readonly IProviderLimitsGuard _providerLimitsGuard;
        private readonly IHubContext<NotificationHub> _hubContext;

        public MessageSendingService(ISMSProviderService smsProvider,
                                     IProviderLimitsGuard providerLimitsGuard,
                                     IHubContext<NotificationHub> hubContext)
        {
            _smsProvider = smsProvider;
            _providerLimitsGuard = providerLimitsGuard;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> SendMessage(SmsMessage model)
        {
            try
            {
                var result = await _providerLimitsGuard.TryPostAsync(model.SenderNumber);

                if (result is MessagePostSuccess success)
                {
                    // send the message here
                    await _smsProvider.SendMessage(model);

                    var gkOkResult = new GateKeeperResult
                    {
                        Number = model.SenderNumber,
                        Title = "success",
                        Detail = success.Message,
                        AccountLimitPercentage = result.AccountLimitPercentage,
                        BusinessLimitPercentage = result.BusinessLimitPercentage
                    };

                    await SendNotificationToUi(gkOkResult);
                    return new OkObjectResult(gkOkResult) { StatusCode = StatusCodes.Status200OK };
                }

                var gkResult = new GateKeeperResult
                {
                    Number = model.SenderNumber,
                    Title = "One of the SMS Provider limits is reached.",
                    Detail = $" {result.Message} | Please try again later.",
                    AccountLimitPercentage = result.AccountLimitPercentage,
                    BusinessLimitPercentage = result.BusinessLimitPercentage
                };

                await SendNotificationToUi(gkResult);
                return new ObjectResult(gkResult) { StatusCode = StatusCodes.Status429TooManyRequests };
            }
            catch (Exception ex)
            {
                // log exception here (not implemented for simplicity)
                Console.Write(ex);

                var response = new GateKeeperResult
                {
                    Number = model.SenderNumber,
                    Title = "Error on the server",
                    Detail = "see logs for more details."
                };

                return new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }

        private async Task SendNotificationToUi(GateKeeperResult gkResult)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", gkResult);
        }
    }
}
