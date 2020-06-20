using NBi.Core.Injection;
using NBi.Extensibility;
using NBi.Extensibility.FlatFile;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    class FlatFileResultSetResolver : IResultSetResolver
    {
        private readonly FlatFileResultSetResolverArgs args;
        private readonly ServiceLocator serviceLocator;

        public FlatFileResultSetResolver(FlatFileResultSetResolverArgs args, ServiceLocator serviceLocator)
        {
            this.args = args;
            this.serviceLocator = serviceLocator;
        }

        public virtual IResultSet Execute()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var providerFactory = serviceLocator.GetFlatFileStoreFactory();
            using (var provider = providerFactory.Instantiate(args.BasePath, args.Path.Execute()))
            {
                var readerFactory = serviceLocator.GetFlatFileReaderFactory();
                var reader = readerFactory.Instantiate(args.ParserName, args.Profile);
                var rs = Execute(provider, reader);
                stopWatch.Stop();
                Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Time needed to load data from flat file: {stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}");
                Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Result-set contains {rs.Rows.Count} row{(rs.Rows.Count > 1 ? "s" : string.Empty)} and {rs.Columns.Count} column{(rs.Columns.Count > 1 ? "s" : string.Empty)}");
                return rs;
            }
        }

        protected internal virtual IResultSet Execute(IFlatFileStore provider, IFlatFileReader reader)
        {
            if (!provider.Exists())
            {
                if (args.Redirection == null)
                    throw new ExternalDependencyNotFoundException(provider.FullPath);
                else
                    return args.Redirection.Execute();
            }

            var stream = provider.GetStream();
            var dataTable = reader.ToDataTable(stream);
            var rs = new ResultSet();
            rs.Load(dataTable);

            return rs;
        }
    }
}
