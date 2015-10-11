using DbExporter.Common;
using DbExporter.Provider.Aggram;
using DbExporter.Provider.Platinum;
using DbExporter.Provider.QS2000;
using DbExporter.Provider.Spife4000;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExporter
{
    public class DbProviderFactory
    {
        public static IDbProvider Create(SupportedDbType dbType)
        {
            switch (dbType)
            {
                case SupportedDbType.AggRAM:
                    return new AggramDbProvider();
                case SupportedDbType.Platinum:
                    return new PlatinumDbProvider();
                case SupportedDbType.Spife4000:
                    return new Spife4000DbProvider();
                case SupportedDbType.QS2000:
                    return new QS2000Provider();
            }
            return null;
        }
    }
}
