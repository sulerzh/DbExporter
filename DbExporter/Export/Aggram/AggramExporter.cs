using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExporter.Common;
using DbExporter.Helper;
using DbExporter.Provider.Aggram;

namespace DbExporter.Export.Aggram
{
    public class AggramExporter : IExporter
    {
        public void Export(List<ShowBase> selectedItems)
        {
            RecordQuery query = new RecordQuery(GlobalConfigVars.DbPath);
            foreach (SimpleResult item in selectedItems)
            {
                var state = query.GetSampleInfoByCrvSeqNum(item.CrvSeqNum);
                if (state == null) continue;

                string filePath = string.Format(@"{0}\{1}_{2}.xml",
                    GlobalConfigVars.XmlPath,
                    item.PrimId,
                    item.CrvSeqNum);
                OjbectDataXmlSerializer.Save(state, filePath);
            }
        }
    }
}
