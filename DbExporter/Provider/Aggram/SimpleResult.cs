using DbExporter.Common;
using System;

namespace DbExporter.Provider.Aggram
{
    public class SimpleResult : ShowBase
    {
        public int CrvSeqNum { get; set; }

        public string PrimId { get; set; }

        public DateTime StartTime { get; set; }

        protected override string GetLabel()
        {
            return string.Format("{0}({1})", PrimId, CrvSeqNum);
        }
    }
}
