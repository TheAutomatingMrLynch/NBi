﻿#region Using directives
using System;
using System.Linq;
using Moq;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Builder;
using NBi.NUnit.Structure;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using NUnit.Framework;
using NBi.Core.Structure.Olap;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class StructureContainBuilderTest
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

        //@@@@@@@@@@@@@@@@@@@@@@@@@
        //    GetConstraint()
        //@@@@@@@@@@@@@@@@@@@@@@@@@

        [Test]
        public void GetConstraint_BuildUniqueItem_CorrectConstraint()
        {
            //Buiding object used during test
            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupsXml()
            {
                ConnectionString = ConnectionStringReader.GetAdomd(),
                Perspective = "Perspective",
            };

            var ctrXml = new ContainXml();
            ctrXml.Caption = "Search";

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<ContainConstraint>());
        }

        [Test]
        public void GetConstraint_BuildMultipleItem_CorrectConstraint()
        {
            //Buiding object used during test
            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupsXml()
            {
                ConnectionString = ConnectionStringReader.GetAdomd(),
                Perspective = "Perspective"
            };

            var ctrXml = new ContainXml();
            ctrXml.Items.Add("Search 1");
            ctrXml.Items.Add("Search 2");
            ctrXml.Items.Add("Search 3");

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<ContainConstraint>());
        }



        //@@@@@@@@@@@@@@@@@@@@@@@@@
        //    GetSystemUnderTest()
        //@@@@@@@@@@@@@@@@@@@@@@@@@


        //**********************
        //       Default ConnectionString
        //**********************

        [Test]
        public void GetSystemUnderTest_ConnectionStringInDefault_CorrectlyInitialized()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();

            sutXml.Item = new MeasureGroupsXml()
            {
                Perspective = "Perspective"
            };

            sutXml.Default = new DefaultXml() { ConnectionString = ConnectionStringReader.GetAdomd() };

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            //Call the method to test
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
        }


        //**********************
        //       Pespective
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectPerspectiveTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new PerspectivesXml()
            {
                ConnectionString = ConnectionStringReader.GetAdomd()
            };

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
        }

        //**********************
        //       Measure-Group
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectMeasureGroupTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupsXml()
            {
                ConnectionString = ConnectionStringReader.GetAdomd(),
                Perspective = "Perspective"
            }; 
            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            //Call the method to test
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
        }

        [Test]
        public void GetSystemUnderTest_InCorrectMeasureGroupTargetWithoutCaption_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupsXml()
            {
                ConnectionString = ConnectionStringReader.GetAdomd(),
                Perspective = "Perspective"
            };
            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
        }


        //**********************
        //       Measure
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectMeasureTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasuresXml()
            {
                ConnectionString = ConnectionStringReader.GetAdomd(),
                Perspective = "Perspective",
                MeasureGroup = "MeasureGroup"
            };
            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
        }

        //**********************
        //       Dimension
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectDimensionTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            var dim = new DimensionsXml();
            dim.ConnectionString = ConnectionStringReader.GetAdomd();
            dim.Perspective = "Perspective";
            sutXml.Item = dim;

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            //Call the method to test
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
        }

        //**********************
        //       Hierarchies
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectHierarchyTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new DimensionsXml()
            {
                ConnectionString = ConnectionStringReader.GetAdomd(),
                Perspective = "Perspective"
            };

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            //Call the method to test
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
        }


        //**********************
        //       Levels
        //**********************


        [Test]
        public void GetSystemUnderTest_CorrectLevelTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new LevelsXml()
            {
                ConnectionString = ConnectionStringReader.GetAdomd(),
                Perspective = "Perspective",
                Dimension = "Dimension",
                Hierarchy = "Hierarchy"
            };
            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
        }


        //**********************
        //       Properties
        //**********************


        [Test]
        public void GetSystemUnderTest_CorrectPropertyTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new PropertiesXml()
            {
                ConnectionString = ConnectionStringReader.GetAdomd(),
                Perspective = "Perspective",
                Dimension = "Dimension",
                Hierarchy = "Hierarchy",
                Level = "Level"
            };
            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
        }


    }
}
