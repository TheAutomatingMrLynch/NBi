using NBi.Core.FlatFile.Storing;
using NBi.Core.Injection;
using NBi.Extensibility;
using NBi.Extensibility.FlatFile;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.FlatFile.Storing
{
    public class FlatFileStoreFactoryTest
    {
        #region Fake
        public class FakeFlatFileStore : IFlatFileStore
        {
            public FakeFlatFileStore(string uri) { }
            public string Uri => throw new NotImplementedException();
#pragma warning disable CA1063 // Implement IDisposable Correctly
            public void Dispose() => throw new NotImplementedException();
#pragma warning restore CA1063 // Implement IDisposable Correctly
            public bool Exists() => throw new NotImplementedException();
            public Stream GetStream() => throw new NotImplementedException();
        }

        public class FakeFlatFileStore2 : IFlatFileStore
        {
            public FakeFlatFileStore2(string uri) { }
            public string Uri => throw new NotImplementedException();
#pragma warning disable CA1063 // Implement IDisposable Correctly
            public void Dispose() => throw new NotImplementedException();
#pragma warning restore CA1063 // Implement IDisposable Correctly
            public bool Exists() => throw new NotImplementedException();
            public Stream GetStream() => throw new NotImplementedException();
        }

        public class FakeFlatFileStore3 : IFlatFileStore
        {
            public FakeFlatFileStore3(string uri) { }
            public string Uri => throw new NotImplementedException();
#pragma warning disable CA1063 // Implement IDisposable Correctly
            public void Dispose() => throw new NotImplementedException();
#pragma warning restore CA1063 // Implement IDisposable Correctly
            public bool Exists() => throw new NotImplementedException();
            public Stream GetStream() => throw new NotImplementedException();
        }

        public class FakeFlatFileStoreWrong : IFlatFileStore
        {
            public FakeFlatFileStoreWrong(int whatsup)
                : base() { }

            public string Uri => throw new NotImplementedException();
#pragma warning disable CA1063 // Implement IDisposable Correctly
            public void Dispose() => throw new NotImplementedException();
#pragma warning restore CA1063 // Implement IDisposable Correctly
            public bool Exists() => throw new NotImplementedException();
            public Stream GetStream() => throw new NotImplementedException();
        }
        #endregion

        [Test]
        public void Instantiate_OneExtensionNoScheme_BasicProvider()
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileStore), new Dictionary<string, string>() { { "extension", "fake" } } },
            };
            config.LoadExtensions(extensions);

            var factory = localServiceLocator.GetFlatFileStoreFactory();
            var engine = factory.Instantiate(string.Empty, "C:\\file.txt");
            Assert.IsInstanceOf<LocalDiskFileStore>(engine);
        }

        [Test]
        public void Instantiate_OneExtensionFileScheme_LocalDiskProvider()
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileStore), new Dictionary<string, string>() { { "extension", "fake" } } },
            };
            config.LoadExtensions(extensions);

            var factory = localServiceLocator.GetFlatFileStoreFactory();
            var engine = factory.Instantiate(string.Empty, $"file://sharedDisk/path/myFile.txt");
            Assert.IsInstanceOf<LocalDiskFileStore>(engine);
        }

        [TestCase("http")]
        [TestCase("https")]
        [Test]
        public void Instantiate_OneExtensionWebScheme_WebProvider(string scheme)
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileStore), new Dictionary<string, string>() { { "extension", "fake" } } },
            };
            config.LoadExtensions(extensions);

            var factory = localServiceLocator.GetFlatFileStoreFactory();
            var engine = factory.Instantiate(string.Empty, $"{scheme}://www.path.com/myFile.txt");
            Assert.IsInstanceOf<WebFileStore>(engine);
        }

        [Test]
        public void Instantiate_OneExtension_ThisExtensionIsReturned()
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileStore), new Dictionary<string, string>() { { "extension", "fake" } } },
            };
            config.LoadExtensions(extensions);

            var factory = localServiceLocator.GetFlatFileStoreFactory();
            var store = factory.Instantiate(string.Empty, "fake://file.txt");
            Assert.IsInstanceOf<FakeFlatFileStore>(store);
        }

        [Test]
        public void Instantiate_ThreeExtensions_CorrectExtensionLoaded()
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileStore), new Dictionary<string, string>() { { "extension", "fake" } } },
                { typeof(FakeFlatFileStore2), new Dictionary<string, string>() { { "extension", "fake2" } } },
                { typeof(FakeFlatFileStore3), new Dictionary<string, string>() { { "extension", "fakeThree" } } },
            };
            config.LoadExtensions(extensions);

            var factory = localServiceLocator.GetFlatFileStoreFactory();
            Assert.IsInstanceOf<FakeFlatFileStore2>(factory.Instantiate(string.Empty, "fake2://file.txt"));
            Assert.IsInstanceOf<FakeFlatFileStore>(factory.Instantiate(string.Empty, "fake://file.txt"));
            Assert.IsInstanceOf<FakeFlatFileStore3>(factory.Instantiate(string.Empty, "fakeThree://file.txt"));
        }

        [Test]
        public void Instantiate_Extension_MultipleInstanceAreDifferent()
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileStore), new Dictionary<string, string>() { { "extension", "fake" } } },
            };
            config.LoadExtensions(extensions);

            var factory = localServiceLocator.GetFlatFileStoreFactory();
            var engine = factory.Instantiate(string.Empty, "fake://file.txt");
            var engine2 = factory.Instantiate(string.Empty, "fake://file.txt");

            Assert.That(engine, Is.Not.EqualTo(engine2));
        }

        [Test]
        public void Instantiate_ExtensionsAdditionalParameters_ExtensionReturned()
        {

            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                {
                    typeof(FakeFlatFileStore),
                    new Dictionary<string, string>()
                    {
                        { "extension", "fake" },
                        { "multiLines", "true" },
                    }
                },
            };
            config.LoadExtensions(extensions);

            var factory = localServiceLocator.GetFlatFileStoreFactory();
            var engine = factory.Instantiate(string.Empty, "fake");

            Assert.That(engine, Is.Not.Null);
        }

        [Test]
        public void Instantiate_ExtensionsRegisteringSameExtensions_ExceptionThrown()
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileStore), new Dictionary<string, string>() { {  "extension", "fake" } } },
                { typeof(FakeFlatFileStore2), new Dictionary<string, string>() { { "extension", "fake" } } },
                { typeof(FakeFlatFileStore3), new Dictionary<string, string>() { { "extension", "fake" } } },
            };
            config.LoadExtensions(extensions);
            var ex = Assert.Throws<ArgumentException>(() => localServiceLocator.GetFlatFileStoreFactory());
            Console.WriteLine(ex.Message);
        }

        [Test]
        public void Instantiate_ExtensionRegisteringSameExtensions_ExceptionThrown()
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileStore), new Dictionary<string, string>() { {  "extension", "fake" } } },
                { typeof(FakeFlatFileStore2), new Dictionary<string, string>() { { "extension", "fake" } } },
                { typeof(FakeFlatFileStore3), new Dictionary<string, string>() { { "extension", "none" } } },
            };
            config.LoadExtensions(extensions);
            var ex = Assert.Throws<ArgumentException>(() => localServiceLocator.GetFlatFileStoreFactory());
            Console.WriteLine(ex.Message);
        }

        [Test]
        public void Instantiate_InvalidConstructorAvailable_ExceptionThrown()
        {
            var localServiceLocator = new ServiceLocator();
            var config = localServiceLocator.GetConfiguration();
            var extensions = new Dictionary<Type, IDictionary<string, string>>
            {
                { typeof(FakeFlatFileStoreWrong), new Dictionary<string, string>() { {  "extension", "fake" } } },
            };
            config.LoadExtensions(extensions);
            var ex = Assert.Throws<ArgumentException>(() => localServiceLocator.GetFlatFileStoreFactory());
            Console.WriteLine(ex.Message);
        }
    }
}
