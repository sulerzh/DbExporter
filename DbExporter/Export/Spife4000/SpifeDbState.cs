using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DbExporter.Export.Spife4000
{
    [Serializable, XmlRoot("Root", Namespace = "http://spife4000DbExporter/1.0", IsNullable = false)]
    public class SpifeDbState
    {
        [XmlElement("Gel_Identifier")]
        public string GelId { get; set; }

        [XmlElement("Sample_Number")]
        public string SampleNum { get; set; }

        [XmlElement("DateTime")]
        public string ScannedDate { get; set; }

        [XmlElement("SampleID")]
        public string SampleId { get; set; }

        public ProteinsTest ProteinsTest { get; set; }

        public IFETest IFETest { get; set; }

        public string SerializeToXMLString()
        {
            StringBuilder output = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(output);
            XmlSerializer serializer = new XmlSerializer(typeof(SpifeDbState));
            try
            {
                serializer.Serialize(xmlWriter, this);
            }
            finally
            {
                xmlWriter.Close();
            }
            return output.ToString();
        }
    }
}
