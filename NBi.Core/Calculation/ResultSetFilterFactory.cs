using NBi.Core.Calculation.Grouping;
using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Predication;
using NBi.Core.Calculation.Ranking;
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
    public class ResultSetFilterFactory
    {
        public IResultSetFilter Instantiate(IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, IPredicateInfo predicateInfo)
        {
            if (predicateInfo.Operand == null)
                throw new ArgumentException("You must specify an operand for a predicate. The operand is the column or alias or expression on which the predicate will be evaluated.");

            var predicateFactory = new PredicateFactory();
            var predicate = predicateFactory.Instantiate(predicateInfo);

            var predicationFactory = new PredicationFactory();
            var predication = predicationFactory.Instantiate(predicate, predicateInfo.Operand, aliases, expressions);

            var filter = new PredicationFilter(predication);

            return filter;
        }

        public IResultSetFilter Instantiate(IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, CombinationOperator combinationOperator, IEnumerable<IPredicateInfo> predicateInfos)
        {
            var predications = new List<IPredication>();

            var predicateFactory = new PredicateFactory();
            var predicationFactory = new PredicationFactory();
            foreach (var predicateInfo in predicateInfos)
            {
                if (predicateInfo.Operand == null)
                    throw new ArgumentException("You must specify an operand for a predicate. The operand is the column or alias or expression on which the predicate will be evaluated.");

                var predicate = predicateFactory.Instantiate(predicateInfo);
                
                var localPredication = predicationFactory.Instantiate(predicate, predicateInfo.Operand, aliases, expressions);
                predications.Add(localPredication);
            }
            var predication = predicationFactory.Instantiate(predications, combinationOperator);

            var filter = new PredicationFilter(predication);
            return filter;
        }

        public IResultSetFilter Instantiate(IRankingInfo rankingInfo, IEnumerable<IColumnDefinitionLight> columns)
        {
            var groupingFactory = new GroupByFactory();
            var grouping = groupingFactory.Instantiate(columns);

            var rankingFactory = new RankingFactory();
            var ranking = rankingFactory.Instantiate(rankingInfo);

            return new FilterGroupByFilter(ranking, grouping);
        }

        //public IResultSetFilter Instantiate(IRankingInfo rankingInfo, IEnumerable<IPredication> predications)
        //{
        //    var groupingFactory = new GroupByFactory();
        //    var grouping = groupingFactory.Instantiate(columns);

        //    var rankingFactory = new RankingFactory();
        //    var ranking = rankingFactory.Instantiate(rankingInfo);

        //    return new FilterGroupByFilter(ranking, grouping);
        //}
    }
}
