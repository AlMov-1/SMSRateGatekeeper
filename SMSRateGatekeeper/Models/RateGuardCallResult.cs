namespace SMSRateGatekeeper.Models
{
    public class RateGuardCallResult
    {
        // this will indicate how much of the threashold is reached by current attempt to post a message
        // in terms of percentage.
        public double ThreasholdPercentageReached { get; }

        public bool CanPost { get; }

        public RateGuardCallResult(double threasholdPercentageReached, 
                                   bool canPost = false)
        {
            ThreasholdPercentageReached = threasholdPercentageReached;
            CanPost = canPost;
        }
    }
}
