﻿#region Using directives
using System.Data;
using Moq;
using NBi.NUnit.Builder;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Settings;
using NUnit.Framework;
using Items = NBi.Xml.Items;
using Systems = NBi.Xml.Systems;
using NBi.Xml.Constraints.Comparer;
using NBi.NUnit.Execution;
using NUnitCtr = NUnit.Framework.Constraints;
using System;
using NBi.Xml.Items.Calculation;
using System.Collections.Generic;
using NBi.Core.Variable;
using NBi.Core.ResultSet;
using NBi.Core.Injection;
using NBi.NUnit.ResultSetComparison;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class ResultSetReferenceExistsBuilderTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        [Test]
        public void GetConstraint_ReferenceExistsXml_ReferenceExistsConstraint()
        {
            var sutXmlStub = new Mock<Systems.ResultSetSystemXml>();
            sutXmlStub.Setup(s => s.File).Returns("myChild.csv");
            var sutXml = sutXmlStub.Object;

            var ctrXml = new ReferenceExistsXml();
            var parentXmlStub = new Mock<Systems.ResultSetSystemXml>();
            parentXmlStub.Setup(s => s.File).Returns("myParent.csv");
            ctrXml.ResultSet = parentXmlStub.Object;

            var builder = new ResultSetReferenceExistsBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<ReferenceExistsConstraint>());
        }


        [Test]
        public void GetSystemUnderTest_ResultSetSystemXml_IResultSetService()
        {
            var sutXmlStub = new Mock<Systems.ResultSetSystemXml>();
            sutXmlStub.Setup(s => s.File).Returns("myFile.csv");
            var sutXml = sutXmlStub.Object;

            var ctrXml = new ReferenceExistsXml();
            var parentXmlStub = new Mock<Systems.ResultSetSystemXml>();
            parentXmlStub.Setup(s => s.File).Returns("myParent.csv");
            ctrXml.ResultSet = parentXmlStub.Object;

            var builder = new ResultSetReferenceExistsBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.InstanceOf<IResultSetService>());
        }
    }
}