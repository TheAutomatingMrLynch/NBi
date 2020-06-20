using NBi.Core.FlatFile.Storing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Integration.Core.FlatFile.Storing
{
    [TestFixture]
    public class LocalDiskStoreTest
    {
        class LocalDiskFileStoreProxy : LocalDiskFileStore
        {
            public LocalDiskFileStoreProxy(string path)
                : base(path) { }
            public Stream ProxyStream { get => Stream; }
        }

        [Test]
        public void Read_File_StreamOpenStartingAtZero()
        {
            var path = DiskOnFile.CreatePhysicalFile("MyFile.txt", $"{GetType().Namespace}.Resources.MyData_2016_01.csv");
            using (var store = new LocalDiskFileStore(path))
            {
                var input = store.GetStream();
                Assert.That(input.CanRead, Is.True);
                Assert.That(input.Position, Is.EqualTo(0));
            }
        }
    }
}
