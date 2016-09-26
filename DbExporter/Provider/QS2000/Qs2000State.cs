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
        public int Index { get; set; }
        public int Left { get; set; }
        [XmlIgnore]
        public int Top { get; set; }
        public int Right { get; set; }
        public string Name { get; set; }
        [XmlIgnore]
        public bool MSpike { get; set; }
        public double Percent { get; set; }
    }

    [Serializable]
    public class Qs2000Result
    {
        public double AG { get; set; }
        public int NumPoint { get; set; }
        public string Points { get; set; }
        
        [XmlArrayItem("Peak", typeof(Peak))]
        [XmlArray("Peaks")]
        public List<Peak> Peaks { get; set; }

        [XmlIgnore]
        public List<double> cache = new List<double>();


        public Qs2000Result()
        { }

        public Qs2000Result(List<short> data, List<short> fraction, List<Fraction> nots)
        {
            RawCurve c = new RawCurve(data.Select(i => (int)i).ToList());
            BaseLineCorrectedCurve correctedCurve = new BaseLineCorrectedCurve { Raw = c};
            this.NumPoint = data.Count;
            this.Points = c.ToString();
            this.Peaks = new List<Peak>();
            int fracCount = fraction[0];
            correctedCurve.SetFraction(0, 0, GlobalConfigVars.BaseLinePercent[0]);
            for (int i = 1; i <= fracCount; i++)
            {
                Peak p = new Peak
                {
                    Index = i,
                    Left = i == 1 ? 0 : fraction[i - 1],
                    Right = fraction[i],
                    Name = nots[i].Label,
                    MSpike = false
                };
                int percent = i > 5 ? 0 : GlobalConfigVars.BaseLinePercent[i];
                correctedCurve.SetFraction(i, p.Right, percent);
                this.Peaks.Add(p);
            }
            if (this.Peaks.Count > 0)
            {
                //计算总值[]
                double total = correctedCurve.GetFractionTotal();
                double albumin = 0;
                double others = 0;
                foreach (var peak in this.Peaks)
                {
                    int start = peak.Left;
                    int end = peak.Right;
                    if (peak.Index == 1)
                    {
                        //albumin[]
                        double currFra = correctedCurve.GetFractionTotal(start, end);
                        peak.Percent = currFra / total;
                        albumin = currFra;
                    }
                    else
                    {
                        //other(]
                        double currFra = correctedCurve.GetFractionTotal(start + 1, end);
                        peak.Percent = currFra / total;
                        others += currFra;
                    }

                    if (peak.MSpike)
                    {
                        //m-spike()
                        double currSpike = correctedCurve.GetFractionTotal(start + 1, end - 1);
                        peak.Percent = currSpike / total;
                    }
                }

                this.AG = albumin / others;
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
