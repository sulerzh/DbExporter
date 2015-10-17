using System;
using System.Collections.Generic;
using System.Linq;
using DbExporter.Common;

namespace DbExporter.Provider.Aggram
{
    public class AggramDbProvider : IDbProvider
    {
        public List<DateTime> GetAllTestDate()
        {
            RecordQuery query = new RecordQuery(GlobalConfigVars.DbPath);
            return query.GetAllDates().Select(r => r.StartTime.Date).Distinct().ToList();
        }

        public List<ShowBase> GetResultByFilterDate(DateTime testDate)
        {
            RecordQuery query = new RecordQuery(GlobalConfigVars.DbPath);
            return query.GetSampleByFilterDate(testDate);
        }
    }
}
