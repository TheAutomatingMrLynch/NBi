using Deedle;
using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Function
{
    class ConcatenateText : BaseAggregation<string>
    {
        public string Separator { get; }

        public ConcatenateText(string separator) : base(new TextCaster())
            => Separator = separator;

        protected override string Execute(Series<int, string> series)
            => string.Join(Separator, series.Values.Select(x => Caster.Execute(x)).ToArray());
    }
}
