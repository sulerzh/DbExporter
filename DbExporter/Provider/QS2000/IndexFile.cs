using System.Collections.Generic;

namespace DbExporter.Provider.QS2000
{
    public class IndexInfo
    {
        public int SeqNum;
        public int Unknown;
        public string Name; // (48)
        public string Instrument; // (48)
        public int GelSize;
        public int Unknown1;
    }


    public class IndexFile
    {
        public int Count;
        public List<IndexInfo> Indexes;
    }
}
