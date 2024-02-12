namespace ReportPortal.Shared.Internal.Delegating
{
    /// <summary>
    /// Creates instances of <see cref="IRequestExecutionThrottler"/>.
    /// </summary>
    public interface IRequestExecutionThrottleFactory
    {
        /// <summary>
        /// Create an instance of <see cref=" IRequestExecutionThrottler"/>
        /// </summary>
        /// <returns>Execution throttler.</returns>
        IRequestExecutionThrottler Create();
    }
}