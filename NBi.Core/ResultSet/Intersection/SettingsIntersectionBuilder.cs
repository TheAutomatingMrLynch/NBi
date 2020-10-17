using NBi.Core.Scalar.Comparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Intersection
{
    public class SettingsIntersectionBuilder : SettingsResultSetBuilder
    {
        protected override void OnBuild()
        {
            BuildSettings(ColumnType.Text, ColumnType.Numeric, null);
        }

        protected override void OnCheck()
        {
            PerformInconsistencyChecks();
            PerformSetsAndColumnsCheck(
                SettingsOrdinalResultSet.KeysChoice.All
                , SettingsOrdinalResultSet.ValuesChoice.None);
            PerformDuplicationChecks();
        }
    }
}
