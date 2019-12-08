using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation
{
    public class AggregationArgs
    {
        public ColumnType ColumnType { get; }
        public AggregationFunctionType Function { get; }
        public IList<IAggregationStrategy> Strategies { get; } = new List<IAggregationStrategy>();
        public IDictionary<string, object> Parameters { get; } = new Dictionary<string, object>();

        public AggregationArgs(AggregationFunctionType function, ColumnType columnType, IDictionary<string, object> parameters)
            => (ColumnType, Function, Parameters) = (columnType, function, parameters);
    }
}
