using System.Collections.Generic;

namespace DbExporter.Common
{
    public interface IExporter
    {
        void Export(List<ShowBase> selectedItems);
    }
}
