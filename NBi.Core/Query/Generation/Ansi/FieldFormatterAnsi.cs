using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Generation.Ansi
{
    class FieldFormatterAnsi : BaseFieldFormatter
    {
        public FieldFormatterAnsi()
        : base('\0', '\0') { }

        public override string Execute(string fieldName)
        {
            if (fieldName.Any(c => char.IsWhiteSpace(c)))
                throw new ArgumentException("ANSI SQL doesn't support field names with a white space in the name of the field");
            return base.Execute(fieldName);
        }
    }
}
