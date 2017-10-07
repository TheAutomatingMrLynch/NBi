using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NBi.Core.Json
{
    public class JsonPathUrlEngine : JsonPathEngine
    {
        public string Url { get; private set; }

        public JsonPathUrlEngine(string url, string from, IEnumerable<AbstractSelect> selects)
            : base(from, selects)
        {
            this.Url = url;
        }

        public override ResultSet.ResultSet Execute()
        {
            using (var reader = new JsonTextReader(new StreamReader(Url)))
            {
                var json = JObject.Load(reader);
                return Execute(json);
            }
        }
    }
}
