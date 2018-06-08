using NBi.Core.Calculation;
using NBi.Xml.Constraints.Comparer;
using NBi.Xml.Items.Calculation.Predication;
using NBi.Xml.Items.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Calculation.Grouping
{
    public class CaseXml
    {
        [XmlElement(Type = typeof(SinglePredicationXml), ElementName = "predicate"),
        XmlElement(Type = typeof(CombinationPredicationXml), ElementName = "combination"),]
        //[XmlIgnore]
        public AbstractPredicationXml Predication { get; set; }
    }
}
