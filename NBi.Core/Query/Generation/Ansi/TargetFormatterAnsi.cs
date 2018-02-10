using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Generation.Ansi
{
    class TargetFormatterAnsi : BaseTargetFormatter
    {
        public TargetFormatterAnsi()
        : base('\0', '\0', '.') { }

        public override string Execute(string schemaName, string tableName)
        {
            if (schemaName.Any(c => char.IsWhiteSpace(c)))
                throw new ArgumentException("ANSI SQL doesn't support schema names with a white space in the name of the field");
            if (tableName.Any(c => char.IsWhiteSpace(c)))
                throw new ArgumentException("ANSI SQL doesn't support table names with a white space in the name of the field");
            return base.Execute(schemaName, tableName);
        }
    }
}
