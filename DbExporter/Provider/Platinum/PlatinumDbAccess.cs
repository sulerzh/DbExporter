using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DbExporter.Provider.Platinum
{
    public class PlatinumDbAccess
    {
        public string DbPath { get; set; }

        private static OleDbConnection CreateConnection(string filename)
        {
            string cs = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                     "Data Source=" + filename;
            return new OleDbConnection(cs);
        }

        private static DemoDef GetDemoDefById(int defId)
        {
            using (OleDbConnection conn = CreateConnection(GlobalConfigVars.DbPath))
            {

                using (var cmd = new OleDbCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from DemoDef where DemoDefIdNr = @id";
                    cmd.Parameters.AddWithValue("@id", defId);

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    var reader = cmd.ExecuteReader();

                    using (reader)
                    {
                        if (reader.Read())
                        {
                            DemoDef result = new DemoDef
                            {
                                NumberOfFields = reader.GetInt16(1)
                            };
                            for (int i = 2; i < result.NumberOfFields * 2 + 2; i += 2)
                            {
                                result.AddField(new FieldDef
                                {
                                    FieldType = reader.GetByte(i),
                                    Field = reader.GetString(i + 1)
                                });
                            }
                            return result;
                        }
                    }
                }
            }
            return null;
        }

        public static Demographic GetDemographicById(int id)
        {
            using (OleDbConnection conn = CreateConnection(GlobalConfigVars.DbPath))
            {
                using (var cmd = new OleDbCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from Demographic where DemographicIdNr = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    var reader = cmd.ExecuteReader();

                    using (reader)
                    {
                        if (reader.Read())
                        {
                            Demographic result = new Demographic();
                            int defId = reader.GetInt32(1);
                            result.Def = GetDemoDefById(defId);
                            for (int i = 2; i < result.Def.NumberOfFields + 2; i++)
                            {
                                string v = DBNull.Value == reader[i] ? "" : reader.GetString(i);
                                result.SetValue(i-2, (string) v);
                            }
                            return result;
                        }
                    }
                }
            }
            return null;
        }

        public static List<Peak> GetPeaksByScanId(int scanId)
        {
            List<Peak> result = new List<Peak>();
            using (OleDbConnection conn = CreateConnection(GlobalConfigVars.DbPath))
            {
                using (var cmd = new OleDbCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from Peak where ScanIdNr = @scanId";
                    cmd.Parameters.AddWithValue("@scanId", scanId);
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    var reader = cmd.ExecuteReader();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Peak p = new Peak
                            {
                                Index = reader.GetInt16(reader.GetOrdinal("Index")),
                                Left = reader.GetInt32(reader.GetOrdinal("Left")),
                                Top = reader.GetInt32(reader.GetOrdinal("Top")),
                                Right = reader.GetInt32(reader.GetOrdinal("Right")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MSpike = 1==reader.GetByte(reader.GetOrdinal("MSpike"))
                            };
                            result.Add(p);
                        }
                    }
                }
            }
            return result;
        }

        private void SaveToFile(byte[] b)
        {
            try
            {
                File.WriteAllBytes("D:\\abc.b", b);
            }
            catch (Exception)
            {}
        }

        public static IEnumerable<ScanResult> GetReportInfos(DateTime filterDate)
        {
            using (OleDbConnection conn = CreateConnection(GlobalConfigVars.DbPath))
            {
                using (var cmd = new OleDbCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from Scan where CreateDate >= @date1 and CreateDate < @date2";
                    cmd.Parameters.AddWithValue("@date1", filterDate.Date);
                    cmd.Parameters.AddWithValue("@date2", filterDate.AddDays(1).Date);

                    // test to fetch all record
                    //cmd.CommandText = "select * from Scan";

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    var reader = cmd.ExecuteReader();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            ScanResult result = new ScanResult();
                            result.ScanIdNr = reader.GetInt32(0);
                            result.DemographicIdNr = reader.GetInt32(3);
                            result.GelName = reader.GetString(reader.GetOrdinal("GelName"));
                            result.GelType = reader.GetString(reader.GetOrdinal("GelType"));
                            result.IFE = 1==reader.GetByte(reader.GetOrdinal("IFE"));
                            result.Edited = 1==reader.GetByte(reader.GetOrdinal("Edited"));
                            result.Viewed = 1 == reader.GetByte(reader.GetOrdinal("Viewed"));
                            result.MeasTime = reader.GetDateTime(reader.GetOrdinal("MeasTime"));
                            result.NumData = reader.GetInt32(reader.GetOrdinal("NumData"));                            
                            if (!result.IFE)
                            {
                                if (reader[reader.GetOrdinal("Scan")] != DBNull.Value)
                                {
                                    result.Scan = (byte[]) reader[reader.GetOrdinal("Scan")];
                                }
                                // SaveToFile((byte[])reader[reader.GetOrdinal("Scan")]);
                            }
                            if (reader[reader.GetOrdinal("DIB")] != DBNull.Value)
                            {
                                result.DIB = (byte[])reader[reader.GetOrdinal("DIB")];
                            }
                            result.CreateDate = reader.GetDateTime(reader.GetOrdinal("CreateDate"));
                            result.UpdateDate = reader.GetDateTime(reader.GetOrdinal("UpdateDate"));
                            result.Operator = reader.GetString(reader.GetOrdinal("Operator"));

                            // Patient
                            result.Patient = GetDemographicById(result.DemographicIdNr);
                            yield return result;
                        }
                    }
                }
            }
        }
    }
}
