using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Predication;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    class PredicationFilter : BaseFilter
    {
        private readonly IPredication predication;

        public PredicationFilter(IPredication predication)
        {
            this.predication = predication;
        }

        protected override bool RowApply(DataRow row)
            => predication.Execute(row);

        public override string Describe()
            => $"{predication.Describe()}";
    }
}
