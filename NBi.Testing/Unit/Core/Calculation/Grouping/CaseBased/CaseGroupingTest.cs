using NBi.Core.Calculation.Grouping.CaseBased;
using NBi.Core.Calculation.Grouping.ColumnBased;
using NBi.Core.Calculation.Predicate.Text;
using NBi.Core.Calculation.Predication;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Comparer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Calculation.Grouping.CaseBased
{
    public class GroupByCaseTest
    {
        [Test]
        public void Execute_SingleColumn_TwoGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", 1 }, new object[] { "beta", 2 }, new object[] { "BETA", 3 }, new object[] { "alpha", 4 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            var lowerCase = new SinglePredication(new TextLowerCase(false), new ColumnIdentifierFactory().Instantiate("#0"), new List<IColumnAlias>(), new List<IColumnExpression>());
            var upperCase = new SinglePredication(new TextUpperCase(false), new ColumnIdentifierFactory().Instantiate("#0"), new List<IColumnAlias>(), new List<IColumnExpression>());

            var grouping = new CaseGrouping(new IPredication[] { lowerCase, upperCase });

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].Rows, Has.Count.EqualTo(3));
            Assert.That(result[1].Rows, Has.Count.EqualTo(1));
        }

        [Test]
        public void Execute_TwoColumns_ThreeGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", "1", 10 }, new object[] { "ALPHA", "1", 20 }, new object[] { "beta", "2", 30 }, new object[] { "ALPHA", "2", 40 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            var lowerCase = new SinglePredication(new TextLowerCase(false), new ColumnIdentifierFactory().Instantiate("#0"), new List<IColumnAlias>(), new List<IColumnExpression>());
            var upperCase = new AndCombinationPredication( new List<IPredication>()
                {
                    new SinglePredication(new TextUpperCase(false), new ColumnIdentifierFactory().Instantiate("#0"), new List<IColumnAlias>(), new List<IColumnExpression>()),
                    new SinglePredication(new TextEqual(false, "1"), new ColumnIdentifierFactory().Instantiate("#1"), new List<IColumnAlias>(), new List<IColumnExpression>()),
                });

            var grouping = new CaseGrouping(new IPredication[] { lowerCase, upperCase });

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[0].Rows, Has.Count.EqualTo(2));
            Assert.That(result[1].Rows, Has.Count.EqualTo(1));
            Assert.That(result[-1].Rows, Has.Count.EqualTo(1));
        }
    }
}
