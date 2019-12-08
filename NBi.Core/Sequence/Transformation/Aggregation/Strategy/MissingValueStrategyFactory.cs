using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Strategy
{
    class MissingValueStrategyFactory
    {
        public IMissingValueStrategy Instantiate(ColumnType columnType)
        {
            switch (columnType)
            {
                case ColumnType.Text: return new DropStrategyText();
                case ColumnType.Numeric: return new DropStrategyNumeric();
                case ColumnType.DateTime: return new DropStrategyDateTime();
                default: throw new ArgumentException();
            }
        }
    }
}
