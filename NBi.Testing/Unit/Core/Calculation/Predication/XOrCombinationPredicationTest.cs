using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core;
using NBi.Core.Calculation;
using Moq;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using System.Data;
using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Predication;

namespace NBi.Testing.Unit.Core.Calculation.Predication
{
    public class XOrCombinationPredicationTest
    {
        [Test]
        public void Execute_TwoTrue_False()
        {
            var leftPredication = Mock.Of<IPredication>(x => x.Execute(It.IsAny<DataRow>()) == true);
            var RightPredication = Mock.Of<IPredication>(x => x.Execute(It.IsAny<DataRow>()) == true);

            var factory = new PredicationFactory();
            var predication = factory.Instantiate(new[] { leftPredication, RightPredication }, CombinationOperator.XOr);

            var dt = new DataTable();
            var row = dt.NewRow();

            Assert.That(predication.Execute(row), Is.False);
        }

        [Test]
        public void Execute_TwoFalse_False()
        {
            var leftPredication = Mock.Of<IPredication>(x => x.Execute(It.IsAny<DataRow>()) == false);
            var RightPredication = Mock.Of<IPredication>(x => x.Execute(It.IsAny<DataRow>()) == false);

            var factory = new PredicationFactory();
            var predication = factory.Instantiate(new[] { leftPredication, RightPredication }, CombinationOperator.XOr);

            var dt = new DataTable();
            var row = dt.NewRow();

            Assert.That(predication.Execute(row), Is.False);
        }

        [Test]
        public void Execute_TrueFalse_True()
        {
            var leftPredication = Mock.Of<IPredication>(x => x.Execute(It.IsAny<DataRow>()) == true);
            var RightPredication = Mock.Of<IPredication>(x => x.Execute(It.IsAny<DataRow>()) == false);

            var factory = new PredicationFactory();
            var predication = factory.Instantiate(new[] { leftPredication, RightPredication }, CombinationOperator.XOr);

            var dt = new DataTable();
            var row = dt.NewRow();

            Assert.That(predication.Execute(row), Is.True);
        }

        [Test]
        public void Execute_FalseTrue_True()
        {
            var leftPredication = Mock.Of<IPredication>(x => x.Execute(It.IsAny<DataRow>()) == false);
            var RightPredication = Mock.Of<IPredication>(x => x.Execute(It.IsAny<DataRow>()) == true);

            var factory = new PredicationFactory();
            var predication = factory.Instantiate(new[] { leftPredication, RightPredication }, CombinationOperator.XOr);

            var dt = new DataTable();
            var row = dt.NewRow();

            Assert.That(predication.Execute(row), Is.True);
        }

        [Test]
        public void Execute_TrueFalseTrue_GoUntilTheEnd()
        {
            var predicationMock1 = new Mock<IPredication>();
            predicationMock1.Setup(x => x.Execute(It.IsAny<DataRow>())).Returns(true);
            var predicationMock2 = new Mock<IPredication>();
            predicationMock2.Setup(x => x.Execute(It.IsAny<DataRow>())).Returns(false);
            var predicationMock3 = new Mock<IPredication>();
            predicationMock3.Setup(x => x.Execute(It.IsAny<DataRow>())).Returns(false);

            var factory = new PredicationFactory();
            var predication = factory.Instantiate(new[] { predicationMock1.Object, predicationMock2.Object, predicationMock3.Object }, CombinationOperator.XOr);

            var dt = new DataTable();
            var row = dt.NewRow();
            predication.Execute(row);

            predicationMock1.Verify(x => x.Execute(row), Times.Once);
            predicationMock2.Verify(x => x.Execute(row), Times.Once);
            predicationMock3.Verify(x => x.Execute(row), Times.Once);
        }


    }
}
