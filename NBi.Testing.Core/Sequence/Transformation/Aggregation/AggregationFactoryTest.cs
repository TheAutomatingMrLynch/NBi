using NBi.Core.ResultSet;
using NBi.Core.Sequence.Transformation.Aggregation;
using NBi.Core.Sequence.Transformation.Aggregation.Function;
using NBi.Core.Sequence.Transformation.Aggregation.Strategy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Sequence.Transformation.Aggregation
{
    public class AggregationFactoryTest
    {
        [Test]
        [TestCase(ColumnType.Numeric,   AggregationFunctionType.Sum,     typeof(SumNumeric))]
        [TestCase(ColumnType.Numeric,   AggregationFunctionType.Average,    typeof(AverageNumeric))]
        [TestCase(ColumnType.Numeric,   AggregationFunctionType.Min,     typeof(MinNumeric))]
        [TestCase(ColumnType.Numeric,   AggregationFunctionType.Max,     typeof(MaxNumeric))]
        [TestCase(ColumnType.DateTime,  AggregationFunctionType.Min,     typeof(MinDateTime))]
        [TestCase(ColumnType.DateTime,  AggregationFunctionType.Max,     typeof(MaxDateTime))]
        public void Instantiate_ColumnTypeandAggregationFunction_CorrectAggregation(ColumnType columnType, AggregationFunctionType function, Type expectedType)
        {
            var factory = new AggregationFactory();
            var aggregation = factory.Instantiate(columnType, function, null, Array.Empty<IAggregationStrategy>());
            Assert.That(aggregation, Is.Not.Null);
            Assert.That(aggregation.Function, Is.TypeOf(expectedType));
        }

        [Test]
        [TestCase(ColumnType.Text, AggregationFunctionType.Concatenate, "separator= - ", typeof(ConcatenateText))]
        public void Instantiate_ColumnTypeAndAggregationFunctionWithParameters_CorrectAggregation(ColumnType columnType, AggregationFunctionType function, string parameters, Type expectedType)
        {
            var factory = new AggregationFactory();
            var dico = new Dictionary<string, object>();
            foreach (var param in parameters.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                dico.Add(param.Split(new[] { '=' })[0], param.Split(new[] { '=' })[1]);
            var aggregation = factory.Instantiate(columnType, function, dico, Array.Empty<IAggregationStrategy>());
            Assert.That(aggregation, Is.Not.Null);
            Assert.That(aggregation.Function, Is.TypeOf(expectedType));
        }

        [TestCase(ColumnType.DateTime, AggregationFunctionType.Sum)]
        [TestCase(ColumnType.DateTime, AggregationFunctionType.Average)]
        [TestCase(ColumnType.Text, AggregationFunctionType.Average)]
        [TestCase(ColumnType.Numeric, AggregationFunctionType.Concatenate)]
        public void Instantiate_ColumnTypeAndAggregationFunction_CorrectAggregation(ColumnType columnType, AggregationFunctionType function)
        {
            var factory = new AggregationFactory();
            Assert.Throws<ArgumentException>( () => factory.Instantiate(columnType, function, null, Array.Empty<IAggregationStrategy>()));
        }
    }
}
