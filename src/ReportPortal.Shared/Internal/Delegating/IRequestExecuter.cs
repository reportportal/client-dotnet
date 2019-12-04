using System;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Internal.Delegating
{
    public interface IRequestExecuter
    {
        Task<T> ExecuteAsync<T>(Func<Task<T>> func);
    }
}
