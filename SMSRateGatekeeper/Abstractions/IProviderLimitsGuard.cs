using SMSRateGatekeeper.MessagePostResults;

namespace SMSRateGatekeeper.Abstractions
{
    public interface IProviderLimitsGuard
    {
        Task<MessagePostResult> TryPostAsync(string business);
    }
}
