using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Configuration;
using NBi.Extensibility.FlatFile;

namespace NBi.Core.FlatFile.Storing
{
    public class FlatFileStoreFactory
    {
        protected IDictionary<string, ConstructorInfo> Stores { get; private set; } = new Dictionary<string, ConstructorInfo>();

        public IFlatFileStore Instantiate(string basePath, string path)
        {
            var fileScheme = path?.Contains("://") ?? false ? path.Split(new[] { "://" }, StringSplitOptions.RemoveEmptyEntries)[0] : string.Empty;

            if (string.IsNullOrEmpty(fileScheme))
                return new LocalDiskFileStore(Path.IsPathRooted(path) ? path : basePath + path);

            if (Stores.ContainsKey(fileScheme))
                return Instantiate(Stores[fileScheme], path);
            throw new ArgumentException();
        }

        private IFlatFileStore Instantiate(ConstructorInfo ctor, string uri) => (IFlatFileStore)(ctor.Invoke(new object[] { uri }));

        public FlatFileStoreFactory(IExtensionsConfiguration config)
        {
            var extensions = config?.Extensions?.Where(x => typeof(IFlatFileStore).IsAssignableFrom(x.Key) && !x.Key.IsAbstract) ?? new List<KeyValuePair<Type, IDictionary<string, string>>>();
            RegisterExtensions(extensions.ToArray());
        }

        protected internal void RegisterExtensions(IEnumerable<KeyValuePair<Type, IDictionary<string, string>>> providers)
        {
            foreach (var provider in providers)
            {
                var type = provider.Key;
                var parameters = provider.Value;

                var extension = parameters.ContainsKey("extension") ? parameters["extension"] : "*.*";
                var ctor = type.GetConstructor(new [] { typeof(string) });

                if (ctor == null)
                    throw new ArgumentException($"Can't load an extension. Can't find a constructor with a single parameter of type string for the type '{type.Name}'");

                if (Stores.ContainsKey(extension))
                {
                    var otherTypes = providers.Where(x => (x.Value.ContainsKey("extension") ? x.Value["extension"] : "*.*") == extension && x.Key != provider.Key).Select(x => x.Key.Name);
                    var sentence = otherTypes.Count() > 1
                        ? $"the other types '{string.Join("', '", otherTypes.Take(otherTypes.Count() - 1))}' and '{otherTypes.ElementAt(otherTypes.Count() - 1)}' are"
                        : $"another type '{otherTypes.ElementAt(0)}' is";
                    throw new ArgumentException($"Can't register an extension. The type '{type.Name}' is trying to register a store for the file scheme '{extension}' but {sentence} also trying to register another store for the same file scheme.");
                }

                Stores.Add(extension, ctor);
            }

            if (!Stores.ContainsKey("http"))
                Stores.Add("http", typeof(WebFileStore).GetConstructor(new[] { typeof(string) }));
            if (!Stores.ContainsKey("https"))
                Stores.Add("https", typeof(WebFileStore).GetConstructor(new[] { typeof(string) }));
            if (!Stores.ContainsKey("file"))
                Stores.Add("file", typeof(LocalDiskFileStore).GetConstructor(new[] { typeof(string) }));

        }
    }
}
