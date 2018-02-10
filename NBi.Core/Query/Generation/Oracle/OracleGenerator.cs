using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Generation.Oracle
{
    class OracleGenerator : SqlGenerator
    {
        public OracleGenerator()
        : base(new TargetFormatterOracle(), new FieldFormatterOracle())
        { }
    }
}
