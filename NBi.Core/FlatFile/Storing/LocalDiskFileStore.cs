using NBi.Extensibility;
using NBi.Extensibility.FlatFile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.FlatFile.Storing
{
    public class LocalDiskFileStore : IFlatFileStore, IDisposable
    {
        public string Uri { get; }
        public Stream Stream { get; private set; }

        public LocalDiskFileStore(string uri) => Uri = uri;
        
        public Stream GetStream()
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Loading data from flat file on local disk '{Uri}'");
            Stream = new FileStream(Uri, FileMode.Open, FileAccess.Read);
            return Stream;
        }

        public bool Exists() => File.Exists(Uri);

        #region Disposable

        bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Stream?.Dispose();
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
