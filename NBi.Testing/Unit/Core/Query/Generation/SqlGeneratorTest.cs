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
    public class SqlGeneratorTest
    {
        [TestCase(typeof(AnsiGenerator))]
        public void Execute_NameWithSpace_Exception(Type type)
        {
            var generator = type.GetConstructor(new Type[] { }).Invoke(new object[] { }) as SqlGenerator;
            Assert.Throws<ArgumentException>(() => generator.Execute(new SqlTarget("dbo", "My Table")));
        }

        [TestCase(typeof(MsSqlGenerator))]
        public void Execute_NameWithSpace_Brackets(Type type)
        {
            var generator = type.GetConstructor(new Type[] { }).Invoke(new object[] { }) as SqlGenerator;
            var sentence = generator.Execute(new SqlTarget("dbo", "My Table"));
            Assert.That(sentence, Is.EqualTo("select * from [dbo].[My Table];"));
        }

        [TestCase(typeof(MySqlGenerator))]
        public void Execute_NameWithSpace_BackTick(Type type)
        {
            var generator = type.GetConstructor(new Type[] { }).Invoke(new object[] { }) as SqlGenerator;
            var sentence = generator.Execute(new SqlTarget("dbo", "My Table"));
            Assert.That(sentence, Is.EqualTo("select * from `dbo`.`My Table`;"));
        }

        [TestCase(typeof(PostgresqlGenerator))]
        [TestCase(typeof(OracleGenerator))]
        [TestCase(typeof(TeradataGenerator))]
        public void Execute_NameWithSpace_DoubleQuotes(Type type)
        {
            var generator = type.GetConstructor(new Type[] { }).Invoke(new object[] { }) as SqlGenerator;
            var sentence = generator.Execute(new SqlTarget("dbo", "My Table"));
            Assert.That(sentence, Is.EqualTo("select * from \"dbo\".\"My Table\";"));
        }
    }
}
