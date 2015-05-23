using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExporter.Common
{
    public interface IExporter
    {
        void Export(List<ShowBase> selectedItems);
    }
}
