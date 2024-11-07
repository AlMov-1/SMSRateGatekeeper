namespace SMSRateGatekeeper.Extensions
{
    public static class NumericExtensions
    {
        // calculates the count as a percentage of the threshold
        public static double AsPercentageOf(this int count, int threshold)
            => ((double)count / threshold) * 100;


        // Rounds the value of double to desired amount of digits after comma
        public static double RoundUpTo(this double value, int digits)
            => Math.Round(value, digits);
    }
}
