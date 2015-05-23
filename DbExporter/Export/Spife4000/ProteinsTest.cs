using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DbExporter.Provider.Spife4000;

namespace DbExporter.Export.Spife4000
{
    [Serializable]
    public class CurveInfo
    {
        public string Curve { get; set; }
        public string Fraction { get; set; }
        public string Spike { get; set; }
    }

    [Serializable]
    public class MSpike
    {
        [XmlAttribute("Name")]
        public string Label { get; set; }
        [XmlAttribute("Value")]
        public double Value { get; set; }
    }

    [Serializable]
    public class ResultInfo
    {
        public double Albumin { get; set; }
        public double Alpha1 { get; set; }
        public double Alpha2 { get; set; }
        public double Beta { get; set; }
        public double Gamma { get; set; }
        public double AG { get; set; }
        [XmlArrayItem("spike", typeof(MSpike))]
        [XmlArray("m-spike")]
        public List<MSpike> MSpike { get; set; }
    }

    [Serializable]
    public class ProteinsTest
    {
        public BasicInfo BasicInfo { get; set; }
        public CurveInfo Graphic { get; set; }
        public ResultInfo Result { get; set; }
        public string Base64Bmp { get; set; }
    }
}
