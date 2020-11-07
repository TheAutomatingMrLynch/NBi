#region Using directives
using System.IO;
using System.Reflection;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NUnit.Framework;
using NBi.Xml.Items.ResultSet;
using NBi.Core.Transformation;
using NBi.Xml.Items.Alteration.Transform;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using NBi.Xml.Systems;
using NBi.Core.Calculation;
using NBi.Xml.Items.Calculation;
#endregion

namespace NBi.Testing.Xml.Unit.Constraints
{
    [TestFixture]
    public class IntersectXmlTest : BaseXmlTest
    {

        [Test]
        public void DeserializeIntersectResultSet_IntersectConstraint_True()
        {
            int testNr = 0;
            var ts = DeserializeSample();
            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<IntersectXml>());
        }

        [Test]
        public void DeserializeIntersectResultSet_IntersectResultSet_IsNotNull()
        {
            int testNr = 0;
            var ts = DeserializeSample();

            var intersect = ts.Tests[testNr].Constraints[0] as IntersectXml;
            Assert.That(intersect.ResultSet, Is.Not.Null);
        }

        [Test]
        public void DeserializeIntersectResultSet_IntersectColumns_IsNotNullOrEmpty()
        {
            int testNr = 0;
            var ts = DeserializeSample();

            var intersect = ts.Tests[testNr].Constraints[0] as IntersectXml;
            Assert.That(intersect.ColumnsDef, Is.Not.Null.And.Not.Empty);
            Assert.That(intersect.ColumnsDef.Count, Is.EqualTo(1));
        }

        [Test]
        public void SerializeIntersectResultSet_IntersectWithResultSetAndColumns_CorrectlySerialized()
        {
            var intersect = new IntersectXml()
            {
                ResultSet = new ResultSetSystemXml()
                {
                    Query = new QueryXml() { InlineQuery = "select * from myTable;" }
                },
                columnsDef = new List<ColumnDefinitionXml>
                { new ColumnDefinitionXml(){ Index = 1 }, new ColumnDefinitionXml(){ Name = "myKey" } }
            };

            var serializer = new XmlSerializer(typeof(IntersectXml));
            var content = string.Empty;
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    serializer.Serialize(writer, intersect);
                content = Encoding.UTF8.GetString(stream.ToArray());
            }

            Debug.WriteLine(content);

            Assert.That(content, Does.Contain("<result-set"));
            Assert.That(content, Does.Contain("select * from myTable;"));
            Assert.That(content, Does.Contain("<column"));
            Assert.That(content, Does.Contain("1"));
            Assert.That(content, Does.Contain("myKey"));
        }
    }
}
