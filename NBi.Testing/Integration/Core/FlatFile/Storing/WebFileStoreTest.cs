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
    public class WebFileStoreTest
    {
        [Test]
        public void Read_File_StreamOpenStartingAtZero()
        {
            var path = "http://github.com/Seddryck/NBi/blob/develop/NBi.Testing/Acceptance/Resources/Csv/MyData_2016_01.csv";
            using (var store = new WebFileStore(path))
            {
                var input = store.GetStream();
                Assert.That(input.CanRead, Is.True);
                Assert.That(input.Position, Is.EqualTo(0));
            }
        }
    }
}
