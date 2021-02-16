using System;

namespace ReportPortal.Shared.Reporter.Statistics
{
    /// <inheritdoc/>
    public class StatisticsCounter : IStatisticsCounter
    {
        private readonly object _lockObj = new object();

        private TimeSpan _sum;

        /// <inheritdoc/>
        public TimeSpan Min { get; private set; }

        /// <inheritdoc/>
        public TimeSpan Max { get; private set; }

        /// <inheritdoc/>
        public TimeSpan Avg
        {
            get
            {
                if (Count == 0)
                {
                    return TimeSpan.Zero;
                }
                else
                {
                    return TimeSpan.FromTicks(_sum.Ticks / Count);
                }
            }
        }

        /// <inheritdoc/>
        public long Count { get; private set; }

        /// <inheritdoc/>
        public void Measure(TimeSpan duration)
        {
            lock (_lockObj)
            {
                if (Count == 0)
                {
                    Min = duration;
                    Max = duration;
                    _sum = duration;
                }
                else
                {
                    if (duration < Min)
                    {
                        Min = duration;
                    }
                    else if (duration > Max)
                    {
                        Max = duration;
                    }

                    _sum += duration;
                }

                Count++;
            }
        }

        /// <summary>
        /// Returns a string that represents the statistics counter.
        /// </summary>
        /// <returns>A string that represents the statistics counter.</returns>
        public override string ToString()
        {
            return $"{Count} cnt min/max/avg {Min.TotalMilliseconds:F0}/{Max.TotalMilliseconds:F0}/{Avg.TotalMilliseconds:F0} ms";
        }
    }
}
