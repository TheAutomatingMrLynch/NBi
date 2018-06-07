using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predication
{
    class OrCombinationPredication : BaseCombinationPredication
    {
        public OrCombinationPredication(IList<IPredication> predications)
            : base(predications)
        { }

        protected override bool ContinueCondition(bool state) 
            => !state;

        protected override bool StartState()
            => false;

        public override string Description { get => "or"; }

    }
}
