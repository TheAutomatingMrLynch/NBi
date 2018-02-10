using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Generation
{
    class SqlTarget : ITarget
    {
        public string SchemaName { get; }
        public string TableName { get; }

        public SqlTarget(string schemaName, string tableName)
        {
            SchemaName = schemaName;
            TableName = tableName;
        }
    }
}
