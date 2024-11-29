using SMSRateGatekeeper.Extensions;
using SMSRateGatekeeper.Models;
using System.Collections.Concurrent;

namespace SMSRateGatekeeper.Services
{
    /// <summary>
    /// This thread-safe class implements a form of a sliding window algorithm,
    /// which is used to monitor and limit the number of events that occur within a specified time frame.
    /// It will track the counter for a number of submissions within the provided time span.
    /// It will decrement a counter for each outdated TimeStamp, thus ensure to allow submition
    /// within the interval.
    /// It will compare the alredy submitted counts to the threashold and return fals if the 
    /// current submission exisds the limit.
    /// Otherwise it will increment the count and return true to allow current submission
    /// </summary>
    public sealed class RateGuard : IAsyncDisposable 
    {
        // submissions counter
        private int _count;

        // submision timestamp tracker
        private readonly ConcurrentQueue<DateTime> _timestamps;

        // threshold to comapare counts to
        private readonly int _threshold;

        // Time interval during which to prevent count to exceed the threashold
        private readonly TimeSpan _timeWindow;

        // is used to reset counter to zero after time interval elapses
        private readonly Timer _timer;

        // to be used to manage concurrent access
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        // to ensure the resources are only disposed once.
        private bool _disposed = false;

        public RateGuard(int threshold, TimeSpan timeWindow)
        {
            _threshold = threshold;
            _timeWindow = timeWindow;
            _timestamps = new ConcurrentQueue<DateTime>();
            _timer = new Timer(ResetCounter, null, timeWindow, timeWindow);
        }

        public async Task<RateGuardCallResult> TryPostAsync()
        {
            var now = DateTime.UtcNow;
            await _semaphore.WaitAsync();

            try
            {
                // Remove outdated timestamps
                while (_timestamps.TryPeek(out DateTime timestamp) && now - timestamp > _timeWindow)
                {
                    _timestamps.TryDequeue(out _);
                    Interlocked.Decrement(ref _count);
                }

                if (_count >= _threshold)
                {
                    return new(_count.AsPercentageOf(_threshold));  //false; =>  Exceeded the allowed count
                }

                // Add current timestamp and increment the counter
                _timestamps.Enqueue(now);
                Interlocked.Increment(ref _count);
                return new(_count.AsPercentageOf(_threshold), true); //true;
            }
            finally 
            { 
                _semaphore.Release(); 
            }
        }

        private void ResetCounter(object state)
        {
            Interlocked.Exchange(ref _count, 0); // resetting to 0

            // Clear the queue
            while (_timestamps.TryDequeue(out _)) ;
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return;
            }

            await _timer.DisposeAsync();

            await _semaphore.WaitAsync();
            try
            {
                _semaphore.Dispose();
            }
            finally
            {
                _semaphore.Release();
            }
            _disposed = true;
        }
    }
}
