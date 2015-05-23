using System;
using System.Xml.Serialization;

namespace DbExporter.Provider.Spife4000
{
    [Serializable]
    public class BasicInfo
    {
        //public int ID { get; set; }
        //public string PatientID { get; set; }  //ADD 2013-1-20
        [XmlIgnore]
        public DateTime ScannedTime { get; set; }
        public string Identifier { get; set; }
        public string Test { get; set; }
        public string Gel_Identifier { get; set; }
        public string Scanned_User { get; set; }
        public string Rescanned_User { get; set; }
        public string Rescanned { get; set; }
        public string Reviewed_User { get; set; }
        public string Reviewed { get; set; }
        public string Edited_User { get; set; }
        public string Edited { get; set; }
        public string Comments { get; set; }
        public string Unused { get; set; }
    }
}
