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
            get
            {
                return GetLabel();
            }
        }

        protected abstract string GetLabel();
    }
}

