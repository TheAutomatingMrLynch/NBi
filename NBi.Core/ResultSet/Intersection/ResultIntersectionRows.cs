using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Intersection
{
    public class ResultIntersectionRows
    {
        public IEnumerable<DataRow> Rows { get; private set; }

        public ResultIntersectionRows(IEnumerable<KeyCollection> values)
        {
            if (values.Count() > 0)
            {
                var dt = new DataTable();
                int i = 0;
                foreach (var key in values.First().Members)
                {
                    dt.Columns.Add(new DataColumn($"#{i}"));
                    i++;
                }
                foreach (var value in values)
                {
                    var dr = dt.NewRow();
                    i = 0;
                    foreach (var key in value.Members)
                    {
                        dr.SetField($"#{i}", key);
                        i++;
                    }
                    dt.Rows.Add(dr);
                }
                Rows = dt.Rows.Cast<DataRow>();
            }
            else
                Rows = Enumerable.Empty<DataRow>();
        }

        public static EmptyIntersectionRows Empty { get; } = new EmptyIntersectionRows();

        public class EmptyIntersectionRows : ResultIntersectionRows
        {
            internal EmptyIntersectionRows() : base(Enumerable.Empty<KeyCollection>()) { }
        }

    }

    




}
