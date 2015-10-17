using System;
using System.Collections.Generic;

namespace DbExporter.Common
{
    public interface IDbProvider
    {
        List<DateTime> GetAllTestDate();
        List<ShowBase> GetResultByFilterDate(DateTime testDate);
    }
}
