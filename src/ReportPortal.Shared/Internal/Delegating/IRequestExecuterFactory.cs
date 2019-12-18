using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <summary>
    /// Creates instances of <see cref="IRequestExecuter"/> based on <see cref="IConfiguration"/>
    /// </summary>
    public interface IRequestExecuterFactory
    {
        /// <summary>
        /// Creates request executer.
        /// </summary>
        /// <returns>An instance of <see cref="IRequestExecuter"/></returns>
        IRequestExecuter Create();
    }
}
