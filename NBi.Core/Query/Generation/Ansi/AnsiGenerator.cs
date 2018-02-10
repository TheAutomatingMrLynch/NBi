using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Generation.Ansi
{
    class AnsiGenerator : SqlGenerator
    {
        public AnsiGenerator()
        : base(new TargetFormatterAnsi(), new FieldFormatterAnsi())
        { }
    }
}
