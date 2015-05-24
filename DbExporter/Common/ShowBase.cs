using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExporter.Common
{
    public abstract class ShowBase
    {
        public string Label
        {
            get { return GetLabel(); }
        }

        protected abstract string GetLabel();
        public static string GetDescription()
        {
            string result = "样本列表";
            switch (GlobalConfigVars.GetDbType(GlobalConfigVars.DbType))
            {
                case SupportedDbType.AggRAM:
                    result = "病案号（曲线序号）";
                    break;
                case SupportedDbType.Platinum:
                    result = "Lis标识（测试类型）";
                    break;
                case SupportedDbType.QS2000:
                    break;
                case SupportedDbType.Spife4000:
                    result = "样本列表(胶片号 序号 ： 样本号)";
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}

