using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.OleDb;
using System.Data;
using DbExporter.Provider.Aggram.ParadoxReader;

namespace DbExporter.Provider.Aggram
{
    public class MSAccessOperation : IAggRamDataAccess
    {
        private string _pdxDirectory = "";
        private string _accessDbPath = "";

        private string[] _tableNames = new string[] { "Patients", "HRTestTypes", "HRTstPrmNames", "HRTestParms", "HRWorkLists", "HRRuns" };

        private ParadoxConnection _pdxConnection;
        private OleDbConnection _dbConnection;

        // Fill the DataSet.
        private DataSet _dataSet;

        private List<SimpleRowInfo> _TestTypeList;
        private List<SimpleRowInfo> _RunTypes;
        private List<SimpleRowInfo> _Patients;
        private List<TestParmNameInfo> _TestParmNames;

        public event AddCurve OnAddCurve;

        DataSet _ResultDataSet;

        /// <summary>
        /// 1 HRTestTypes
        /// </summary>
        public List<SimpleRowInfo> HRTestTypes
        {
            get
            {
                if (_TestTypeList == null)
                {
                    _TestTypeList = (from qs in _dataSet.Tables["HRTestTypes"].AsEnumerable()
                                     select new SimpleRowInfo
                             {
                                 ID = qs.Field<string>("TestTypeID"),
                                 Tag = qs.Field<int>("TestTypeTag")
                             }).ToList();
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
            return (from qs in _dataSet.Tables["HRTestParms"].AsEnumerable()
                    select new SimpleRowInfo
                    {
                        ID = qs.Field<string>("ProcID"),
                        Tag = qs.Field<int>("ProcTag")
                    }).ToList();
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
            return (from qs in _dataSet.Tables["HRRuns"].AsEnumerable()
                    where qs.Field<int>("TestTypeTag") == testTypeTag
                 && qs.Field<int>("RunType") == mainRunType
                 && (useMainTest ?
                 qs.Field<int>("MainTest") == procTag : qs.Field<int>("ProcTag") == procTag)
                    select new SimpleRowInfo
                    {
                        ID = qs.Field<string>("WrkLstID"),
                        Tag = qs.Field<int>("WrkLstNum")
                    }).ToList();
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
                    _Patients =
                        (from qs in _dataSet.Tables["Patients"].AsEnumerable()
                         where !qs.Field<string>("PrimID").StartsWith("~~")
                         select new SimpleRowInfo
                         {
                             ID = qs.Field<string>("PrimID"),
                             Tag = 0
                         }).ToList();
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
                    _TestParmNames =
                        (from qs in _dataSet.Tables["HRTstPrmNames"].AsEnumerable()
                         select new TestParmNameInfo
                         {
                             MTest = qs.Field<int>("MTest"),
                             OrderNum = qs.Field<int>("OrderNum"),
                             ParmName = qs.Field<string>("ParmName"),
                             ParmType = qs.Field<int>("ParmType"),
                             RunType = qs.Field<int>("RunType")
                         }).ToList();
                }
                return _TestParmNames;
            }
        }

        public List<SimpleRowInfo> GetWorklistNumber(string primID)
        {
            return (from qs in _dataSet.Tables["HRRuns"].AsEnumerable()
                    where qs.Field<string>("PrimID") == primID
                    select new SimpleRowInfo
                    {
                        ID = qs.Field<string>("WrkLstID"),
                        Tag = qs.Field<int>("WrkLstNum")
                    }).ToList();
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

        #region Constructor

        public MSAccessOperation(string pdxDbDir, string accessDbPath, bool quitSyncProcess)
        {
            //string accessDbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\\AggRam.mdb");
            //if (!File.Exists(accessDbPath))
            //{
            //    string templateDbFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\\aggdb.template");
            //    File.Copy(templateDbFileName, accessDbPath);
            //}

            _pdxDirectory = pdxDbDir;
            _accessDbPath = accessDbPath;

            // 创建数据库连接
            _pdxConnection = new ParadoxConnection(_pdxDirectory);
            _dbConnection = new OleDbConnection(
                "Provider=Microsoft.Jet.OLEDB.4.0;" +
                "Data Source=" + _accessDbPath);

            // 创建DataSet
            _dataSet = new DataSet("AggRam");
            foreach (string tableName in _tableNames)
            {
                FillDataTable(tableName, quitSyncProcess);
            }
        }

        #endregion

        #region Private methods

        private DataTable CreateReportTable()
        {
            DataTable report = new DataTable("Report");
            report.Columns.Add("ChnlNum", typeof(string));
            report.Columns.Add("StartTime", typeof(DateTime));
            report.Columns.Add("ProcID", typeof(string));
            report.Columns.Add("Abbrev", typeof(string));
            report.Columns.Add("Conc", typeof(double));
            report.Columns.Add("Unit", typeof(string));
            report.Columns.Add("PPP", typeof(double));
            report.Columns.Add("PRP", typeof(double));
            report.Columns.Add("MaxPercent", typeof(double));
            report.Columns.Add("MaxPCTime", typeof(int));
            report.Columns.Add("LagTime", typeof(double));
            report.Columns.Add("Slope", typeof(double));
            return report;
        }

        private DataTable CreatePatientTable()
        {
            DataTable patient = new DataTable("Patient");
            patient.Columns.Add("ChnlNum", typeof(int));

            //var header = (from qs in _dataSet.Tables["Patients"].AsEnumerable()
            //              where qs.Field<string>("PrimID").StartsWith("~~~")
            //              select qs).First();
            //foreach (var column in header.ItemArray)
            //{
            //    if (column == DBNull.Value) break;

            //    patient.Columns.Add(column.ToString().Trim('~'), typeof(string));
            //}

            patient.Columns.Add("PrimID", typeof(string));
            patient.Columns.Add("Name", typeof(string));
            patient.Columns.Add("Birthdate", typeof(string));
            patient.Columns.Add("Sex", typeof(string));
            patient.Columns.Add("Physician", typeof(string));
            patient.Columns.Add("Diagnosis", typeof(string));
            return patient;
        }

        private void FillDataTable(string tableName, bool quitSyncProcess)
        {
            using (OleDbDataAdapter adapter = new OleDbDataAdapter())
            {
                // Open the connection.
                if (_dbConnection.State != ConnectionState.Open)
                {
                    _dbConnection.Open();
                }

                // A table mapping names the DataTable.
                adapter.TableMappings.Add("Table", tableName);

                // Create a SqlCommand to retrieve Suppliers data.
                string sql = String.Format("SELECT * FROM {0};", tableName);
                adapter.SelectCommand = new OleDbCommand(sql, _dbConnection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);

                // 填充数据
                adapter.Fill(_dataSet);

                // 数据交换
                if (!quitSyncProcess)
                {
                    DataExchange(tableName);
                    // 数据更新
                    adapter.UpdateCommand = builder.GetUpdateCommand();
                    try
                    {
                        adapter.Update(_dataSet);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private bool IsRowExisted(DataRow dr, DataTable table)
        {
            int count = 0;
            switch (table.TableName)
            {
                case "Patients":
                    count = (from qs in table.AsEnumerable()
                             where qs.Field<string>(0) == dr.Field<string>(0)
                             select qs).Count();
                    break;
                case "HRTestTypes":
                    count = (from qs in table.AsEnumerable()
                             where qs.Field<int>(0) == dr.Field<int>(0)
                             select qs).Count();
                    break;
                case "HRTstPrmNames":
                    count = (from qs in table.AsEnumerable()
                             where qs.Field<int>(0) == dr.Field<int>(0)
                             && qs.Field<int>(1) == dr.Field<int>(1)
                             && qs.Field<int>(2) == dr.Field<int>(2)
                             select qs).Count();
                    break;
                case "HRTestParms":
                    count = (from qs in table.AsEnumerable()
                             where qs.Field<int>(0) == dr.Field<int>(0)
                             && qs.Field<string>(1) == dr.Field<string>(1)
                             select qs).Count();
                    break;
                case "HRWorkLists":
                    count = (from qs in table.AsEnumerable()
                             where qs.Field<string>(0) == dr.Field<string>(0)
                             && qs.Field<int>(1) == dr.Field<int>(1)
                             select qs).Count();
                    break;
                case "HRRuns":
                    count = (from qs in table.AsEnumerable()
                             where qs.Field<int>(0) == dr.Field<int>(0)
                             && qs.Field<int>(1) == dr.Field<int>(1)
                             select qs).Count();
                    break;
            }

            return count > 0;
        }

        private void DataExchange(string tableName)
        {
            DataTable table = _dataSet.Tables[tableName];
            ParadoxDataReader reader = _pdxConnection.ExecuteQuery(tableName, new RecordQuery.NullCondtion());
            while (reader.Read())
            {
                DataRow row = table.NewRow();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[i] = reader[i];
                }
                // todo 添加数据行前，判断数据是否存在
                if (!IsRowExisted(row, table))
                {
                    table.Rows.Add(row);
                }
            }
        }

        #endregion

        /// <summary>
        /// 获取Curve Sequence Number列表
        /// </summary>
        public List<SimpleRowInfo> CrvSeqNos(int testTypeTag, int mainRunType, string mainTestID)
        {
            return
                (from qs in _dataSet.Tables["HRRuns"].AsEnumerable()
                 where qs.Field<int>("TestTypeTag") == testTypeTag
                 && qs.Field<int>("RunType") == mainRunType
                 && qs.Field<string>("ProcTag") == mainTestID
                 select new SimpleRowInfo
                 {
                     ID = "",
                     Tag = qs.Field<int>("CrvSeqNum"),
                 }).ToList();
        }

        private double FloatPS(double val, int res)
        {
            return Double.Parse(val.ToString("F" + res.ToString()));
        }

        public IEnumerable<DataRow> GetReportInfos(string title, DataTable table)
        {
            var crvs = from qs in _dataSet.Tables["HRRuns"].AsEnumerable()
                       where qs.Field<string>("PrimID").IndexOf(title) >= 0
                       && qs.Field<int>("WrkLstNum") > 0
                       select qs;

            foreach(var crv in crvs)
            {
                DataRow row = table.NewRow();
                int selectedProcTag = crv.Field<int>("ProcTag");
                string selectedPrimID = crv.Field<string>("PrimID");
                var procs = from qs in _dataSet.Tables["HRTestParms"].AsEnumerable()
                            where qs.Field<int>("ProcTag") == selectedProcTag
                            select qs;
                
                string procID = "";
                string abbrev = "";
                if (procs.Count() > 0)
                {
                    var procInfo = procs.First();
                    procID = procInfo.Field<string>("ProcID");
                    abbrev = procInfo.Field<string>("Abbrev");
                }

                string patientName = "";
                var patients = from qs in _dataSet.Tables["Patients"].AsEnumerable()
                               where qs.Field<string>("PrimID") == selectedPrimID
                            select qs.Field<string>(1);

                if (patients.Count() > 0)
                {
                    patientName = patients.First();
                }

                double ppp = ((double)(crv.Field<int>("InitialMin") - crv.Field<int>("ScaleSet1Rd"))) * crv.Field<double>("ScaleSetResult");
                double prp = ((double)(crv.Field<int>("InitialMax") - crv.Field<int>("ScaleSet1Rd"))) * crv.Field<double>("ScaleSetResult");
                double maxPercent = ((double)(crv.Field<int>("MaxPCPoint") - crv.Field<int>("InitialMax"))) / ((double)(crv.Field<int>("InitialMin") - crv.Field<int>("InitialMax")));
                //yield return new ReportInfo
                //{
                //    PrimID = crv.Field<string>("PrimID"),
                //    ChnlNum = crv.Field<int>("ChnlNum") + 1,
                //    StartTime = crv.Field<DateTime>("StartTime"),
                //    ProcID = procID,
                //    Abbrev = abbrev,
                //    Unit = crv.Field<string>("Unit"),
                //    Conc = crv.Field<double>("Conc"),
                //    PRP = FloatPS(prp, 3),
                //    PPP = FloatPS(ppp, 3),
                //    MaxPercent = FloatPS(maxPercent * 100, 1),
                //    MaxPCTime = FloatPS((double)crv.Field<int>("MaxPCTime") / 1000.0, 0),
                //    LagTime = FloatPS((double)crv.Field<int>("LagTime") / 1000.0, 1),
                //    Slope = FloatPS(crv.Field<double>("Slope"), 1)
                //};

                row["PrimID"] = crv.Field<string>("PrimID");
                row["PatientName"] = patientName;
                row["ChnlNum"] = crv.Field<int>("ChnlNum") + 1;
                row["StartTime"] = crv.Field<DateTime>("StartTime");
                row["ProcID"] = procID;
                row["Abbrev"] = abbrev;
                row["Unit"] = crv.Field<string>("Unit");
                row["Conc"] = crv.Field<double>("Conc");
                row["PRP"] = FloatPS(prp, 3);
                row["PPP"] = FloatPS(ppp, 3);
                row["MaxPercent"] = FloatPS(maxPercent * 100, 1);
                row["MaxPCTime"] = FloatPS((double)crv.Field<int>("MaxPCTime") / 1000.0, 0);
                row["LagTime"] = FloatPS((double)crv.Field<int>("LagTime") / 1000.0, 1);
                row["Slope"] = FloatPS(crv.Field<double>("Slope"), 1);

                yield return row;
            }
        }

        public int GetCurves(string primID, int crvSeqNo, string worklistID, int worklistNumber)
        {
            ResultDataSet.Tables.Clear();
            OrderedEnumerableRowCollection<DataRow> selectedRows = null;

            if (crvSeqNo > 0)
            {
                //var condition = new ParadoxCondition.Compare(ParadoxCompareOperator.Equal, mainTestID, 8, 0);
                //QueryArgs q = new QueryArgs("HRRuns", condition, "CrvSeqNum", "");
                //return GetSimpleRowInfos(q, false);
            }

            if (!String.IsNullOrEmpty(primID) && worklistNumber > 0)
            {
                selectedRows = from qs in _dataSet.Tables["HRRuns"].AsEnumerable()
                                   where qs.Field<string>("PrimID") == primID &&
                                   qs.Field<int>("WrkLstNum") == worklistNumber
                                   orderby qs.Field<int>("ChnlNum") ascending
                                   select qs;
            }

            if (!String.IsNullOrEmpty(worklistID) && worklistNumber > 0)
            {
                selectedRows = from qs in _dataSet.Tables["HRRuns"].AsEnumerable()
                               where qs.Field<int>("WrkLstNum") == worklistNumber
                               && qs.Field<string>("WrkLstID") == worklistID
                               orderby qs.Field<int>("ChnlNum") ascending
                               select qs;                
            }

            if (selectedRows != null && selectedRows.Count() > 0)
            {
                // 创建结果报告集
                DataTable reportTable = CreateReportTable();
                ResultDataSet.Tables.Add(reportTable);

                // 创建病人信息结果集
                DataTable patientTable = CreatePatientTable();
                ResultDataSet.Tables.Add(patientTable);

                foreach (var sr in selectedRows)
                {
                    // 创建曲线点信息
                    int lblChnNum = sr.Field<int>("ChnlNum") + 1;
                    string label = "Channel " + lblChnNum.ToString();
                    AggRamCurve crv = new AggRamCurve(
                        label,
                        (int)sr["InitialMax"], (int)sr["InitialMin"],
                        (int)sr["ScaleSet1Rd"], (int)sr["MaxPCPoint"], (double)sr["ScaleSetResult"],
                        (int)sr["DataPoints"], (byte[])sr["Data"]);

                    // Add Curve
                    if (OnAddCurve != null)
                    {
                        OnAddCurve(label, crv);
                        //OnAddCurve(label, CreateCurve((int)rdr["DataPoints"], (byte[])rdr["Data"]));
                    }

                    int selectedProcTag = sr.Field<int>("ProcTag");
                    var procs = from qs in _dataSet.Tables["HRTestParms"].AsEnumerable()
                                where qs.Field<int>("ProcTag") == selectedProcTag
                                select qs;
                    // Add Report Row
                    DataRow reportRow = reportTable.NewRow();
                    reportRow["ChnlNum"] = lblChnNum;
                    reportRow["StartTime"] = sr.Field<DateTime>("StartTime");
                    if (procs.Count() > 0)
                    {
                        var procInfo = procs.First();
                        reportRow["ProcID"] = procInfo.Field<string>("ProcID");
                        reportRow["Abbrev"] = procInfo.Field<string>("Abbrev");
                    }
                    reportRow["Unit"] = sr.Field<string>("Unit");
                    reportRow["Conc"] = sr.Field<double>("Conc");
                    reportRow["PRP"] = FloatPS(crv.PRP, 3);
                    reportRow["PPP"] = FloatPS(crv.PPP, 3);
                    reportRow["MaxPercent"] = FloatPS(crv.MaxPercent * 100, 1);
                    reportRow["MaxPCTime"] = FloatPS((double)sr.Field<int>("MaxPCTime") / 1000.0, 0);
                    reportRow["LagTime"] = FloatPS((double)sr.Field<int>("LagTime") / 1000.0, 1);
                    reportRow["Slope"] = FloatPS(sr.Field<double>("Slope"), 1);
                    reportTable.Rows.Add(reportRow);

                    // Add Patient Row
                    DataRow row = patientTable.NewRow();
                    row[0] = lblChnNum;
                    string selectedPrimID = sr.Field<string>("PrimID");
                    var patientInfos = from qs in _dataSet.Tables["Patients"].AsEnumerable()
                                       where qs.Field<string>("PrimID") == selectedPrimID
                                       select qs;

                    if (patientInfos.Count() > 0)
                    {
                        var patientInfo = patientInfos.First();
                        object[] array = (patientInfo.ItemArray.Take(patientTable.Columns.Count - 1)).ToArray();
                        for (int i = 0; i < patientTable.Columns.Count - 1; i++)
                        {
                            row[i + 1] = array[i];
                        }
                    }
                    else
                    {
                        row[1] = selectedPrimID;
                    }
                    patientTable.Rows.Add(row);
                }
                return selectedRows.Count();
            }

            return 0;
        }
    }
}
