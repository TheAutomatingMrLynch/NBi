﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.SqlServer.ReportingService
{
    public interface IValueFilter
    {
        string Value { get; }
    }
}
