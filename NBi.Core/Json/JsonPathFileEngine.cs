using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NBi.Core.Json
{
    public class JsonPathFileEngine : JsonPathEngine
    {
        public string FilePath { get; private set; }

        public JsonPathFileEngine(string filePath, string from, IEnumerable<AbstractSelect> selects)
            : base(from, selects)
        {
            this.FilePath = filePath;
        }

        public override ResultSet.ResultSet Execute()
        {
            if (!File.Exists(FilePath))
                throw new InvalidOperationException(string.Format("File '{0}' doesn't exist!", FilePath));

            using (var reader = new JsonTextReader(new StreamReader(FilePath)))
            {
                var json = JObject.Load(reader);
                return Execute(json);
            }
        }
    }
}
