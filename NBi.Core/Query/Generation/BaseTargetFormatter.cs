using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Generation
{
    abstract class BaseTargetFormatter : ITargetFormatter
    {
        protected char StartingChar { get; }
        protected char EndingChar { get; }
        protected char Separator { get; }

        public BaseTargetFormatter(char startingChar, char endingChar, char separator)
        {
            StartingChar = startingChar;
            EndingChar = endingChar;
            Separator = separator;
        }

        public virtual string Execute(string schemaName, string tableName) => $"{StartingChar}{schemaName}{EndingChar}{Separator}{StartingChar}{tableName}{EndingChar}";
    }
}
