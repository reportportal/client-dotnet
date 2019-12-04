using ReportPortal.Shared.Configuration;

namespace ReportPortal.Shared.Internal.Delegating
{
    public interface IRequestExecuterFactory
    {
        IRequestExecuter Create(IConfiguration configuration);
    }
}
