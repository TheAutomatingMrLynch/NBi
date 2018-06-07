using NBi.Core.Calculation.Predicate;
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
    public abstract class BaseFilter : IResultSetFilter
    {
        protected BaseFilter()
        {}

        public ResultSet.ResultSet AntiApply(ResultSet.ResultSet rs)
        {
            return Apply(rs, (x => !x));
        }

        public ResultSet.ResultSet Apply(ResultSet.ResultSet rs)
        {
            return Apply(rs, (x => x));
        }

        protected ResultSet.ResultSet Apply(ResultSet.ResultSet rs, Func<bool,bool> onApply)
        {
            var filteredRs = new ResultSet.ResultSet();
            var table = rs.Table.Clone();
            filteredRs.Load(table);
            filteredRs.Table.Clear();
            
            foreach (DataRow row in rs.Rows)
            {
                if (onApply(RowApply(row)))
                {
                    if (filteredRs.Rows.Count == 0 && filteredRs.Columns.Count != row.Table.Columns.Count)
                    {
                        foreach (DataColumn column in row.Table.Columns)
                        {
                            if (!filteredRs.Columns.Cast<DataColumn>().Any(x => x.ColumnName == column.ColumnName))
                                filteredRs.Columns.Add(column.ColumnName, typeof(object));
                        }
                    }
                    filteredRs.Table.ImportRow(row);
                }
            }

            filteredRs.Table.AcceptChanges();
            return filteredRs;
        }

        protected abstract bool RowApply(DataRow row);

        public abstract string Describe();
    }
}
