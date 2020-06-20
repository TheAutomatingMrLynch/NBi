using NBi.Core.FlatFile;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.FlatFile
{
    [TestFixture]
    public class LocalDiskStoreTest
    {
        class LocalDiskFileStoreProxy : LocalDiskFileStore
        {
            public LocalDiskFileStoreProxy(string basePath, string path)
                : base(basePath, path) { }
            public Stream ProxyStream { get => Stream; }
        }

        private static readonly object locker = new object();

        public static string CreatePhysicalFile(string filename, string resource)
        {
            //if filename starts by a directory separator remove it
            if (filename.StartsWith(Path.DirectorySeparatorChar.ToString()))
                filename = filename.Substring(1);

            //Build the fullpath for the file to read
            var fullpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + filename;

            //create the directory if needed
            if (!Directory.Exists(Path.GetDirectoryName(fullpath)))
                Directory.CreateDirectory(Path.GetDirectoryName(fullpath));

            lock (locker)
            {
                //delete it if already existing
                if (File.Exists(fullpath))
                    File.Delete(fullpath);

                // A Stream is needed to read the XLS document.
                using (Stream stream = Assembly.GetExecutingAssembly()
                                               .GetManifestResourceStream(resource))
                {
                    if (stream == null)
                        throw new FileNotFoundException(resource);

                    //Open another stream to persist the file on disk
                    using (Stream file = File.OpenWrite(fullpath))
                    {
                        byte[] buffer = new byte[8 * 1024];
                        int len;
                        while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            file.Write(buffer, 0, len);
                        }
                    }
                }
            }
            return fullpath;
        }

        [Test]
        public void Read_File_StreamOpenStartingAtZero()
        {
            var path = CreatePhysicalFile("MyFile.txt", $"{GetType().Namespace}.Resources.MyData_2016_01.csv");
            using (var store = new LocalDiskFileStore(string.Empty, path))
            {
                var input = store.GetStream();
                Assert.That(input.CanRead, Is.True);
                Assert.That(input.Position, Is.EqualTo(0));
            }
        }
    }
}
