using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Generation.MsSql
{
    class MsSqlGenerator : SqlGenerator
    {
        public MsSqlGenerator()
        : base(new TargetFormatterMsSql(), new FieldFormatterMsSql())
        { }
    }
}
