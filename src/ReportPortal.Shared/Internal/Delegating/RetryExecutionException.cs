using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <summary>
    /// Occurs when retry request execution is unsuccessful.
    /// </summary>
    public class RetryExecutionException : AggregateException
    {
        private readonly string _message;

        /// <summary>
        /// Initializes a new instance of <see cref="RetryExecutionException"/>
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="innerExceptions">Inner exceptions.</param>
        public RetryExecutionException(string methodName, IEnumerable<Exception> innerExceptions)
            : base(message: null, innerExceptions)
        {
            _message = $"'Invocation of '{methodName}' has not been finished.";
        }

        /// <inheritdoc/>
        public override string Message => _message;

        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine($"{GetType().Name}: {Message}");
            text.Append(StackTrace);

            for (int index = 0; index < InnerExceptions.Count; index++)
            {
                text.Append(Environment.NewLine + " ---> ");
                text.AppendFormat(CultureInfo.InvariantCulture, "(Inner Exception #{0}) ", index);
                text.Append(InnerExceptions[index].ToString());

                text.Append(" <--- ");
                text.AppendLine();
            }

            return text.ToString();
        }
    }
}
