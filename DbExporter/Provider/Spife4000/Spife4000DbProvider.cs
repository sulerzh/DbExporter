using DbExporter.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExporter.Provider.Spife4000
{
    public class Spife4000DbProvider : IDbProvider
    {
        private bool IsMatched(string tdfFile, DateTime filterDate, string filterSampleId, out TdfInfo tdfInfo)
        {
            if (TdfParser.Parse(tdfFile, out tdfInfo))
            {
                return tdfInfo != null &&
                       filterDate.Date == tdfInfo.ScannedTime.Date &&
                       (filterSampleId == null ||
                       filterSampleId == tdfInfo.SampleId);
            }
            return false;
        }
        
        /// <summary>
        /// 获取病人信息
        /// </summary>
        /// <param name="filterDate"></param>
        /// <param name="filterSampleId"></param>
        /// <returns></returns>
        public static PatientInfo GetSampleInfo(TdfInfo tdfInfo)
        {
            var sampleResult = BdfParser.Parse(tdfInfo.BdfFilePath);
            sampleResult.BasicInfo.ScannedTime = tdfInfo.ScannedTime;
            return sampleResult;
        }

        public List<ShowBase> GetResultByFilterDate(DateTime testDate)
        {
            List<TdfInfo> result = new List<TdfInfo>();

            var rootFolder = new DirectoryInfo(GlobalConfigVars.DbPath);
            foreach (FileInfo bdfFileInfo in rootFolder.GetFiles("*.BDF", SearchOption.AllDirectories))
            {
                string tdfFilePath = Path.ChangeExtension(bdfFileInfo.FullName, ".TDF");
                TdfInfo tdfInfo;
                if (IsMatched(tdfFilePath, testDate, null, out tdfInfo))
                {
                    result.Add(tdfInfo);
                }
            }
            // 排序
            result.Sort();
            return result.ToList<ShowBase>();
        }

        public List<DateTime> GetAllTestDate()
        {
            List<TdfInfo> result = new List<TdfInfo>();

            var rootFolder = new DirectoryInfo(GlobalConfigVars.DbPath);
            foreach (FileInfo bdfFileInfo in rootFolder.GetFiles("*.BDF", SearchOption.AllDirectories))
            {
                string tdfFilePath = Path.ChangeExtension(bdfFileInfo.FullName, ".TDF");
                TdfInfo tdfInfo;
                if (TdfParser.Parse(tdfFilePath, out tdfInfo))
                {
                    result.Add(tdfInfo);
                }
            }

            return result.Select(r => r.ScannedTime.Date).Distinct().ToList();
        }
    }
}
