using Microsoft.AspNetCore.Mvc;
using SMSRateGatekeeper.Models;

namespace SMSRateGatekeeper.Abstractions
{
    public interface ISMSProviderService
    {
        Task<bool> SendMessage(SmsMessage model);
    }
}
