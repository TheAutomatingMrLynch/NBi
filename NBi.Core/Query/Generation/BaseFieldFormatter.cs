using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Generation
{
    abstract class BaseFieldFormatter : IFieldFormatter
    {
        protected char StartingChar { get; }
        protected char EndingChar { get; }

        public BaseFieldFormatter(char startingChar, char endingChar)
        {
            StartingChar = startingChar;
            EndingChar = endingChar;
        }

        public virtual string Execute(string fieldName) => $"{StartingChar}{fieldName}{EndingChar}";
    }
}
