using System.ComponentModel.DataAnnotations;

namespace SMSRateGatekeeper.Models
{
    public class SmsMessage
    {
        [Phone]
        public string SenderNumber { get; set; }

        [Phone]
        public string DestinationNumber { get; set; }

        [StringLength(100, ErrorMessage = "The message cannot be longer than 100 characters.")]
        public string MessageText { get; set; }
    }
}
