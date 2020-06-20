using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility.FlatFile
{
    public interface IFlatFileStore : IDisposable
    {
        string Uri { get; }
        bool Exists();
        Stream GetStream();
    }
}
