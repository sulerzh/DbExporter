using DbExporter.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace DbExporter.Provider.QS2000
{
    /// <summary>
    /// Peak表记录类
    /// </summary>
    [Serializable]
    public class Peak
    {
        [XmlIgnore]
        public int Index { get; set; }
        [XmlIgnore]
        public int Left { get; set; }
        [XmlIgnore]
        public int Top { get; set; }
        [XmlIgnore]
        public int Right { get; set; }
        public string Name { get; set; }
        [XmlIgnore]
        public bool MSpike { get; set; }
        public double Percent { get; set; }
    }

    [Serializable]
    public class Qs2000Result
    {
        public int NumPoint { get; set; }
        public string Points { get; set; }
        [XmlArrayItem("Peak", typeof(Peak))]
        [XmlArray("Peaks")]
        public List<Peak> Peaks { get; set; }

        public Qs2000Result()
        { }

        public Qs2000Result(List<short> data, List<short> fraction, List<Fraction> nots)
        {
            Curve c = new Curve(data.Select(i=>(int)i).ToList());
            this.NumPoint = data.Count;
            this.Points = c.ToString();
            this.Peaks = new List<Peak>();
            for (int i=0;i<fraction.Count-1;i++)
            {
                if (fraction[i] < fraction[i + 1])
                {
                    Peak p = new Peak { Index=i+1, Left= fraction[i], Right= fraction[i + 1], Name= nots[i].Label, MSpike=false };
                    this.Peaks.Add(p);
                }
            }
            if (this.Peaks.Count > 0)
            {
                //计算总值[]
                double total = c.GetFractionTotal(0, c.PointCount - 1);
                foreach (var peak in this.Peaks)
                {
                    if (peak.Index == 1)
                    {
                        //albumin[]
                        double currFra = c.GetFractionTotal(peak.Left, peak.Right);
                        peak.Percent = currFra / total;
                    }
                    else
                    {
                        //other(]
                        double currFra = c.GetFractionTotal(peak.Left + 1, peak.Right);
                        peak.Percent = currFra / total;
                    }

                    if (peak.MSpike)
                    {
                        //m-spike()
                        double currSpike = c.GetFractionTotal(peak.Left + 1, peak.Right - 1);
                        peak.Percent = currSpike / total;
                    }
                }
            }
        }
    }

    [Serializable, XmlRoot("Root", Namespace = "http://Qs2000/1.0", IsNullable = false)]
    public class Qs2000State
    {
        [XmlElement("SampleID")]
        public string SampleId { get; set; }

        [XmlElement("DateTime")]
        public string ScannedDate { get; set; }

        public string TestType { get; set; }

        [XmlElement("GelSeqNum")]
        public string SeqNum { get; set; }

        [XmlElement("Sample_Number")]
        public string SampleNum { get; set; }

        [XmlIgnore]
        public byte[] Scan { get; set; }

        public Qs2000Result Result { get; set; }

        public string Base64Bmp
        {
            get
            {
                if (this.Scan != null)
                    return Base64Converter.ImageToBase64(this.Scan);
                return "";
            }
            set
            {
                this.Scan = Base64Converter.Base64ToImage(value);
            }
        }

    }
}
