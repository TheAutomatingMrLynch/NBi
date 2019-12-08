using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Strategy
{
    abstract class ReplaceByDefaultStrategy<T> : IMissingValueStrategy
    {
        private T DefaultValue { get; }
        public ICaster<T> Caster { get; }

        protected ReplaceByDefaultStrategy(ICaster<T> caster, T defaultValue) 
            => (Caster, DefaultValue) = (caster, defaultValue);

        public IEnumerable<object> Execute(IEnumerable<object> values)
            => values.Select(x => Caster.IsStrictlyValid(x) ? Caster.Execute(x) : DefaultValue).Cast<object>();
    }

    class ReplaceByDefaultStrategyText :ReplaceByDefaultStrategy<string>
    {
        public ReplaceByDefaultStrategyText(string defaultValue)
        : base(new TextCaster(), defaultValue) { }
    }

    class ReplaceByDefaultStrategyNumeric :ReplaceByDefaultStrategy<decimal>
    {
        public ReplaceByDefaultStrategyNumeric(decimal defaultValue)
        : base(new NumericCaster(), defaultValue) { }
    }

    class ReplaceByDefaultStrategyDateTime :ReplaceByDefaultStrategy<DateTime>
    {
        public ReplaceByDefaultStrategyDateTime(DateTime defaultValue)
        : base(new DateTimeCaster(), defaultValue) { }
    }
}
