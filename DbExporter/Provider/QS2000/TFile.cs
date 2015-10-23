using DbExporter.Common;
using System;
using System.Collections.Generic;

namespace DbExporter.Provider.QS2000
{
    // t**.fsd
    // 576
    public class TFileHeader
    {
        public TestIdentity Identity; // 36
        // skip 220个字节
        public byte[] Count; // (100) 12 or 1
    }

    public class TFileData : ShowBase
    {
        public int SeqNum;
        public PatientInfo Patient;
        public string csStatus; // (21)
        public string csDate; // (20)

        public string PatientId
        {
            get
            {
                if (this.Patient == null || this.Patient.Demographic.Count == 0)
                {
                    return "";
                }
                return this.Patient.Demographic[0];
            }
        }
        public DateTime ScanDate {
            get { return QS2000Provider.ParseDateTime(this.csDate); }
        }
        public byte Id;
        public string Name; // SPE-18 OR IFE(21)
        public string Unknown; // (3)

        protected override string GetLabel()
        {
            return string.Format("{0} ({1} - {2}，{3})", this.PatientId, this.SeqNum, this.Id, this.Name);
        }
    }

    public class TFile
    {
        public TFileHeader Header;
        public List<TFileData> Datas;
    }
}
