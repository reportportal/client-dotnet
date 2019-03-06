﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReportPortal.Client.Api.Log.Model
{
    /// <summary>
    /// Describes levels for log items.
    /// </summary>
    public enum LogLevel
    {
        [DataMember(Name = "TRACE")]
        Trace,
        [DataMember(Name = "DEBUG")]
        Debug,
        [DataMember(Name = "INFO")]
        Info,
        [DataMember(Name = "WARN")]
        Warning,
        [DataMember(Name = "ERROR")]
        Error,
        [DataMember(Name = "FATAL")]
        Fatal
    }
}
