using Microsoft.AspNetCore.Mvc;
using SMSRateGatekeeper.Models;

namespace SMSRateGatekeeper.Abstractions
{
    public interface IMessageSendingService
    {
        Task<IActionResult> SendMessage(SmsMessage model);
    }
}
