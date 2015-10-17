using DbExporter.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DbExporter.Provider.Platinum
{
    public class PlatinumDbProvider : IDbProvider
    {
        public List<DateTime> GetAllTestDate()
        {
            return PlatinumDbAccess.GetAllDates().Select(d => d.Date).ToList();
        }

        public List<ShowBase> GetResultByFilterDate(DateTime testDate)
        {
            return PlatinumDbAccess.GetReportInfos(testDate).ToList();
        }
    }
}
