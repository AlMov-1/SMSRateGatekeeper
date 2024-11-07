namespace SMSRateGatekeeper.MessagePostResults
{
    public class MessagePostSuccess : MessagePostResult
    {
        public MessagePostSuccess(string business,                                    
                                  double accountLimitPercentage,
                                  double? businessLimitPercentage = null)
            : base(business,
                   accountLimitPercentage,
                   businessLimitPercentage) { }
       

        public override string Message => $"Message for '{business}' business posted successfully.";
    }
}
