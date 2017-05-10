using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Rest
{
    public class RestCommand 
    {
        private readonly IRestClient client;

        public RestCommand(IRestClient client)
        {
            this.client = client;
            Parameters = new Dictionary<string, string>();
        }

        public string Uri { get; set; }
        public IDictionary<string, string> Parameters { get; }

        public virtual DataSet Execute()
        {
            var builder = new UriParametersBuilder();
            builder.Setup(Uri, Parameters);
            builder.Build();
            var content = client.Download(builder.GetUri(), builder.GetRemainingParameters());
            var dataset = client.Parse(content);
            
            return dataset;
        }

       
    }
}
