using NBi.Core.Sequence.Transformation.Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Summarization
{
    public class ColumnAggregationArgs : AggregationArgs
    {
        public IColumnIdentifier Identifier { get; }

        protected internal ColumnAggregationArgs(IColumnIdentifier identifier, AggregationFunctionType function, ColumnType columnType)
            : this(identifier, function, columnType, new Dictionary<string, object>()) { }

        public ColumnAggregationArgs(IColumnIdentifier identifier, AggregationFunctionType function, ColumnType columnType, IDictionary<string, object> parameters)
            : base(function, columnType, parameters)
            => (Identifier) = (identifier);
    }
}
