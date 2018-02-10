using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Generation
{
    class SqlGeneratorFactory
    {
        public SqlGenerator Instantiate(SqlDialect sqldialect)
        {
            switch (sqldialect)
            {
                case SqlDialect.Ansi: return new Ansi.AnsiGenerator();
                case SqlDialect.MsSql: return new MsSql.MsSqlGenerator();
                case SqlDialect.MySql: return new MySql.MySqlGenerator();
                case SqlDialect.Postgresql: return new Postgresql.PostgresqlGenerator();
                case SqlDialect.Oracle: return new Oracle.OracleGenerator();
                case SqlDialect.Teradata: return new Teradata.TeradataGenerator();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
