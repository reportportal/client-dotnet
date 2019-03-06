﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReportPortal.Client.Api.Filter.Model
{
    public enum FilterType
    {
        [DataMember(Name = "launch")]
        Launch,
        [DataMember(Name = "testitem")]
        TestItem,
        [DataMember(Name = "log")]
        Log
    }
}
