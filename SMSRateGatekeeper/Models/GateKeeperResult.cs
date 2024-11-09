using Newtonsoft.Json;

namespace SMSRateGatekeeper.Models
{
    /// <summary>
    /// This class represents the result that will be returend to the Business 
    /// that initiated the HTTP POST to the 
    /// SMSRateGateKeeper service.
    /// </summary>
    public class GateKeeperResult
    {
        //[JsonProperty("status")]
        //public int Status { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }

        [JsonProperty("accountLimitPercentage")]
        public double AccountLimitPercentage { get; set; }

        [JsonProperty("businessLimitPercentage")]
        public double? BusinessLimitPercentage { get; set; }
    }
}
