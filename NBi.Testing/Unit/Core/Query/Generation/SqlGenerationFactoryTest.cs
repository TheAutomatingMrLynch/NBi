using NBi.Core.Query.Generation;
using NBi.Core.Query.Generation.Ansi;
using NBi.Core.Query.Generation.MsSql;
using NBi.Core.Query.Generation.MySql;
using NBi.Core.Query.Generation.Oracle;
using NBi.Core.Query.Generation.Postgresql;
using NBi.Core.Query.Generation.Teradata;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Query.Generation
{
    public class SqlGenerationFactoryTest
    {
        [TestCase(SqlDialect.Ansi, typeof(AnsiGenerator))]
        [TestCase(SqlDialect.MsSql, typeof(MsSqlGenerator))]
        [TestCase(SqlDialect.MySql, typeof(MySqlGenerator))]
        [TestCase(SqlDialect.Postgresql, typeof(PostgresqlGenerator))]
        [TestCase(SqlDialect.Oracle, typeof(OracleGenerator))]
        [TestCase(SqlDialect.Teradata, typeof(TeradataGenerator))]
        public void Instantiate(SqlDialect dialect, Type type)
        {
            var factory = new SqlGeneratorFactory();
            var generator = factory.Instantiate(dialect);
            Assert.IsInstanceOf(type, generator);
        }
    }
}
