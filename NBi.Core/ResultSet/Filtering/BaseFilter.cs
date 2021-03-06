﻿using NBi.Extensibility;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering
{
    public abstract class BaseFilter : IResultSetFilter
    {
        protected Context Context { get; }
        protected BaseFilter(Context context)
        => Context = context;

        public IResultSet AntiApply(IResultSet rs) => Apply(rs, (x => !x));

        public IResultSet Apply(IResultSet rs) => Apply(rs, (x => x));

        protected IResultSet Apply(IResultSet rs, Func<bool, bool> onApply)
        {
            var filteredRs = new ResultSet();
            var table = rs.Table.Clone();
            filteredRs.Load(table);
            filteredRs.Table.Clear();

            foreach (DataRow row in rs.Rows)
            {
                Context.Switch(row);
                if (onApply(RowApply(Context)))
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

        protected abstract bool RowApply(Context context);

        public abstract string Describe();
    }
}