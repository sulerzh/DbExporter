using DbExporter.Common;
using System;

namespace DbExporter.Provider.Spife4000
{
    public class TdfInfo : ShowBase, IComparable<TdfInfo>
    {
        public string GelId { get; set; }
        public string SampleNum { get; set; }
        public DateTime ScannedTime { get; set; }
        public string SampleId { get; set; }
        public string BdfFilePath { get; set; }

        public int CompareTo(TdfInfo other)
        {
            int result = ScannedTime.CompareTo(other.ScannedTime);
            if (result != 0)
                return result;
            result = GelId.CompareTo(other.GelId);
            if (result != 0)
                return result;
            return Int32.Parse(SampleNum).CompareTo(Int32.Parse(other.SampleNum));
        }

        protected override string GetLabel()
        {
            string s = string.IsNullOrEmpty(SampleId) ? "" : SampleId;
            return string.Format("{0} {1,2} : {2}", GelId, SampleNum, s);
        }
    }
}
