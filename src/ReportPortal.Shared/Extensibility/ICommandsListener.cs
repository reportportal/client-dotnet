using ReportPortal.Shared.Extensibility.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Shared.Extensibility
{
    public interface ICommandsListener
    {
        void Initialize(ICommandsSource commandsSource);
    }
}
