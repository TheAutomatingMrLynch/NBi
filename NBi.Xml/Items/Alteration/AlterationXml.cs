﻿using NBi.Xml.Items.Alteration.Conversion;
using NBi.Xml.Items.Alteration.Extension;
using NBi.Xml.Items.Alteration.Renaming;
using NBi.Xml.Items.Alteration.Transform;
using NBi.Xml.Items.Calculation;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Items.Sequence.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration
{
    public class AlterationXml
    {
        [XmlElement("rename")]
        public List<RenamingXml> Renamings { get; set; }
        [XmlElement("extend")]
        public List<ExtendXml> Extensions { get; set; }
        [XmlElement("filter")]
        public List<FilterXml> Filters { get; set; }
        [XmlElement("convert")]
        public List<ConvertXml> Conversions { get; set; }
        [XmlElement("transform")]
        public List<TransformXml> Transformations { get; set; }
        [XmlElement("summarize")]
        public List<SummarizeXml> Summarizations { get; set; }

        public AlterationXml()
        {
            Renamings = new List<RenamingXml>();
            Extensions = new List<ExtendXml>();
            Filters = new List<FilterXml>();
            Conversions = new List<ConvertXml>();
            Transformations = new List<TransformXml>();
            Summarizations = new List<SummarizeXml>();
        }
    }
}
