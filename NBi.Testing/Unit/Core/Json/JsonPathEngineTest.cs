using NBi.Core.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Json
{
    public class JsonPathEngineTest
    {
        private class JsonPathStreamEngine : JsonPathEngine
        {
            private readonly StreamReader streamReader;

            public JsonPathStreamEngine(StreamReader streamReader, string from, IEnumerable<ElementSelect> selects)
                : base(from, selects)
            {
                this.streamReader = streamReader;
            }

            public override NBi.Core.ResultSet.ResultSet Execute()
            {
                using (var reader = new JsonTextReader(streamReader))
                {
                    var json = JObject.Load(reader);
                    return Execute(json);
                };
            }
        }

        protected StreamReader GetResourceReader()
        {
            // A Stream is needed to read the XML document.
            var stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Core.Resources.PurchaseOrders.json");
            var reader = new StreamReader(stream);
            return reader;
        }


        [Test]
        public void Execute_Example_ColumnCount()
        {
            var from = "$.Addresses";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect("[*].Name")
                , new ElementSelect("[*].Country")
                , new ElementSelect("$.PurchaseOrderNumber")
            };

            using (var reader = GetResourceReader())
            {
                var engine = new JsonPathStreamEngine(reader, from, selects);
                var result = engine.Execute();
                Assert.That(result.Columns.Count, Is.EqualTo(3));
                Assert.That(result.Rows.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public void Execute_Example_ValueCheck()
        {
            var from = "$.Addresses";
            var selects = new List<ElementSelect>()
            {
                new ElementSelect("@.Name")
                , new ElementSelect("@.Country")
                , new ElementSelect("@..PurchaseOrderNumber")
            };

            using (var reader = GetResourceReader())
            {
                var engine = new JsonPathStreamEngine(reader, from, selects);
                var result = engine.Execute();
                Assert.That(result.Rows[0].ItemArray[0], Is.EqualTo("Ellen Adams"));
            }
        }

    }
}
