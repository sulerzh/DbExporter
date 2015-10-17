using DbExporter.Common;
using DbExporter.Helper;
using DbExporter.Provider.QS2000;
using System.Collections.Generic;

namespace DbExporter.Export.QS2000
{
    public class Qs2000Exporter : IExporter
    {
        private QS2000Provider Provider = new QS2000Provider();
        public Qs2000Exporter()
        {

        }
        public void Export(List<ShowBase> selectedItems)
        {
            foreach (ShowBase item in selectedItems)
            {
                TFileData query = (TFileData)item;
                Qs2000State state = this.Provider.GetState(query);
                string filePath = string.Format(@"{0}\{1}.xml",
                    GlobalConfigVars.XmlPath,
                    query.SeqNum + "_" + query.Id);
                OjbectDataXmlSerializer.Save(state, filePath);
            }
        }
    }
}
