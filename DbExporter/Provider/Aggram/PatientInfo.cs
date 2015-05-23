using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbExporter.Provider.Aggram
{
    [Serializable]
    public class PatientInfo
    {
        public string PrimID { get; set; }
        public string Name { get; set; }
        public string Birthdate { get; set; }
        public string Sex { get; set; }
        public string Physician { get; set; }
        public string Diagnosis { get; set; }
    }
}


