using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExporter.Common;

namespace DbExporter.Provider.Aggram
{
    public class AggramDbProvider : IDbProvider
    {
        public List<DateTime> GetAllTestDate()
        {
            throw new NotImplementedException();
        }

        public List<ShowBase> GetResultByFilterDate(DateTime testDate)
        {
            RecordQuery query = new RecordQuery(GlobalConfigVars.DbPath);
            return query.GetSampleByFilterDate(testDate).ToList<ShowBase>();
        }
    }
}
