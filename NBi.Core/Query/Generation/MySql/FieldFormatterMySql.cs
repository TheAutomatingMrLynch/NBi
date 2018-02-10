using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Generation.MySql
{
    class FieldFormatterMySql : BaseFieldFormatter
    {
        public FieldFormatterMySql()
        : base('`', '`') { }
    }
}
