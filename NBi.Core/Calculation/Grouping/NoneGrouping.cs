using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;

namespace NBi.Core.Calculation.Grouping
{
    sealed class NoneGrouping : IGroupBy
    {
        public IDictionary<object, DataTable> Execute(ResultSet.ResultSet resultSet)
        {
            return new Dictionary<object, DataTable>()
            {
                { new object(), resultSet.Table }
            };
        }
    }
}
