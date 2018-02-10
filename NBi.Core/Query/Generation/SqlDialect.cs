using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Core.Query.Generation
{
    public enum SqlDialect
    {
        [XmlEnum("ansi")] Ansi,
        [XmlEnum("mssql")] MsSql,
        [XmlEnum("mysql")] MySql,
        [XmlEnum("postgresql")] Postgresql,
        [XmlEnum("oracle")] Oracle,
        [XmlEnum("teradata")] Teradata,
    }
}
