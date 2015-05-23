using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExporter.Common
{
    public interface IDbProvider
    {
        List<DateTime> GetAllTestDate();
        List<ShowBase> GetResultByFilterDate(DateTime testDate);
    }
}
