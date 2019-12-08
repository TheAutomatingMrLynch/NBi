using NBi.Core.ResultSet;
using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Strategy
{
    abstract class DropStrategy<T> : IMissingValueStrategy
    {
        public ICaster<T> Caster { get; }

        protected DropStrategy(ICaster<T> caster)
            => Caster = caster;

        public IEnumerable<object> Execute(IEnumerable<object> values)
            =>  values.Where(x => Caster.IsStrictlyValid(x)).Select(x => Caster.Execute(x)).Cast<object>(); 
    }

    class DropStrategyText : DropStrategy<string>
    {
        public DropStrategyText() 
        : base(new TextCaster()){ }
    }

    class DropStrategyNumeric : DropStrategy<decimal>
    {
        public DropStrategyNumeric()
        : base(new NumericCaster()) { }
    }

    class DropStrategyDateTime : DropStrategy<DateTime>
    {
        public DropStrategyDateTime()
        : base(new DateTimeCaster()) { }
    }
}
