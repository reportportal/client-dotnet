using System;

namespace ReportPortal.Client.Extentions
{
    [Serializable]
    public class ServiceException: Exception
    {
        private readonly int _errorCode;
        public ServiceException(string message, int code):base(message)
        {
            _errorCode = code;
        }

        public int ErrorCode
        {
            get { return _errorCode; }
        }
    }
}
