using DbExporter.Provider.Aggram;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DbExporter.Export.Aggram
{
    [Serializable]
    public class Curve
    {
        /// <summary>
        /// 曲线标签只
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 初始最大值
        /// </summary>
        public int InitialMax { get; set; }

        /// <summary>
        /// 初始最小值
        /// </summary>
        public int InitialMin { get; set; }

        /// <summary>
        /// OD0点点值
        /// </summary>
        public int ZeroODPoint { get; set; }

        /// <summary>
        /// 最大PC点值
        /// </summary>
        public int MaxPCPoint { get; set; }

        /// <summary>
        /// 点单位，大约为1.64e-9
        /// </summary>
        public double PointUnit { get; set; }

        /// <summary>
        /// 点数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 点数组
        /// </summary>
        [XmlArrayItem("Point", typeof(CurvePoint))]
        [XmlArray("CurvePoints")]
        public List<CurvePoint> CurvePoints { get; set; }
    }

    [Serializable, XmlRoot("Root", Namespace = "http://AggRamDbExporter/1.0", IsNullable = false)]
    public class AggRamDbState
    {
        [XmlElement("WrkLstNum")]
        public int WrkLstNum { get; set; }

        [XmlElement("ChnlNum")]
        public short ChnlNum { get; set; }

        [XmlElement("StartTime")]
        public string StartTime { get; set; }

        [XmlElement("CrvSeqNum")]
        public int CrvSeqNum { get; set; }
        
        public PatientInfo Patient { get; set; }

        public ReportInfo Report { get; set; }

        public Curve Curve { get; set; }



        public string SerializeToXMLString()
        {
            StringBuilder output = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(output);
            XmlSerializer serializer = new XmlSerializer(typeof(AggRamDbState));
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
