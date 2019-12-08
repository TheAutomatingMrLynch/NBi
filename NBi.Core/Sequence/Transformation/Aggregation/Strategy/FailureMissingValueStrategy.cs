using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Strategy
{
    abstract class FailureMissingValueStrategy<T> : IMissingValueStrategy
    {
        public ICaster<T> Caster { get; }

        protected FailureMissingValueStrategy(ICaster<T> caster)
            => Caster = caster;

        public IEnumerable<object> Execute(IEnumerable<object> values)
        {
            if (values.All(x => Caster.IsStrictlyValid(x)))
                return values.Select(x => Caster.Execute(x)).Cast<object>();
            else
                throw new ArgumentException();
        }
    }


    class FailureMissingValueStrategyText : FailureMissingValueStrategy<string>
    {
        public FailureMissingValueStrategyText()
        : base(new TextCaster()) { }
    }

    class FailureMissingValueStrategyNumeric : FailureMissingValueStrategy<decimal>
    {
        public FailureMissingValueStrategyNumeric()
        : base(new NumericCaster()) { }
    }

    class FailureMissingValueStrategyDateTime : FailureMissingValueStrategy<DateTime>
    {
        public FailureMissingValueStrategyDateTime()
        : base(new DateTimeCaster()) { }
    }
}
