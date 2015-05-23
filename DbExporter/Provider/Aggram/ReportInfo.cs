using System;
using System.Xml.Serialization;

namespace DbExporter.Provider.Aggram
{
    [Serializable]
    public class ReportInfo
    {
        [XmlIgnore]
        public string PrimID { get; set; }

        [XmlIgnore]
        public int ChnlNum { get; set; }

        [XmlIgnore]
        public DateTime StartTime { get; set; }

        public string ProcID { get; set; }

        public string Abbrev { get; set; }

        public double Conc { get; set; }

        public string Unit { get; set; }

        public double PRP { get; set; }

        public double PPP { get; set; }

        public double MaxPercent { get; set; }

        public double MaxPCTime { get; set; }

        public double LagTime { get; set; }

        public double Slope { get; set; }

    }
}
