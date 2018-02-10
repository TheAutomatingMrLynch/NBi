using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Generation.Teradata
{
    class TargetFormatterTeradata : BaseTargetFormatter
    {
        public TargetFormatterTeradata()
        : base('"', '"', '.') { }
    }
}
