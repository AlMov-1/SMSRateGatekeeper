using Microsoft.AspNetCore.Mvc;
using SMSRateGatekeeper.Abstractions;
using SMSRateGatekeeper.Models;

namespace SMSRateGatekeeper.Services
{
    /// <summary>
    /// This class encapsulates the code that will use 
    /// SMS provider package with its types to send the messages to actual provider.
    /// NOT IMPEMENTED
    /// </summary>
    public class SMSProviderService : ISMSProviderService
    {
        /// <summary>
        /// NOTE:
        /// for simplicity we return the bool here but in real life 
        /// it could be some type from Provider's package indicating the result.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> SendMessage(SmsMessage model)
        {
            //not implemented -> only simulates
            await Task.Delay(100);
            return true;           
        }
    }
}
