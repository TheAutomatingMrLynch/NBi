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
    public class OrCombinationPredicationTest
    {
        [Test]
        public void Execute_TwoTrue_True()
        {
            var leftPredication = Mock.Of<IPredication>(x => x.Execute(It.IsAny<DataRow>()) == true);
            var RightPredication = Mock.Of<IPredication>(x => x.Execute(It.IsAny<DataRow>()) == true);

            var factory = new PredicationFactory();
            var predication = factory.Instantiate(new[] { leftPredication, RightPredication }, CombinationOperator.Or);

            var dt = new DataTable();
            var row = dt.NewRow();

            Assert.That(predication.Execute(row), Is.True);
        }

        [Test]
        public void Execute_TrueFalse_True()
        {
            var leftPredication = Mock.Of<IPredication>(x => x.Execute(It.IsAny<DataRow>()) == true);
            var RightPredication = Mock.Of<IPredication>(x => x.Execute(It.IsAny<DataRow>()) == false);

            var factory = new PredicationFactory();
            var predication = factory.Instantiate(new[] { leftPredication, RightPredication }, CombinationOperator.Or);

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
            var predication = factory.Instantiate(new[] { leftPredication, RightPredication }, CombinationOperator.Or);

            var dt = new DataTable();
            var row = dt.NewRow();

            Assert.That(predication.Execute(row), Is.True);
        }


        [Test]
        public void Execute_FalseFalse_False()
        {
            var leftPredication = Mock.Of<IPredication>(x => x.Execute(It.IsAny<DataRow>()) == false);
            var RightPredication = Mock.Of<IPredication>(x => x.Execute(It.IsAny<DataRow>()) == false);

            var factory = new PredicationFactory();
            var predication = factory.Instantiate(new[] { leftPredication, RightPredication }, CombinationOperator.Or);

            var dt = new DataTable();
            var row = dt.NewRow();

            Assert.That(predication.Execute(row), Is.False);
        }

        [Test]
        public void Execute_TrueFalse_StopOnFirst()
        {
            var leftPredicationMock = new Mock<IPredication>();
            leftPredicationMock.Setup(x => x.Execute(It.IsAny<DataRow>())).Returns(true);
            var rightPredicationMock = new Mock<IPredication>();
            rightPredicationMock.Setup(x => x.Execute(It.IsAny<DataRow>())).Returns(false);

            var factory = new PredicationFactory();
            var predication = factory.Instantiate(new[] { leftPredicationMock.Object, rightPredicationMock.Object }, CombinationOperator.Or);

            var dt = new DataTable();
            var row = dt.NewRow();
            predication.Execute(row);

            leftPredicationMock.Verify(x => x.Execute(row), Times.Once);
            rightPredicationMock.Verify(x => x.Execute(It.IsAny<DataRow>()), Times.Never);
        }


    }
}
