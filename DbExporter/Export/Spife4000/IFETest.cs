using DbExporter.Provider.Spife4000;
using System;

namespace DbExporter.Export.Spife4000
{
    [Serializable]
    public class IFETest
    {
        public BasicInfo BasicInfo { get; set; }
        public string Base64Bmp { get; set; }
    }
}
