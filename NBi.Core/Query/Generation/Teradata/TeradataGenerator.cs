using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Generation.Teradata
{
    class TeradataGenerator : SqlGenerator
    {
        public TeradataGenerator()
        : base(new TargetFormatterTeradata(), new FieldFormatterTeradata())
        { }
    }
}
