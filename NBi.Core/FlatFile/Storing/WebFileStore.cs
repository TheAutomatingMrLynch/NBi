using NBi.Extensibility;
using NBi.Extensibility.FlatFile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.FlatFile.Storing
{
    public class WebFileStore : IFlatFileStore, IDisposable
    {
        public string Uri { get; }
        private WebClient WebClient { get; set; }
        private MemoryStream Stream { get; set; }

        public WebFileStore(string uri)
        {
            Uri = uri;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        public Stream GetStream()
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Loading data from flat file from the web with url '{Uri}'");
            WebClient = new WebClient();
            Stream = new MemoryStream(WebClient.DownloadData(Uri));
            return Stream;
        }

        public bool Exists()
        {
            try
            {
                var request = WebRequest.Create(Uri) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }

        #region Disposable

        bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Stream?.Dispose();
                WebClient?.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}