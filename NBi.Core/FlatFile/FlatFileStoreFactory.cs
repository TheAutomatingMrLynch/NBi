using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Configuration;
using NBi.Extensibility.FlatFile;

namespace NBi.Core.FlatFile
{
    public class FlatFileStoreFactory
    {
        protected IDictionary<string, CtorInvocation> Stores { get; private set; } = new Dictionary<string, CtorInvocation>();
        protected delegate object CtorInvocation();

        public IFlatFileStore Instantiate(string basePath, string path)
        {
            var fileScheme = (path?.Contains("://") ?? false) ? path.Split(new[] { "://" }, StringSplitOptions.RemoveEmptyEntries)[0] : string.Empty;

            if (string.IsNullOrEmpty(fileScheme))
                return new LocalDiskFileStore(basePath, path);

            if (Stores.ContainsKey(fileScheme))
                return Instantiate(Stores[fileScheme]);
            else if (Stores.ContainsKey("*.*"))
                return Instantiate(Stores["*.*"]);
            throw new ArgumentException();
        }

        private IFlatFileStore Instantiate(CtorInvocation ctorInvocation) => (IFlatFileStore)ctorInvocation.Invoke();

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
                var ctor = type.GetConstructor(Array.Empty<Type>());

                if (ctor == null)
                    throw new ArgumentException($"Can't load an extension. Can't find a constructor without parameter for the type '{type.Name}'");
                object ctorInvocation() => ctor.Invoke(Array.Empty<object>());

                if (Stores.ContainsKey(extension))
                {
                    var otherTypes = providers.Where(x => (x.Value.ContainsKey("extension") ? x.Value["extension"] : "*.*") == extension && x.Key != provider.Key).Select(x => x.Key.Name);
                    var sentence = otherTypes.Count() > 1
                        ? $"the other types '{string.Join("', '", otherTypes.Take(otherTypes.Count() - 1))}' and '{otherTypes.ElementAt(otherTypes.Count() - 1)}' are"
                        : $"another type '{otherTypes.ElementAt(0)}' is";
                    throw new ArgumentException($"Can't register an extension. The type '{type.Name}' is trying to register a store for the file scheme '{extension}' but {sentence} also trying to register another store for the same file scheme.");
                }

                Stores.Add(extension, ctorInvocation);
            }
        }
    }
}
