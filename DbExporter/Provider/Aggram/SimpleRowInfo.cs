namespace DbExporter.Provider.Aggram
{
    /// <summary>
    /// 简单行信息，包含主键和字符串信息
    /// </summary>
    public class SimpleRowInfo
    {
        public int Tag
        { get; set; }
        public string ID
        { get; set; }

        public SimpleRowInfo()
        { }

        public SimpleRowInfo(int tag, string id)
        {
            Tag = tag;
            ID = id;
        }
    }

    public class TestParmNameInfo
    {
        public int MTest { get; set; }
        public int RunType { get; set; }
        public int OrderNum { get; set; }
        public int ParmType { get; set; }
        public string ParmName { get; set; }

        public TestParmNameInfo()
        { }
    }
}
