using Microsoft.AspNetCore.Mvc;
using SMSRateGatekeeper.Abstractions;
using SMSRateGatekeeper.Models;

namespace SMSRateGatekeeper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostSMS : ControllerBase
    {
        private readonly IMessageSendingService _msgService;

        public PostSMS(IMessageSendingService msgService)
        {
            _msgService = msgService;
        }


        [HttpPost]
        [ProducesResponseType<GateKeeperResult>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendSMS([FromBody] SmsMessage message)
        {
            return await _msgService.SendMessage(message);
        }

    }
}
