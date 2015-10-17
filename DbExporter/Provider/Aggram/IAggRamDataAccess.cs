using System.Data;
using System.Collections.Generic;
namespace DbExporter.Provider.Aggram
{
    public delegate void AddCurve(string curveName, AggRamCurve curve);

    public interface IAggRamDataAccess
    {
        /// <summary>
        /// delegate OnAddCurve
        /// </summary>
        event AddCurve OnAddCurve;

        /// <summary>
        /// 运行类型
        /// </summary>
        List<SimpleRowInfo> RunTypes { get; }

        /// <summary>
        /// 测试类型
        /// </summary>
        List<SimpleRowInfo> HRTestTypes { get; }

        /// <summary>
        /// 测试参数名称
        /// </summary>
        List<TestParmNameInfo> TestParmNames { get; }

        /// <summary>
        /// 病人信息
        /// </summary>
        List<SimpleRowInfo> Patients { get; }

        /// <summary>
        /// 测试参数
        /// </summary>
        /// <param name="testTypeTag"></param>
        /// <returns></returns>
        List<SimpleRowInfo> HRTestParms(int testTypeTag);

        /// <summary>
        /// 曲线序列号列表
        /// </summary>
        /// <param name="testTypeTag"></param>
        /// <param name="mainRunType"></param>
        /// <param name="mainTestID"></param>
        /// <returns></returns>
        List<SimpleRowInfo> CrvSeqNos(int testTypeTag, int mainRunType, string mainTestID);

        /// <summary>
        /// 工作列表
        /// </summary>
        /// <param name="testTypeTag"></param>
        /// <param name="mainRunType"></param>
        /// <param name="procTag"></param>
        /// <param name="useMainTest"></param>
        /// <returns></returns>
        List<SimpleRowInfo> HRWorkLists(int testTypeTag, int mainRunType, int procTag, bool useMainTest);

        /// <summary>
        /// 通过病人ID获取wrkLstNumber列表
        /// </summary>
        /// <param name="primID"></param>
        /// <returns></returns>
        List<SimpleRowInfo> GetWorklistNumber(string primID);

        /// <summary>
        /// 结果数据集
        /// </summary>
        DataSet ResultDataSet { get; }

        /// <summary>
        /// 获取曲线
        /// </summary>
        /// <param name="primID"></param>
        /// <param name="crvSeqNo"></param>
        /// <param name="worklistID"></param>
        /// <param name="worklistNumber"></param>
        /// <returns></returns>
        int GetCurves(string primID, int crvSeqNo, string worklistID, int worklistNumber);
    }
}
