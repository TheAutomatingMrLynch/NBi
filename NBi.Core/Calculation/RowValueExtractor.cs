using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    public class RowValueExtractor
    {
        protected readonly IEnumerable<IColumnExpression> expressions;
        protected readonly IEnumerable<IColumnAlias> aliases;

        public RowValueExtractor(IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
        {
            this.aliases = aliases;
            this.expressions = expressions;
        }

        public object Execute(IColumnIdentifier identifier, ref DataRow row)
        {
            if (identifier is ColumnPositionIdentifier)
            {
                var ordinal = (identifier as ColumnPositionIdentifier).Position;
                if (ordinal <= row.Table.Columns.Count)
                    return row.ItemArray[ordinal];
                else
                    throw new ArgumentException($"The variable of the predicate is identified as '{identifier.Label}' but the column in position '{ordinal}' doesn't exist. The dataset only contains {row.Table.Columns.Count} columns.");
            }

            var name = (identifier as ColumnNameIdentifier).Name;
            var alias = aliases?.SingleOrDefault(x => x.Name == name);
            if (alias != null)
                return row.ItemArray[alias.Column];

            var expression = expressions?.SingleOrDefault(x => x.Name == name);
            if (expression != null)
            {
                var result = EvaluateExpression(expression, row);
                var expColumnName = $"exp::{name}";
                if (!row.Table.Columns.Contains(expColumnName))
                {
                    var newColumn = new DataColumn(expColumnName, typeof(object));
                    row.Table.Columns.Add(newColumn);
                }

                row[expColumnName] = result;
                return result;
            }

            var column = row.Table.Columns.Cast<DataColumn>().SingleOrDefault(x => x.ColumnName == name);
            if (column != null)
                return row[column.ColumnName];

            throw new ArgumentException($"The value '{name}' is not recognized as a column name or a column position or a column alias or an expression.");
        }

        protected object EvaluateExpression(IColumnExpression expression, DataRow row)
        {
            var exp = new NCalc.Expression(expression.Value);
            var factory = new ColumnIdentifierFactory();

            exp.EvaluateParameter += delegate (string name, NCalc.ParameterArgs args)
            {
                args.Result = Execute(factory.Instantiate(name), ref row);
            };

            return exp.Evaluate();
        }
    }
}
