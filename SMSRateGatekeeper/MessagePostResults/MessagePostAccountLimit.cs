namespace SMSRateGatekeeper.MessagePostResults
{
    public class MessagePostAccountLimit : MessagePostResult
    {
        public MessagePostAccountLimit(string business,
                                       double accountLimitPercentage,
                                       double? businessLimitPercentage = null)
            : base(business,
                   accountLimitPercentage,
                   businessLimitPercentage){ }

        public override string Message =>
            $"Message for '{business}' business not posted due to account limit.";
    }
}
