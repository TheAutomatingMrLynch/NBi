using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Generation
{
    interface ITargetFormatter
    {
        string Execute(string schemaName, string tableName);
    }
}
