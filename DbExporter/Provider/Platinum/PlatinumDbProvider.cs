﻿using DbExporter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExporter.Provider.Platinum
{
    public class PlatinumDbProvider : IDbProvider
    {
        public List<DateTime> GetAllTestDate()
        {
            throw new NotImplementedException();
        }

        public List<ShowBase> GetResultByFilterDate(DateTime testDate)
        {
            return PlatinumDbAccess.GetReportInfos(testDate).ToList<ShowBase>();
        }
    }
}
