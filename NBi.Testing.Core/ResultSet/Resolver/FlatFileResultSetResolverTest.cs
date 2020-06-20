using NBi.Extensibility.Resolving;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Injection;
using NBi.Core.FlatFile;
using System.IO;
using System.Reflection;
using NBi.Extensibility.FlatFile;
using Moq;
using NBi.Extensibility;
using NBi.Core.FlatFile.Parsing;

namespace NBi.Testing.Core.ResultSet.Resolver
{
    public class FlatFileResultSetResolverTest
    {
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
                using (var stream = Assembly.GetExecutingAssembly()
                                               .GetManifestResourceStream(resource))
                {
                    if (stream == null)
                        throw new FileNotFoundException(resource);

                    //Open another stream to persist the file on disk
                    using (var file = File.OpenWrite(fullpath))
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

        [Test()]
        public void Instantiate_OneSequence_CorrectType()
        {
            var path = CreatePhysicalFile("MyFile.txt", $"{GetType().Namespace}.Resources.MyData_2016_01.csv");

            var resolver = new FlatFileResultSetResolver(
                new FlatFileResultSetResolverArgs(
                    new LiteralScalarResolver<string>(path),
                    string.Empty,
                    string.Empty,
                    CsvProfile.SemiColumnDoubleQuote
                ), new ServiceLocator()
            );

            var rs = resolver.Execute();

            Assert.That(rs.Columns.Count, Is.EqualTo(3));
            Assert.That(rs.Rows.Count, Is.EqualTo(2));
            Assert.That(rs.Rows[0][0], Is.EqualTo("2016-01-01"));
            Assert.That(rs.Rows[1][0], Is.EqualTo("2016-01-04"));
        }

        [Test]
        public void Provider_AllMethodsCalled_Once()
        {
            var resolver = new FlatFileResultSetResolver(
                new FlatFileResultSetResolverArgs(
                    new LiteralScalarResolver<string>(string.Empty),
                    string.Empty,
                    string.Empty,
                    CsvProfile.SemiColumnDoubleQuote
                ), new ServiceLocator()
            );

            var stream = new Mock<Stream>();

            var provider = new Mock<IFlatFileStore>();
            provider.Setup(x => x.Exists()).Returns(true);
            provider.Setup(x => x.GetStream()).Returns(stream.Object);

            var reader = new Mock<IFlatFileReader>();
            reader.Setup(x => x.ToDataTable(stream.Object)).Returns(new DataTable());

            var result = resolver.Execute(provider.Object, reader.Object);

            provider.Verify(x => x.Exists(), Times.Once);
            provider.Verify(x => x.GetStream(), Times.Once);
        }

        [Test]
        public void Provider_FileDoesntExist_NoGetStream()
        {
            var resolver = new FlatFileResultSetResolver(
                new FlatFileResultSetResolverArgs(
                    new LiteralScalarResolver<string>(string.Empty),
                    string.Empty,
                    string.Empty,
                    CsvProfile.SemiColumnDoubleQuote
                ), new ServiceLocator()
            );

            var stream = new Mock<Stream>();

            var provider = new Mock<IFlatFileStore>();
            provider.Setup(x => x.Exists()).Returns(false);
            provider.Setup(x => x.GetStream()).Returns(stream.Object);

            var reader = new Mock<IFlatFileReader>();
            reader.Setup(x => x.ToDataTable(stream.Object)).Returns(new DataTable());

            Assert.Throws<ExternalDependencyNotFoundException>(() => resolver.Execute(provider.Object, reader.Object));

            provider.Verify(x => x.Exists(), Times.Once);
            provider.Verify(x => x.GetStream(), Times.Never);
        }

        [Test]
        public void Reader_AllMethodsCalled_Once()
        {
            var resolver = new FlatFileResultSetResolver(
                new FlatFileResultSetResolverArgs(
                    new LiteralScalarResolver<string>(string.Empty),
                    string.Empty,
                    string.Empty,
                    CsvProfile.SemiColumnDoubleQuote
                ), new ServiceLocator()
            );

            var stream = new Mock<Stream>();

            var provider = new Mock<IFlatFileStore>();
            provider.Setup(x => x.Exists()).Returns(true);
            provider.Setup(x => x.GetStream()).Returns(stream.Object);

            var reader = new Mock<IFlatFileReader>();
            reader.Setup(x => x.ToDataTable(stream.Object)).Returns(new DataTable());

            var result = resolver.Execute(provider.Object, reader.Object);

            reader.Verify(x => x.ToDataTable(stream.Object), Times.Once);
        }

    }
}