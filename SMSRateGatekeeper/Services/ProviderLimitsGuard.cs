using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using SMSRateGatekeeper.Abstractions;
using SMSRateGatekeeper.MessagePostResults;
using SMSRateGatekeeper.Options;

namespace SMSRateGatekeeper.Services
{
    /// <summary>
    /// This thread safe class will keep track of sms submission limits for each bisiness and
    /// for the entire account, using reusable RateGuard class for both purposes.
    /// It is registered as a singleton.
    /// </summary>
    public class ProviderLimitsGuard : IProviderLimitsGuard, IAsyncDisposable
    {
        // to ensure the resources are only disposed once.
        private bool _disposed = false;

        // keeps tracking of each bisiness commits within provider threashold for the business
        private readonly ConcurrentDictionary<string, RateGuard> _businessesTracker = new();

        // keeps track of all submissions for entire account
        private readonly RateGuard _acoountGuard;

        private readonly LimitsOptions _limits;

        public ProviderLimitsGuard(IOptions<SMSProviderOptions> smsProviderOptions)
        {
            _limits = smsProviderOptions.Value.Limits;

            var accountLimit = _limits.MaxCountForEntireAccountPerSec;

            // time span for which we will avaluate the counts
            var timeWindow = TimeSpan.FromSeconds(1);

            // start guarding posts for entire account
            _acoountGuard = new RateGuard(accountLimit, timeWindow);
        }

        public async Task<MessagePostResult> TryPostAsync(string business)
        {
            // first check if limit for entire account is reached
            var accountResult = await _acoountGuard.TryPostAsync();
            if (!accountResult.CanPost)
            {
                return new MessagePostAccountLimit(business, 
                                                   accountResult.ThreasholdPercentageReached); 
            }

            // try to get the Rate Guard for the particular business on behalf of which the 
            // attemt to post is taking place.
            if (!_businessesTracker.TryGetValue(business, out var businessGuard))
            {
                // we didn't find the business with its guard,
                // so lets create it
                businessGuard = new RateGuard(_limits.MaxCountForBusinessPerSec,
                                              TimeSpan.FromSeconds(1));

                // let's add it to the dictionary that traks posts for registered businesses:
                _businessesTracker.TryAdd(business, businessGuard);
            }

            // we use retreived on new Rate Guard for the business
            // to assess the limit:
            var businessResult = await businessGuard.TryPostAsync();
            if (!businessResult.CanPost)
            {
                return new MessagePostBusinessLimit(business,
                                                    accountResult.ThreasholdPercentageReached,
                                                    businessResult.ThreasholdPercentageReached);
            }

            // we return the result indication that no limit has been reached:
            return new MessagePostSuccess(business,
                                          accountResult.ThreasholdPercentageReached,
                                          businessResult.ThreasholdPercentageReached);
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return;
            }

            await _acoountGuard.DisposeAsync();

            foreach (var rateGuard in _businessesTracker.Values)
            {
                await rateGuard.DisposeAsync();
            }

            _disposed = true;
        }
    }
}
