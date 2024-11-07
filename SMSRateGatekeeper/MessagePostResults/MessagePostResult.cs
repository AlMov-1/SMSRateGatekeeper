namespace SMSRateGatekeeper.MessagePostResults
{
    // Represents the result of the attempt to POST the nmessage
    // to the SMS Provider Guard Service
    public abstract class MessagePostResult
    {
        protected MessagePostResult(string business, 
                                    double accountLimitPercentage,
                                    double? businessLimitPercentage = null)
        {
            this.business = business;
            AccountLimitPercentage = accountLimitPercentage;
            BusinessLimitPercentage = businessLimitPercentage;
        }

        protected string business { get; }
        public double AccountLimitPercentage { get; }

        // For simplicity we will not indicate the BusinessLimitPercentage
        // in the case where account limit percentage is reached as we return right away in this case
        // and not try to retrieve the business count.
        // So we will allow this to be nullable
        public double? BusinessLimitPercentage { get; }

        // This will hold the message indicating the result of the atempt to POST the SMS
        public abstract string Message { get; }
    }
}
