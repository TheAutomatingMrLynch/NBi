using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Generation
{
    abstract class SqlGenerator
    {
        private ITargetFormatter TargetFormatter;
        private IFieldFormatter FieldFormatter;
        
        protected SqlGenerator(ITargetFormatter targetFormatter, IFieldFormatter fieldFormatter)
        {
            TargetFormatter = targetFormatter;
            FieldFormatter = fieldFormatter;
        }

        public string Execute(SqlTarget target)
        {
            var fields = GetFields();
            return $"select {(string.Join(", ", FormatFields(fields)))} from {FormatTarget(target.SchemaName, target.TableName)};";
        }

        private IEnumerable<string> GetFields() => Enumerable.Empty<string>();

        protected virtual IEnumerable<string> FormatFields(IEnumerable<string> fields)
        {
            if (fields.Count() == 0)
            {
                yield return "*";
                yield break;
            }
            
            foreach (var field in fields)
                yield return FieldFormatter.Execute(field);
        }

        protected virtual string FormatTarget(string schema, string table) => TargetFormatter.Execute(schema, table);
    }
}
