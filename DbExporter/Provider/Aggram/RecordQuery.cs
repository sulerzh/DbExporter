using System;
using System.Collections.Generic;
using System.Linq;
using DbExporter.Export.Aggram;
using DbExporter.Provider.Aggram.ParadoxReader;
using System.Data;
using DbExporter.Common;

namespace DbExporter.Provider.Aggram
{
    public class RecordQuery : IAggRamDataAccess
    {
        ParadoxConnection _Connection;

        List<SimpleRowInfo> _TestTypeList;
        List<SimpleRowInfo> _RunTypes;
        List<SimpleRowInfo> _Patients;
        List<TestParmNameInfo> _TestParmNames;

        DataSet _ResultDataSet;

        public event AddCurve OnAddCurve;

        public RecordQuery(string dbPath)
        {
            _Connection = new ParadoxConnection(dbPath);
        }

        public bool IsSettingOK()
        {
            return _Connection.IsOpen();
        }

        #region Read Data From Paradox Table

        /// <summary>
        /// 1 HRTestTypes
        /// </summary>
        public List<SimpleRowInfo> HRTestTypes
        {
            get
            {
                if (_TestTypeList == null)
                {
                    QueryArgs q = new QueryArgs("HRTestTypes", "TestTypeTag", "TestTypeID");
                    _TestTypeList = GetSimpleRowInfos(q);
                }
                return _TestTypeList;
            }
        }

        /// <summary>
        /// 2 RunTypes
        /// </summary>
        public List<SimpleRowInfo> RunTypes
        {
            get
            {
                if (_RunTypes == null)
                {
                    _RunTypes = new List<SimpleRowInfo>();
                    _RunTypes.Add(new SimpleRowInfo(0, "Patients"));
                    _RunTypes.Add(new SimpleRowInfo(1, "References"));
                    _RunTypes.Add(new SimpleRowInfo(2, "Standards"));
                    _RunTypes.Add(new SimpleRowInfo(3, "QC"));
                }
                return _RunTypes;
            }
        }

        /// <summary>
        /// 3 获取某种测试的试剂类型列表
        /// </summary>
        /// <param name="testTypeTag">测试类型编号</param>
        /// <returns></returns>
        public List<SimpleRowInfo> HRTestParms(int testTypeTag)
        {
            var condition = new ParadoxCondition.Compare(
                ParadoxCompareOperator.Equal, testTypeTag, 0, 0);
            QueryArgs q = new QueryArgs("HRTestParms", condition, "ProcTag", "ProcID");
            return GetSimpleRowInfos(q);
        }

        /// <summary>
        /// 4 获取Worklist列表
        /// </summary>
        /// <param name="testTypeTag"></param>
        /// <param name="mainRunType"></param>
        /// <param name="mainTestID"></param>
        /// <returns></returns>
        public List<SimpleRowInfo> HRWorkLists(int testTypeTag, int mainRunType, int procTag, bool useMainTest)
        {
            int dataIndex = useMainTest ? 9 : 8;
            var condition =
                new ParadoxCondition.LogicalAnd(
                    new ParadoxCondition.Compare(
                    ParadoxCompareOperator.Greater, 0, 0, 0),
                    new ParadoxCondition.Compare(
                        ParadoxCompareOperator.Equal, testTypeTag, 7, 0),
                    new ParadoxCondition.Compare(
                        ParadoxCompareOperator.Equal, (short)mainRunType, 5, 0),
                    new ParadoxCondition.Compare(
                        ParadoxCompareOperator.Equal, procTag, dataIndex, 0)
                        );
            QueryArgs q = new QueryArgs("HRRuns", condition, "WrkLstNum", "WrkLstID");
            return GetSimpleRowInfos(q, false);
        }

        /// <summary>
        /// 5 获取PrimID列表
        /// </summary>
        /// <returns></returns>
        public List<SimpleRowInfo> Patients
        {
            get
            {
                if (_TestTypeList == null)
                {
                    QueryArgs q = new QueryArgs("Patients", "", "PrimID");
                    _Patients = GetSimpleRowInfos(q);
                }
                return _Patients;
            }
        }

        public List<TestParmNameInfo> TestParmNames
        {
            get
            {
                if (_TestParmNames == null)
                {
                    QueryArgs q = new QueryArgs("HRTstPrmNames");
                    _TestParmNames = GetTestParmNames(q);
                }
                return _TestParmNames;
            }
        }

        /// <summary>
        /// 获取Curve Sequence Number列表
        /// </summary>
        public List<SimpleRowInfo> CrvSeqNos(int testTypeTag, int mainRunType, string mainTestID)
        {
            var condition =
            new ParadoxCondition.LogicalAnd(
                new ParadoxCondition.Compare(
                    ParadoxCompareOperator.Greater, 0, 0, 0),
                new ParadoxCondition.Compare(
                    ParadoxCompareOperator.Equal, testTypeTag, 7, 0),
                new ParadoxCondition.Compare(
                    ParadoxCompareOperator.Equal, (short)mainRunType, 5, 0),
                new ParadoxCondition.Compare(
                    ParadoxCompareOperator.Equal, mainTestID, 8, 0)
                    );
            QueryArgs q = new QueryArgs("HRRuns", condition, "CrvSeqNum", "");
            return GetSimpleRowInfos(q, false);
        }

        public int GetCurves(string primID, int crvSeqNo, string worklistID, int worklistNumber)
        {
            if (crvSeqNo > 0)
            {
                //var condition = new ParadoxCondition.Compare(ParadoxCompareOperator.Equal, mainTestID, 8, 0);
                //QueryArgs q = new QueryArgs("HRRuns", condition, "CrvSeqNum", "");
                //return GetSimpleRowInfos(q, false);
            }

            if (!String.IsNullOrEmpty(primID))
            { }

            if (!String.IsNullOrEmpty(worklistID) && worklistNumber > 0)
            {
                var condition =
                    new ParadoxCondition.LogicalAnd(
                        new ParadoxCondition.Compare(
                            ParadoxCompareOperator.Equal, worklistID, 4, 0),
                        new ParadoxCondition.Compare(
                            ParadoxCompareOperator.Equal, worklistNumber, 0, 0)
                        );
                QueryArgs q = new QueryArgs("HRRuns", condition, "", "");

                return GetResultInfos(q, false);
            }

            return 0;
        }

        public DataSet ResultDataSet
        {
            get
            {
                if (_ResultDataSet == null)
                {
                    _ResultDataSet = new DataSet("ResultDataSet");
                    _ResultDataSet.Tables.Add(new DataTable("Report"));
                    _ResultDataSet.Tables.Add(new DataTable("Patient"));
                }
                return _ResultDataSet;
            }
        }

        #endregion

        #region GetSimpleRowInfos

        /// <summary>
        /// 获取简单行信息列表
        /// </summary>
        /// <param name="q">查询参数</param>
        /// <returns></returns>
        private List<SimpleRowInfo> GetSimpleRowInfos(QueryArgs q)
        {
            return GetSimpleRowInfos(q, true);
        }

        /// <summary>
        /// 获取简单行信息列表
        /// </summary>
        /// <param name="q">查询参数</param>
        /// <param name="useIndex">是否使用索引</param>
        /// <returns></returns>
        private List<SimpleRowInfo> GetSimpleRowInfos(QueryArgs q, bool useIndex)
        {
            List<SimpleRowInfo> results = new List<SimpleRowInfo>();
            var rdr = _Connection.ExecuteQuery(q.TableName, q.Condition, useIndex);
            while (rdr.Read())
            {
                SimpleRowInfo testType = new SimpleRowInfo();
                if (!String.IsNullOrEmpty(q.TagName))
                {
                    testType.Tag = (int)rdr[q.TagName];
                }
                if (!String.IsNullOrEmpty(q.IDName))
                {
                    testType.ID = rdr[q.IDName].ToString();
                }
                results.Add(testType);
            }

            return results;
        }

        /// <summary>
        /// 获取测试参数名称
        /// </summary>
        /// <param name="q"></param>
        /// <param name="useIndex"></param>
        /// <returns></returns>
        private List<TestParmNameInfo> GetTestParmNames(QueryArgs q)
        {
            List<TestParmNameInfo> results = new List<TestParmNameInfo>();
            var rdr = _Connection.ExecuteQuery(q.TableName, q.Condition);
            while (rdr.Read())
            {
                TestParmNameInfo tstPrmNameInfo = new TestParmNameInfo();
                tstPrmNameInfo.MTest = (int)rdr["MTest"];
                tstPrmNameInfo.RunType = (int)rdr["RunType"];
                tstPrmNameInfo.OrderNum = (int)rdr["OrderNum"];
                tstPrmNameInfo.ParmType = (int)rdr["ParmType"];
                tstPrmNameInfo.ParmName = (string)rdr["ParmName"];
                results.Add(tstPrmNameInfo);
            }

            return results;
        }

        private int GetResultInfos(QueryArgs q, bool useIndex)
        {
            List<string> primIDs = new List<string>();
            _ResultDataSet = new DataSet("ResultDataSet");
            var rdr = _Connection.ExecuteQuery(q.TableName, q.Condition, useIndex);
            DataTable reportTable = CreateReportTable(rdr, "Report", _ResultDataSet);
            while (rdr.Read())
            {
                // 初始化数据记录
                DataRow row = reportTable.NewRow();
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    row[i] = rdr[i];
                }
                reportTable.Rows.Add(row);

                // 保存病人ID
                primIDs.Add((string)rdr["PrimID"]);

                // 创建曲线点信息
                if (OnAddCurve != null)
                {
                    string label = "Channel " + rdr["ChnlNum"].ToString();

                    AggRamCurve crv = new AggRamCurve(
                        label,
                        (int)rdr["InitialMax"], (int)rdr["InitialMin"],
                        (int)rdr["ScaleSet1Rd"], (int)rdr["MaxPCPoint"], (double)rdr["ScaleSetResult"],
                        (int)rdr["DataPoints"], (byte[])rdr["Data"]);
                    OnAddCurve(label, crv);
                    //OnAddCurve(label, CreateCurve((int)rdr["DataPoints"], (byte[])rdr["Data"]));
                }
            }

            _ResultDataSet.Tables.Add(CreatePatientsTable(primIDs));

            return primIDs.Count;
        }

        private DataTable CreateReportTable(ParadoxDataReader rdr, string viewName, DataSet ds)
        {
            DataTable reportTable = new DataTable("Report");
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                reportTable.Columns.Add(rdr.FieldNames[i], rdr.GetFieldType(i));
            }
            ds.Tables.Add(reportTable);

            return reportTable;
        }

        /// <summary>
        /// 创建结点集合
        /// </summary>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<CurvePoint> CreateCurve(int count, byte[] data)
        {
            List<CurvePoint> c = new List<CurvePoint>();
            for (int i = 0; i < count; i++)
            {
                byte[] buffer = new byte[8];
                Buffer.BlockCopy(data, i * 8, buffer, 0, 8);
                c.Add(new CurvePoint(buffer));
            }
            return c;
        }

        private void CreateReportTable(int mTest, int runType)
        {
            DataTable baseTable = new DataTable();

            // DataGridView 表头显示名称集合
            var fields = from f in TestParmNames
                         where f.MTest == mTest && f.RunType == runType
                         orderby f.OrderNum
                         select f;
            //var table = new ParadoxTable(_dbPath, "HRRuns"); // table
            //var rdr = new ParadoxDataReader(table, null); // reader

            //for (int i = 0; i < table.FieldCount; i++)
            //{
            //    baseTable.Columns.Add(table.FieldNames[i], rdr.GetFieldType(i));
            //}
        }

        private DataTable CreatePatientsTable(List<string> primIDs)
        {
            DataTable patientTable = null;
            foreach (string primID in primIDs)
            {
                var condition = new ParadoxCondition.Compare(
                            ParadoxCompareOperator.Equal, primID, 0, 0);
                var rdr = _Connection.ExecuteQuery("Patients", condition, false);

                // 初始化表
                if (patientTable == null)
                {
                    patientTable = new DataTable("patient");
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        patientTable.Columns.Add(rdr.FieldNames[i], rdr.GetFieldType(i));
                    }
                }

                if (rdr.Read())
                {
                    DataRow row = patientTable.NewRow();

                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        row[i] = rdr[i];
                    }

                    patientTable.Rows.Add(row);
                }
                else
                {
                    DataRow row = patientTable.NewRow();
                    row[0] = primID;
                    patientTable.Rows.Add(row);
                }
            }

            return patientTable;
        }

        #endregion

        #region QueryArgs

        /// <summary>
        /// 简单行信息的检索参数对象
        /// </summary>
        public class QueryArgs
        {
            #region Properties

            public string TableName { get; set; }
            public string TagName { get; set; }
            public string IDName { get; set; }
            public ParadoxCondition Condition { get; set; }

            #endregion

            #region Constructor

            public QueryArgs()
            { }

            public QueryArgs(string table)
            {
                this.TableName = table;
                this.Condition = new NullCondtion();
                this.TagName = "";
                this.IDName = "";
            }

            public QueryArgs(string table, string tag, string id)
            {
                this.TableName = table;
                this.Condition = new NullCondtion();
                this.TagName = tag;
                this.IDName = id;
            }

            public QueryArgs(string table, ParadoxCondition condition, string tag, string id)
            {
                this.TableName = table;
                this.Condition = condition;
                this.TagName = tag;
                this.IDName = id;
            }

            #endregion
        }

        #endregion

        #region NullCondition

        /// <summary>
        /// 空过滤条件
        /// </summary>
        public class NullCondtion : ParadoxCondition
        {
            public override bool IsDataOk(ParadoxRecord dataRec)
            {
                return true;
            }

            public override bool IsIndexPossible(ParadoxRecord indexRec, ParadoxRecord nextRec)
            {
                return true;
            }
        }

        #endregion


        #region IAggRamDataAccess 成员


        public List<SimpleRowInfo> GetWorklistNumber(string primID)
        {
            throw new NotImplementedException();
        }

        #endregion

        internal List<SimpleResult> GetAllDates()
        {
            var condition =
                new ParadoxCondition.Compare(
                    ParadoxCompareOperator.Greater, 0, 0, 0);
            var rdr = _Connection.ExecuteQuery("HRRuns", condition, false);
            List<SimpleResult> result = new List<SimpleResult>();
            while (rdr.Read())
            {
                SimpleResult r = new SimpleResult();
                r.CrvSeqNum = rdr.GetInt32(rdr.GetOrdinal("CrvSeqNum"));
                r.PrimId = rdr.GetString(rdr.GetOrdinal("PrimId"));
                r.StartTime = rdr.GetDateTime(rdr.GetOrdinal("StartTime"));
                result.Add(r);
            }
            return result;
            // return GetSimpleRowInfos(q, false);
        }

        internal List<ShowBase> GetSampleByFilterDate(DateTime filterDate)
        {
            var condition =
            new ParadoxCondition.LogicalAnd(
                new ParadoxCondition.Compare(
                    ParadoxCompareOperator.Greater, 0, 0, 0),
                new ParadoxCondition.Compare(
                    ParadoxCompareOperator.GreaterOrEqual, filterDate.Date, 2, 0),
                    new ParadoxCondition.Compare(
                    ParadoxCompareOperator.Less, filterDate.AddDays(1).Date, 2, 0)
                    );
            var c = new ParadoxCondition.LogicalAnd(
                new ParadoxCondition.Compare(
                    ParadoxCompareOperator.Greater, 0, 0, 0));
            var rdr = _Connection.ExecuteQuery("HRRuns", condition, false);
            List<ShowBase> result = new List<ShowBase>();
            while (rdr.Read())
            {
                SimpleResult r = new SimpleResult();
                r.CrvSeqNum = rdr.GetInt32(rdr.GetOrdinal("CrvSeqNum"));
                r.PrimId = rdr.GetString(rdr.GetOrdinal("PrimId"));
                r.StartTime = rdr.GetDateTime(rdr.GetOrdinal("StartTime"));
                result.Add((ShowBase)r);
            }
            return result;
            // return GetSimpleRowInfos(q, false);
        }
        private double FloatPS(double val, int res)
        {
            return Double.Parse(val.ToString("F" + res.ToString()));
        }

        public PatientInfo GetPatientInfo(string primId)
        {
            var condition =
                new ParadoxCondition.Compare(ParadoxCompareOperator.Equal, primId, 0, 0);
            var sr = _Connection.ExecuteQuery("Patients", condition, false);
            if (sr.Read())
            {
                PatientInfo result = new PatientInfo();
                result.PrimID = (string)sr[0];
                result.Name = sr[1] == DBNull.Value ? "" : (string)sr[1];
                result.Birthdate = sr[2] == DBNull.Value ? "" : (string)sr[2];
                result.Sex = sr[3] == DBNull.Value ? "" : (string)sr[3];
                result.Physician = sr[4] == DBNull.Value ? "" : (string)sr[4];
                result.Diagnosis = sr[5] == DBNull.Value ? "" : (string)sr[5];
                return result;
            }
            return new PatientInfo { PrimID = primId };
        }

        public HrTestParams GetTestParams(int procTag)
        {
            var condition =
                new ParadoxCondition.Compare(ParadoxCompareOperator.Equal, procTag, 2, 0);
            var sr = _Connection.ExecuteQuery("HRTestParms", condition, false);
            if (sr.Read())
            {
                HrTestParams result = new HrTestParams();
                result.ProcTag = procTag;
                result.ProcID = (string)sr["ProcID"];
                result.Abbrev = (string)sr["Abbrev"];
                return result;
            }
            return null;
        }

        internal AggRamDbState GetSampleInfoByCrvSeqNum(int crvSeqNum)
        {
            var condition =
            new ParadoxCondition.LogicalAnd(
                new ParadoxCondition.Compare(
                    ParadoxCompareOperator.Greater, 0, 0, 0),
                new ParadoxCondition.Compare(
                    ParadoxCompareOperator.Equal, crvSeqNum, 3, 0));
            var sr = _Connection.ExecuteQuery("HRRuns", condition, false);
            if (sr.Read())
            {
                AggRamDbState result = new AggRamDbState();
                // 基本信息
                result.WrkLstNum = (int)sr["WrkLstNum"];
                result.ChnlNum = (short)((short)sr["ChnlNum"] + 1);
                result.CrvSeqNum = (int)sr["CrvSeqNum"];
                result.StartTime = ((DateTime)sr["StartTime"]).ToString("yyyy/MM/dd HH:mm:ss");

                // 创建曲线点信息
                int lblChnNum = (short)sr["ChnlNum"] + 1;
                string label = "Channel " + lblChnNum.ToString();
                AggRamCurve crv = new AggRamCurve(
                    label,
                    (int)sr["InitialMax"], (int)sr["InitialMin"],
                    (int)sr["ScaleSet1Rd"], (int)sr["MaxPCPoint"], (double)sr["ScaleSetResult"],
                    (int)sr["DataPoints"], (byte[])sr["Data"]);
                Curve c = new Curve();
                c.Label = crv.Label;
                c.InitialMin = (int)sr["InitialMin"];
                c.InitialMax = (int)sr["InitialMax"];
                c.ZeroODPoint = crv.ZeroODPoint;
                c.MaxPCPoint = crv.MaxPCPoint;
                c.PointUnit = crv.PointUnit;
                c.CurvePoints = crv.CurvePoints;
                c.Count = crv.Count;
                result.Curve = c;
                // Add Report Row
                ReportInfo reportRow = new ReportInfo();
                reportRow.ChnlNum = lblChnNum;
                reportRow.StartTime = (DateTime)sr["StartTime"];

                int selectedProcTag = (int)sr["ProcTag"];
                var procInfo = GetTestParams(selectedProcTag);
                if (procInfo != null)
                {
                    reportRow.ProcID = procInfo.ProcID;
                    reportRow.Abbrev = procInfo.Abbrev;
                }
                reportRow.Unit = (string)sr["Unit"];
                reportRow.Conc = (double)sr["Conc"];
                reportRow.PRP = FloatPS(crv.PRP, 3);
                reportRow.PPP = FloatPS(crv.PPP, 3);
                reportRow.MaxPercent = FloatPS(crv.MaxPercent * 100, 1);
                reportRow.MaxPCTime = FloatPS((int)sr["MaxPCTime"] / 1000.0, 0);
                reportRow.LagTime = FloatPS((int)sr["LagTime"] / 1000.0, 1);
                reportRow.Slope = FloatPS((double)sr["Slope"], 1);
                result.Report = reportRow;

                string selectedPrimID = (string)sr["PrimID"];

                result.Patient = GetPatientInfo(selectedPrimID);
                return result;
            }
            return null;
        }
    }
}
