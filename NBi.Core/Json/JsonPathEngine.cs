using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using NBi.Core.ResultSet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NBi.Core.Json
{
    public abstract class JsonPathEngine
    {
        private readonly IEnumerable<AbstractSelect> selects;
        private readonly string from;

        public JsonPathEngine(string from, IEnumerable<AbstractSelect> selects)
        {
            this.from = from;
            this.selects = selects;
        }

        public abstract ResultSet.ResultSet Execute();

        public ResultSet.ResultSet Execute(JObject items)
        {
            var result = from item in items.SelectTokens(@from)
                         select GetObj(item);

            var builder = new ResultSetBuilder();
            var rows = result.ToArray();
            var resultSet = builder.Build(rows);
            return resultSet;
        }

        private object GetObj(JToken x)
        {
            var obj = new List<object>();
            obj.AddRange(BuildXPaths(x, selects).ToArray());
            return obj;
        }

        protected internal IEnumerable<object> BuildXPaths(JToken item, IEnumerable<AbstractSelect> selects)
        {
            foreach (var select in selects)
                if (select is ElementSelect)
                    yield return
                    (
                        item.SelectTokens(select.Path)
                            ?? new JValue((object)null)
                    ).Values<object>();
        }
    }
}
