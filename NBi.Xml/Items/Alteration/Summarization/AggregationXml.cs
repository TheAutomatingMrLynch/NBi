using NBi.Core.ResultSet;
using NBi.Core.Sequence.Transformation.Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Summarization
{
    public abstract class AggregationXml
    {
        [XmlIgnore]
        public AggregationFunctionType Function { get; set; }
        public AggregationXml(AggregationFunctionType function) => Function = function;

        [XmlAttribute("column")]
        public string IdentifierSerializer { get; set; }
        [XmlIgnore]
        public IColumnIdentifier Identifier
        {
            get => new ColumnIdentifierFactory().Instantiate(IdentifierSerializer);
            set => IdentifierSerializer = value.Label;
        }
        [XmlAttribute("type")]
        public ColumnType ColumnType { get; set; }
        [XmlAttribute("if-empty")]
        public string EmptySeriesStrategyName { get; set; }
        [XmlAttribute("if-missing-value")]
        public string MissingValuesStrategyName { get; set; }
        [XmlIgnore]
        public virtual IDictionary<string, object> Parameters { get => new Dictionary<string, object>(); }
    }

    public class SumXml : AggregationXml
    {
        public SumXml() : base(AggregationFunctionType.Sum) { }
    }

    public class AverageXml : AggregationXml
    {
        public AverageXml() : base(AggregationFunctionType.Average) { }
    }

    public class MaxXml : AggregationXml
    {
        public MaxXml() : base(AggregationFunctionType.Max) { }
    }

    public class MinXml : AggregationXml
    {
        public MinXml() : base(AggregationFunctionType.Min) { }
    }

    public class ConcatenateXml : AggregationXml
    {
        public ConcatenateXml() : base(AggregationFunctionType.Concatenate) { }

        [XmlAttribute("separator")]
        public string Separator { get; set; }

        [XmlIgnore]
        public override IDictionary<string, object> Parameters { get => new Dictionary<string, object>() { { "separator", Separator } }; }
    }
}