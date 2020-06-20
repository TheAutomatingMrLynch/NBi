using NBi.Extensibility;
using NBi.Extensibility.FlatFile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.FlatFile
{
    public class LocalDiskFileStore : IFlatFileStore, IDisposable
    {
        public string FullPath { get; }
        public Stream Stream { get; private set; }

        public LocalDiskFileStore(string basePath, string path)
        {
            FullPath = (Path.IsPathRooted(path)) ? path : basePath + path;
        }

        public Stream GetStream()
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Loading data from flat file on local disk '{FullPath}'");
            Stream = new FileStream(FullPath, FileMode.Open, FileAccess.Read);
            return Stream;
        }

        public bool Exists() => File.Exists(FullPath);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stream?.Dispose();
            }
        }
    }
}
