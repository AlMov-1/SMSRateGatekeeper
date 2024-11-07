namespace SMSRateGatekeeper.Options
{
    public class SMSProviderOptions
    {
        public LimitsOptions Limits { get; set; }
        public DetailsOptions Details { get; set; }
    }

    public class LimitsOptions
    {
        public int MaxCountForBusinessPerSec { get; set; }
        public int MaxCountForEntireAccountPerSec { get; set; }
    }

    public class DetailsOptions
    {
        public string APIKey { get; set; }
        public string APISecret { get; set; }
        public string Url { get; set; }
    }

}
