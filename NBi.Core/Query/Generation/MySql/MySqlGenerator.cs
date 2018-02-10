using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Generation.MySql
{
    class MySqlGenerator : SqlGenerator
    {
        public MySqlGenerator()
        : base(new TargetFormatterMySql(), new FieldFormatterMySql())
        { }
    }
}
