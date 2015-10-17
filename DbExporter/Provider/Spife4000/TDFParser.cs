using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DbExporter.Provider.Spife4000
{
    public class TdfParser
    {
        private static Regex PatientIdRegex = new Regex(@"Label=Patient I[D|d](entifier)?\s*Value=(\w+)\s*", RegexOptions.Compiled);
        private static Regex PatientIdentifierRegex = new Regex(@"Sample\/[\s\S]*Patient_Identifier=(\d+)[\s\S]*\/Sample", RegexOptions.Compiled);
        private static Regex ScannedDateTimeRegex = new Regex(@"Date_Time_Scanned=((\d{2}|\d{4})\/\d{2}\/(\d{2}|\d{4})\s+\d{2}:\d{2}:\d{2}:\d{2})", RegexOptions.Compiled);

        private static string TdfParseRegex =
            @"Gel_Identifier=(\d+)" +
            @"[\s\S]*" +
            @"Sample_Number=(\d+)" +
            @"[\s\S]*" +
            // 扫描时间 
            @"Date_Time_Scanned=((\d{2}|\d{4})\/\d{2}\/(\d{2}|\d{4})\s+\d{2}:\d{2}:\d{2}):\d{2}"+
            @"[\s\S]*"+
            // 病人ID
            @"Sample\/[\s\S]*Patient_Identifier=(\d+)?[\s\S]*\/Sample"+
            @"[\s\S]*" +
            // 病人姓名
            @"Label=Patient Name\s*Value=(\d+)?\s*";
        private static Regex TdfRegex = new Regex(TdfParseRegex, RegexOptions.Compiled);

        public static bool Parse(string file, out TdfInfo tdfInfo)
        {
            //scannedTime = DateTime.MaxValue;
            //sampleId = string.Empty;
            tdfInfo = null;
            try
            {
                using (var sr = new StreamReader(file))
                {
                    var content = sr.ReadToEnd();
                    var m = TdfRegex.Match(content);
                    if (m.Success)
                    {
                        tdfInfo = new TdfInfo
                        {
                            GelId = m.Groups[1].Value,
                            SampleNum = m.Groups[2].Value,
                            ScannedTime = DateTime.Parse(m.Groups[3].Value),
                            SampleId = m.Groups[6].Value != string.Empty ? m.Groups[6].Value : m.Groups[7].Value,
                            BdfFilePath = Path.ChangeExtension(file, ".BDF")
                        };
                        if (tdfInfo.SampleId == "")
                        {
                            var s = PatientIdRegex.Match(content);
                            if (s.Success)
                            {
                                tdfInfo.SampleId = s.Groups[2].Value;
                            }
                        }
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }

            return false;
        }
    }
}
