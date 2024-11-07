using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSRateGatekeeper.MessagePostResults;
using SMSRateGatekeeper.Models;
using SMSRateGatekeeper.Services;

namespace Tests
{
    public class RateGuardShould
    {
        [Fact]
        public async Task ReturnFalseWhenRateExidsLimit()
        {
            // Arrange
            int threshold = 2;
            TimeSpan timeWindow = TimeSpan.FromSeconds(1);

            var rateLimiter = new RateGuard(threshold, timeWindow);

            var tasks = new List<Task<RateGuardCallResult>>();
            for (int i = 0; i < 3; i++)
            {
                tasks.Add(Task.Run(() => rateLimiter.TryPostAsync()));
            }

            // Act
            var resultTask = await Task.WhenAll(tasks);

            // Assert
            int successCount = resultTask.Select(r => r.CanPost)
                                         .Where(r => r)
                                         .Count();

            int failCount = resultTask.Select(r => r.CanPost)
                                      .Where(r => !r)
                                      .Count();

            Assert.Equal(2, successCount); // Only the first two calls should succeed
            Assert.Equal(1, failCount); // The third call should fail
        }

        [Fact]
        public async Task ReturnReachedPercentageOfTheThreshold()
        {
            // Arrange
            int threshold = 2;
            TimeSpan timeWindow = TimeSpan.FromSeconds(1);

            var rateLimiter = new RateGuard(threshold, timeWindow);

            // Act
            var result = await rateLimiter.TryPostAsync();

            Assert.Equal(50, result.ThreasholdPercentageReached);
        }
    }
}
