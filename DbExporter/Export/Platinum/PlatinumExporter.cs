using System.Collections.Generic;
using DbExporter.Common;
using DbExporter.Helper;
using DbExporter.Provider.Platinum;

namespace DbExporter.Export.Platinum
{
    public class PlatinumExporter : IExporter
    {
        public void Export(List<ShowBase> selectedItems)
        {
            foreach (ShowBase item in selectedItems)
            {
                PlatinumState state = new PlatinumState((ScanResult)item);
                string filePath = string.Format(@"{0}\{1}.xml",
                    GlobalConfigVars.XmlPath,
                    state.Scan.ScanIdNr);
                OjbectDataXmlSerializer.Save(state, filePath);
            }
        }
    }
}
