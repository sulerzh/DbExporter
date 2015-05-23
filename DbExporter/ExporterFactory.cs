using DbExporter.Common;
using DbExporter.Export.Aggram;
using DbExporter.Export.Platinum;
using DbExporter.Export.Spife4000;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExporter
{
    public class ExporterFactory
    {
        public static IExporter Create(SupportedDbType dbType)
        {
            switch (dbType)
            {
                case SupportedDbType.AggRAM:
                    return new AggramExporter();
                case SupportedDbType.Platinum:
                    return new PlatinumExporter();
                case SupportedDbType.Spife4000:
                    return new Spife4000Exporter();
                case SupportedDbType.QS2000:
                    return null;
            }
            return null;
        }
    }
}
