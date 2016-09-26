using DbExporter.Common;
using DbExporter.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace DbExporter.Provider.Platinum
{
    /// <summary>
    /// Scan表记录类
    /// </summary>
    [Serializable]
    public class ScanResult : ShowBase
    {
        public int ScanIdNr { get; set; }
        [XmlIgnore]
        public int DemographicIdNr { get; set; }

        // Lis标识
        public string LisLabel
        {
            get { return string.Format("{0}{1}", Patient.Def.GetLisLabel(), IFE ? "（IFE)" : ""); }
        }
        public string GelName { get; set; }
        public string GelType { get; set; }
        public bool IFE { get; set; }
        [XmlIgnore]
        public bool Edited { get; set; }
        [XmlIgnore]
        public bool Viewed { get; set; }
        [XmlIgnore]
        public DateTime MeasTime { get; set; }
        [XmlElement("MeasTime")]
        public string MeasTimeString
        {
            get { return this.MeasTime.ToString("yyyy/MM/dd HH:mm:ss"); }
            set { this.MeasTime = DateTime.Parse(value); }
        }

        [XmlIgnore]
        public int NumData { get; set; }
        [XmlIgnore]
        public byte[] Scan { get; set; }
        [XmlIgnore]
        public byte[] DIB { get; set; }
        [XmlIgnore]
        public DateTime CreateDate { get; set; }
        [XmlElement("CreateDate")]
        public string CreateDateString
        {
            get { return this.CreateDate.ToString("yyyy/MM/dd HH:mm:ss"); }
            set { this.CreateDate = DateTime.Parse(value); }
        }
        [XmlIgnore]
        public DateTime UpdateDate { get; set; }
        //[XmlElement("UpdateDate")]
        //public string UpdateDateString 
        //{
        //    get { return this.UpdateDate.ToString("yyyy/MM/dd HH:mm:ss"); }
        //    set { this.UpdateDate = DateTime.Parse(value);}
        //}
        [XmlIgnore]
        public string Operator { get; set; }

        public Demographic Patient { get; set; }

        protected override string GetLabel()
        {
            return string.Format("{0}{1}", Patient.Def.GetLisLabel(), IFE ? "（IFE)" : "");
        }
    }

    public enum FileTypeEnum : byte
    {
        String = 0,//串
        UniqueString = 1,// 唯一串
        DateTime = 2,//日期
        LisLabel = 3,// lis标识
        ListLabelUS = 4//lis标识/unicode string
    }

    [Serializable]
    public class FieldDef
    {
        [XmlAttribute("FieldType")]
        public byte FieldType { get; set; }
        [XmlAttribute("Field")]
        public string Field { get; set; }
        [XmlAttribute("Value")]
        public string Value { get; set; }
    }

    /// <summary>
    /// DemoDef表记录类
    /// </summary>
    [Serializable]
    public class DemoDef
    {
        private List<FieldDef> _fields = new List<FieldDef>();

        [XmlAttribute("NumberOfFields")]
        public int NumberOfFields { get; set; }

        [XmlArrayItem("Field", typeof(FieldDef))]
        [XmlArray("Fields")]
        public List<FieldDef> Fields { get { return _fields; } }

        public void AddField(FieldDef def)
        {
            _fields.Add(def);
        }

        // Lis 标签
        public string GetLisLabel()
        {
            foreach (var fieldDef in Fields)
            {
                if (fieldDef.FieldType == (byte)FileTypeEnum.LisLabel)
                {
                    return fieldDef.Value;
                }
            }
            return "无'Lis标识'字段";
        }
    }

    /// <summary>
    /// Demographic表记录类
    /// </summary>
    [Serializable]
    public class Demographic
    {
        public DemoDef Def { get; set; }

        public void SetValue(int i, string value)
        {
            if (this.Def != null)
            {
                this.Def.Fields[i].Value = value;
            }
        }
    }

    /// <summary>
    /// Peak表记录类
    /// </summary>
    [Serializable]
    public class Peak
    {
        public int Index { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public string Name { get; set; }
        public bool MSpike { get; set; }
        public double Percent { get; set; }
    }

    /// <summary>
    /// 用于计算peak的值，以及将曲线点进行序列化
    /// </summary>
    [Serializable]
    public class CalResult
    {
        private int _scanId;
        private int _numData;
        private byte[] _scan;

        public double AG { get; set; }
        public int NumPoint
        {
            get { return _numData; }
            set { _numData = value; }
        }
        
        public string Points { get; set; }
        [XmlArrayItem("Peak", typeof(Peak))]
        [XmlArray("Peaks")]
        public List<Peak> Peaks { get; set; }

        public CalResult()
        {
        }

        public CalResult(int scanId, int numData, byte[] scan)
        {
            this._scanId = scanId;
            this._numData = numData;
            this._scan = scan;
            Initialize();
        }

        private void Initialize()
        {
            RawCurve c = GetCurveFromBlob(_scan, _numData);
            this.Points = c.ToString();
            this.Peaks = PlatinumDbAccess.GetPeaksByScanId(_scanId);
            if (this.Peaks != null && this.Peaks.Count > 0)
            {
                BaseLineCorrectedCurve correctedCurve = new BaseLineCorrectedCurve { Raw = c };
                correctedCurve.SetFraction(0, 0, GlobalConfigVars.BaseLinePercent[0]);
                for (int i = 1; i <= this.Peaks.Count; i++)
                {
                    correctedCurve.SetFraction(i, this.Peaks[i - 1].Right, GlobalConfigVars.BaseLinePercent[i]);
                }

                //计算总值[]
                double total = correctedCurve.GetFractionTotal();
                double albumin = 0;
                double others = 0;
                foreach (var peak in this.Peaks)
                {
                    if (peak.Index == 1)
                    {
                        //albumin[]
                        double currFra = correctedCurve.GetFractionTotal(peak.Left, peak.Right);
                        peak.Percent = currFra / total;
                        albumin = currFra;
                    }
                    else
                    {
                        //other(]
                        double currFra = correctedCurve.GetFractionTotal(peak.Left + 1, peak.Right);
                        peak.Percent = currFra / total;
                        others += currFra;
                    }

                    if (peak.MSpike)
                    {
                        //m-spike()
                        double currSpike = correctedCurve.GetFractionTotal(peak.Left + 1, peak.Right - 1);
                        peak.Percent = currSpike / total;
                    }
                }

                this.AG = albumin / others;
            }
        }

        private static RawCurve GetCurveFromBlob(byte[] buffer, int numData)
        {
            RawCurve c = new RawCurve();
            try
            {
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (BinaryReader br = new BinaryReader(ms))
                    {
                        for (int i = 0; i < numData; i++)
                        {
                            int pointValue = br.ReadInt32();
                            c.AddPoint(pointValue);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return c;
        }
    }

    [Serializable, XmlRoot("Root", Namespace = "http://spife4000DbExporter/1.0", IsNullable = false)]
    public class PlatinumState
    {
        public ScanResult Scan { get; set; }
        public CalResult Result { get; set; }
        public string Base64JFIF
        {
            get
            {
                if (this.Scan != null && this.Scan.DIB != null)
                    return Base64Converter.ImageToBase64(this.Scan.DIB);
                return "";
            }
            set
            {
                this.Scan.DIB = Base64Converter.Base64ToImage(value);
            }
        }

        public PlatinumState()
        {
        }

        public PlatinumState(ScanResult scan)
        {
            this.Scan = scan;

            if (!this.Scan.IFE)
            {
                this.Result = new CalResult(this.Scan.ScanIdNr, this.Scan.NumData, this.Scan.Scan);
            }
        }

    }
}
