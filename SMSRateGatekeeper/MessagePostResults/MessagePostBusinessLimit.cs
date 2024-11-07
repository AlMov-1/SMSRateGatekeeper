namespace SMSRateGatekeeper.MessagePostResults
{
    public class MessagePostBusinessLimit : MessagePostResult
    {
        public MessagePostBusinessLimit(string business,
                                        double accountLimitPercentage,
                                        double? businessLimitPercentage = null)
            : base(business,
                   accountLimitPercentage,
                   businessLimitPercentage){ }


        public override string Message =>
            $"Message for '{business}' business not posted due to business limit.";
    }
}
