using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deedle;
using NBi.Core.ResultSet;
using NBi.Core.Sequence.Transformation.Aggregation.Strategy;
using NBi.Core.Sequence.Transformation.Aggregation.Function;
using System.Reflection;
using NBi.Core.Assemblies;

namespace NBi.Core.Sequence.Transformation.Aggregation
{
    public class AggregationFactory
    {
        public Aggregation Instantiate(ColumnType columnType, AggregationFunctionType function, IDictionary<string, object> parameters, IAggregationStrategy[] strategies)
        {
            var missingValue = (IMissingValueStrategy)(strategies.SingleOrDefault(x => x is IMissingValueStrategy) ?? new MissingValueStrategyFactory().Instantiate(columnType));
            var emptySeries = (IEmptySeriesStrategy)(strategies.SingleOrDefault(x => x is IEmptySeriesStrategy) ?? new ReturnDefaultStrategy(0));

            var @namespace = $"{this.GetType().Namespace}.Function.";
            var typeName = $"{Enum.GetName(typeof(AggregationFunctionType), function)}{Enum.GetName(typeof(ColumnType), columnType)}";
            var type = GetType().Assembly.GetType($"{@namespace}{typeName}", false, true) ?? throw new ArgumentException($"No aggregation named '{typeName}' has been found in the namespace '{@namespace}'.");
            var ctor = type.GetConstructors().FirstOrDefault(
                c => c.GetParameters().All(p => (parameters ?? new Dictionary<string, object>()).Keys.Contains(p.Name, StringComparer.InvariantCultureIgnoreCase))
                && c.GetParameters().Count() == (parameters ?? new Dictionary<string, object>()).Count()
            );
            var typeConverter = new TypeConverter();
            var ctorParams = ctor.GetParameters().Select(
                p => typeConverter.Convert(
                    parameters.First(x => string.Compare(x.Key, p.Name, true) == 0).Value
                    , p.ParameterType)
                ).ToArray();
            return new Aggregation((IAggregationFunction)(ctor.Invoke(ctorParams)), missingValue, emptySeries);
        }

        public Aggregation Instantiate(AggregationArgs args)
            => Instantiate(args.ColumnType, args.Function, args.Parameters, args.Strategies.ToArray());
    }
}
