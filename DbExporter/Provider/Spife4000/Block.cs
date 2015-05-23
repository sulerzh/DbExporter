namespace DbExporter.Provider.Spife4000
{
    public class Block
    {
        public int FractionIndex; //分区号
        public int StartIndex; //起始点
        public int EndIndex; //结束点

        public override string ToString()
        {
            return FractionIndex.ToString() + "," + StartIndex.ToString() + "," + EndIndex.ToString();
        }
    }
}
