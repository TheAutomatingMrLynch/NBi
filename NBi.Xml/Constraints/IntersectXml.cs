using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Xml.Items;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Settings;
using NBi.Xml.Items.Hierarchical.Xml;
using NBi.Core.ResultSet.Equivalence;
using NBi.Core.Query.Client;
using NBi.Xml.Systems;

namespace NBi.Xml.Constraints
{
    public class IntersectXml : AbstractConstraintXml
    {
        private readonly bool parallelizeQueries = false;
        public bool ParallelizeQueries => parallelizeQueries || Settings.ParallelizeQueries;

        public IntersectXml() { }

        internal IntersectXml(bool parallelizeQueries) => this.parallelizeQueries = parallelizeQueries;

        internal IntersectXml(SettingsXml settings) => Settings = settings;

        [XmlIgnore()]
        public override DefaultXml Default
        {
            get => base.Default;
            set => base.Default = value;
        }

        [XmlElement("result-set")]
        public virtual ResultSetSystemXml ResultSet { get; set; }

        [XmlElement("column")]
        public List<ColumnDefinitionXml> columnsDef;

        public IReadOnlyList<IColumnDefinition> ColumnsDef
        {
            get => (columnsDef ?? new List<ColumnDefinitionXml>()).Cast<IColumnDefinition>().ToList() ;
        }
    }
}
