using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predication
{
    class PredicationFactory
    {
        public IPredication Instantiate(IPredicate predicate, IColumnIdentifier operand, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
            => new SinglePredication(predicate, operand, aliases, expressions);

        public IPredication Instantiate(IList<IPredication> predications, CombinationOperator combinationOperator)
        {
            switch (combinationOperator)
            {
                case CombinationOperator.Or:
                    return new OrCombinationPredication(predications);
                case CombinationOperator.XOr:
                    return new XOrCombinationPredication(predications);
                case CombinationOperator.And:
                    return new AndCombinationPredication(predications);
                default:
                    throw new ArgumentOutOfRangeException(nameof(combinationOperator));
            }
        }
    }
}
