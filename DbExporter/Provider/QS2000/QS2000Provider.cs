using DbExporter.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Forms;

namespace DbExporter.Provider.QS2000
{
    public class QS2000Provider : IDbProvider
    {
        int ginc = 1;

        public string GetTFile(int seqNum)
        {
            return string.Format(
                "T{0}.fsd",
                seqNum.ToString().PadLeft(7, '0'));
        }

        public string GetDFile(int seqNum)
        {
            return string.Format(
                "D{0}.fsd",
                seqNum.ToString().PadLeft(7, '0'));
        }


        public byte[] ConvertToByteArray(string input)
        {
            return Encoding.ASCII.GetBytes(input);
        }

        public string ConvertToString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes).TrimEnd('\0');
        }

        public string ConvertToGBString(byte[] bytes)
        {
            var gb = Encoding.GetEncoding("gb2312");
            return gb.GetString(bytes).TrimEnd('\0');
        }

        public TestsFile ParseTestsFile(string testsFile)
        {
            TestsFile result = new TestsFile();
            using (var fs = File.OpenRead(testsFile))
            {
                using (var br = new BinaryReader(fs))
                {
                    TestsFileHeader header = new TestsFileHeader();
                    header.RecSizeLabel = ConvertToString(br.ReadBytes(8));
                    header.RecSize = br.ReadInt32();
                    header.Unknown = br.ReadInt32();
                    result.Header = header;

                    result.Tests = new List<TestProperties>();
                    while (fs.Length > fs.Position)
                    {
                        var test = new TestProperties();
                        using (var ms = new MemoryStream(br.ReadBytes(header.RecSize)))
                        {
                            using (var br1 = new BinaryReader(ms))
                            {
                                // 读取TestIdentity
                                var identity = new TestIdentity();
                                // identity.Name = ConvertToString(br1.ReadBytes(21));
                                identity.Name = ConvertToGBString(br1.ReadBytes(21));
                                identity.Instrument = ConvertToString(br1.ReadBytes(11));
                                identity.GelSize = br1.ReadInt16();
                                identity.StainType = br1.ReadInt16();
                                test.Identity = identity;

                                // 
                                test.bIFE = br1.ReadBoolean();
                                test.Units = ConvertToString(br1.ReadBytes(11));
                                test.TotalUnits = ConvertToString(br1.ReadBytes(11));
                                test.AutoEditOptions = br1.ReadByte();
                                test.Precision = br1.ReadInt16();
                                test.UnitsFullScale = br1.ReadInt16();
                                test.ImageContrast = br1.ReadInt16();

                                // 读取DemoIdentity
                                var demos = new List<DemoIdentity>();
                                for (int i = 0; i < 10; i++)
                                {
                                    DemoIdentity d = new DemoIdentity();
                                    d.Label = ConvertToString(br1.ReadBytes(21));
                                    d.Type = br1.ReadByte();
                                    d.ASTMField = br1.ReadByte();
                                    demos.Add(d);
                                }
                                test.Demo = demos;

                                // 读取RangeSet
                                var rs = new List<Range>();
                                for (int i = 0; i < 9; i++)
                                {
                                    Range r = new Range();
                                    r.Sex = ConvertToString(br1.ReadBytes(2));
                                    r.Type = br1.ReadByte();
                                    r.AgeLow = br1.ReadByte();
                                    r.AgeLowUnits = br1.ReadByte();
                                    r.AgeHigh = br1.ReadByte();
                                    r.AgeHighUnits = br1.ReadByte();
                                    rs.Add(r);
                                }
                                test.RangeSet = rs;

                                br1.ReadByte();

                                // 读取Ratio
                                var ratios = new List<Ratio>();
                                for (int i = 0; i < 2; i++)
                                {
                                    Ratio r = new Ratio();
                                    r.Label = Encoding.ASCII.GetString(br1.ReadBytes(12));
                                    r.dRange = new List<KeyValuePair<double, double>>();
                                    for (int j = 0; j < 9; j++)
                                    {
                                        KeyValuePair<double, double> t = new KeyValuePair<double, double>(br1.ReadDouble(), br1.ReadDouble());
                                        r.dRange.Add(t);
                                    }
                                    ratios.Add(r);
                                }
                                test.Ratio = ratios;

                                // 读取Fraction
                                var fractions = new List<Fraction>();
                                for (int i = 0; i < 10 + 1; i++)
                                {
                                    Fraction f = new Fraction();
                                    f.Label = ConvertToString(br1.ReadBytes(13));
                                    f.Ratio = new List<byte>();
                                    f.Ratio.Add(br1.ReadByte());
                                    f.Ratio.Add(br1.ReadByte());
                                    f.bIstd = br1.ReadBoolean();
                                    f.dPctRange = new List<KeyValuePair<double, double>>();
                                    for (int j = 0; j < 9; j++)
                                    {
                                        KeyValuePair<double, double> t = new KeyValuePair<double, double>(br1.ReadDouble(), br1.ReadDouble());
                                        f.dPctRange.Add(t);
                                    }
                                    f.dUnitsRange = new List<KeyValuePair<double, double>>();
                                    for (int j = 0; j < 9; j++)
                                    {
                                        KeyValuePair<double, double> t = new KeyValuePair<double, double>(br1.ReadDouble(), br1.ReadDouble());
                                        f.dUnitsRange.Add(t);
                                    }
                                    fractions.Add(f);
                                }
                                test.Fraction = fractions;

                                // 不读取了
                                //test.RstrctBandLabel = ConvertToString(br1.ReadBytes(14));
                                //test.Reserved = Encoding.ASCII.GetString(br1.ReadBytes(2));
                                //test.Options = br1.ReadByte();
                            }
                        }
                        result.Tests.Add(test);
                    }
                }
            }
            return result;
        }

        public IndexFile ParseIndexFile(string indexFile)
        {
            IndexFile result = new IndexFile();
            using (var fs = File.OpenRead(indexFile))
            {
                using (var br = new BinaryReader(fs))
                {
                    result.Indexes = new List<IndexInfo>();
                    int count = br.ReadInt32();
                    for (int i = 1; i < count; i++)
                    {
                        IndexInfo item = new IndexInfo();
                        item.SeqNum = br.ReadInt32();
                        item.Unknown = br.ReadInt32();
                        item.Name = ConvertToString(br.ReadBytes(48));
                        item.Instrument = ConvertToString(br.ReadBytes(48));
                        item.GelSize = br.ReadInt32();
                        item.Unknown1 = br.ReadInt32();
                        result.Indexes.Add(item);
                    }
                }
            }
            return result;
        }

        public TFile ParseTFile(string file)
        {
            TFile result = new TFile();
            using (var fs = File.OpenRead(file))
            {
                using (var br = new BinaryReader(fs))
                {
                    result.Header = new TFileHeader();
                    // 读取Identity
                    var identity = new TestIdentity();
                    // identity.Name = ConvertToString(br.ReadBytes(21));
                    identity.Name = ConvertToGBString(br.ReadBytes(21)); 
                    identity.Instrument = ConvertToString(br.ReadBytes(11));
                    identity.GelSize = br.ReadInt16();
                    identity.StainType = br.ReadInt16();
                    result.Header.Identity = identity;

                    // skip
                    br.ReadBytes(220);

                    // 读取count数组
                    byte nullValue = 0xFF;
                    byte[] sampleSeqArray = br.ReadBytes(100);
                    result.Header.Count = sampleSeqArray.TakeWhile(i => i != nullValue).ToArray();
                    int sampleCount = result.Header.Count.Length;

                    // 读取数据
                    result.Datas = new List<TFileData>();
                    for (int i = 0; i < sampleCount; i++)
                    {
                        var data = new TFileData();
                        data.Patient = new PatientInfo();
                        for (int p = 0; p < 10; p++)
                        {
                            var demo = br.ReadBytes(21);
                            var demoType = br.ReadByte();
                            if (demoType == 0)
                            {
                                data.Patient.Demographic.Add(ConvertToGBString(demo));
                            }
                            if (demoType == 1)
                            {
                                data.Patient.Demographic.Add(ConvertToString(demo));
                            }
                        }
                        data.csStatus = ConvertToString(br.ReadBytes(21));
                        data.csDate = ConvertToString(br.ReadBytes(20));
                        data.Id = br.ReadByte();
                        data.Name = ConvertToGBString(br.ReadBytes(21));
                        data.Unknown = ConvertToString(br.ReadBytes(3));
                        result.Datas.Add(data);

                    }
                }
            }
            return result;
        }

        public static DateTime ParseDateTime(string csDate)
        {
            return DateTime.ParseExact(csDate, "yyyyMMddHHmmssfff", null);
        }

        public DFile ParseDFile(string file, string gelName, int gelSize)
        {
            int inc = 1;

            DFile result = new DFile();
            using (var fs = File.OpenRead(file))
            {
                using (var br = new BinaryReader(fs))
                {
                    bool isIFE = gelName == "IFE";
                    result.Scans = new List<ScanCommon>();
                    for (int i = 0; i < gelSize; i++)
                    {
                        string name = ConvertToString(br.ReadBytes(8));
                        int gelSize1 = br.ReadInt16();

                        // 跳过ScanCommon信息解析                            
                        br.ReadBytes(842 - 10);
                        //scan.Patient;
                        //scan.Status
                        //scan.DateTimeStam;
                        //scan.SampleNumbe;
                        //scan.TestIdentit;
                        //scan.RunIdentity
                        //scan.Comments
                        //scan.Characteristic
                        //scan.ImageContras;

                        if (isIFE)
                        {
                            var scan = new IFEScan();
                            // 解析units
                            scan.Units = new List<float>();
                            for (int u = 0; u < 9 + 1; u++)
                            {
                                scan.Units.Add(br.ReadSingle());
                            }

                            // 
                            scan.ImageAspectRatio = br.ReadSingle();
                            scan.Image = br.ReadBytes(50400); // 6 * 140 * 20 *3 

                            List<Bitmap> parts = new List<Bitmap>();
                            List<string> bandNames = new List<string> { "SP", "G", "A", "M", "K", "L" };
                            for (int v = 0; v < 6; v++)
                            {
                                var temp = new byte[8400];
                                Array.Copy(scan.Image, v * 8400, temp, 0, 8400);

                                var part = BitmapImageHelper.CreateBitmap(140, 20, temp, true);
                                parts.Add(part);
                            }
                            var bitMap = BitmapImageHelper.FixImageMerge(parts, bandNames);
                            //var outFileName = "E:\\export\\" + ginc + "_" + inc++ + ".bmp";
                            //bitMap.Save(outFileName);

                            scan.DestBytes = BitmapImageHelper.GetBytes(bitMap);

                            result.Scans.Add(scan);
                        }
                        else
                        {
                            var scan = new StdScan();

                            // 解析ScanIdentifier
                            scan.Reference = new List<ScanIdentifier>();
                            for (int s = 0; s < 4; s++)
                            {
                                var reference = new ScanIdentifier();
                                reference.TimeStamp = br.ReadUInt32();
                                reference.SampleNumber = br.ReadByte();
                                reference.Instrument = ConvertToString(br.ReadBytes(11));
                                scan.Reference.Add(reference);
                            }

                            // skip
                            // br.ReadBytes(894);
                            scan.FractionIdentity = br.ReadUInt16();
                            // 解析data
                            scan.Data = new List<short>();
                            int dcount = br.ReadInt16();
                            var pos = 910 + i * 21150;
                            var pos1 = fs.Position;
                            for (int d = 0; d < dcount; d++)
                            {
                                scan.Data.Add(br.ReadInt16());
                            }
                            // skip
                            br.ReadBytes(2090 - (dcount + 1) * 2);
                            // 解析rawdata
                            scan.RawData = new List<ushort>();
                            int rdcount = br.ReadInt16();
                            int rdcount1 = rdcount * 3 / 4;
                            for (int r = 0; r < rdcount1; r++)
                            {
                                scan.RawData.Add(br.ReadUInt16());
                            }
                            // skip
                            br.ReadBytes(1512 - (rdcount1 + 1) * 2);
                            // 解析fraction
                            scan.Fraction = new List<short>();
                            for (int r = 0; r < 10 + 1; r++)
                            {
                                scan.Fraction.Add(br.ReadInt16());
                            }
                            // 解析rawfraction
                            scan.RawFraction = new List<short>();
                            for (int r = 0; r < 10 + 1; r++)
                            {
                                scan.RawFraction.Add(br.ReadInt16());
                            }

                            // union 1
                            br.ReadInt16();

                            // union 2
                            br.ReadBytes(44);

                            //
                            scan.OverlayAdjust = new List<short>();
                            for (int o = 0; o < 16; o++)
                            {
                                scan.OverlayAdjust.Add(br.ReadInt16());
                            }
                            scan.Sensitivity = br.ReadInt16();

                            scan.Amplification = br.ReadSingle();
                            scan.DspScaleFactor = br.ReadSingle();
                            scan.NumScaleFactor = br.ReadSingle();
                            scan.ImageAspectRatio = br.ReadSingle();

                            scan.Image = br.ReadBytes(16500); // 220*25*3

                            //var outFileName = "E:\\export\\" + ginc + "_" + inc++ + ".bmp";
                            //BitmapImageHelper.SaveBitmap(220, 25, scan.Image, outFileName);
                            scan.DestBytes = BitmapImageHelper.GetBytes(220, 25, scan.Image);

                            result.Scans.Add(scan);
                        }

                    }
                }
            }
            ginc++;
            return result;
        }

        public void Parse(string archFolder)
        {
            //string testsFile = GlobalConfigVars.DbPath + "\\" + "Tests.qsd";
            // var test = ParseTestsFile(testsFile);
            string indexFile = GlobalConfigVars.DbPath + "\\" + "Index.fsd";
            var index = ParseIndexFile(indexFile);
            if (index != null && index.Indexes != null)
            {
                foreach (IndexInfo item in index.Indexes)
                {
                    string tfile = GlobalConfigVars.DbPath + "\\" + GetTFile(item.SeqNum);
                    TFile tFile = ParseTFile(tfile);
                    string dfile = GlobalConfigVars.DbPath + "\\" + GetDFile(item.SeqNum);
                    DFile dFile = ParseDFile(dfile, item.Name, item.GelSize);
                }
            }
        }

        public IEnumerable<TFileData> GetTFileData()
        {

            string indexFile = GlobalConfigVars.DbPath + "\\" + "Index.fsd";
            IndexFile index = ParseIndexFile(indexFile);
            if (index != null && index.Indexes != null)
            {
                foreach (IndexInfo item in index.Indexes)
                {
                    string tfile = GlobalConfigVars.DbPath + "\\" + GetTFile(item.SeqNum);
                    TFile tFile = ParseTFile(tfile);
                    foreach (TFileData d in tFile.Datas)
                    {
                        d.SeqNum = item.SeqNum;
                        yield return d;
                    }
                }
            }
        }

        public List<DateTime> GetAllTestDate()
        {
            return GetTFileData().Select(t => t.ScanDate.Date).ToList();
        }

        public List<ShowBase> GetResultByFilterDate(DateTime testDate)
        {
            return GetTFileData().Where(d => d.ScanDate.Date == testDate.Date).Select(r => (ShowBase)r).ToList();
        }

        public Qs2000State GetState(TFileData query)
        {
            string testsFile = GlobalConfigVars.DbPath + "\\" + "Tests.qsd";
            TestsFile test = ParseTestsFile(testsFile);

            string tfile = GlobalConfigVars.DbPath + "\\" + GetTFile(query.SeqNum);
            TFile tFile = ParseTFile(tfile);
            string dfile = GlobalConfigVars.DbPath + "\\" + GetDFile(query.SeqNum);
            DFile dFile = ParseDFile(dfile, tFile.Header.Identity.Name, tFile.Header.Identity.GelSize);
            // 查找配置
            TestProperties tp = test.Tests.Where(t => t.Identity.Equals(tFile.Header.Identity)).First();

            int idx = query.Id - 1;
            Qs2000State state = new Qs2000State
            {
                SeqNum = query.SeqNum.ToString(),
                SampleNum = query.Id.ToString(),
                SampleId = tFile.Datas[idx].Patient.Demographic[0],
                ScannedDate = tFile.Datas[idx].ScanDate.ToString("yyyy/MM/dd HH:mm:ss"),
                Scan = dFile.Scans[idx].DestBytes,
                TestType = tFile.Header.Identity.Name
            };
            if (!tp.bIFE)
            {
                StdScan scan = dFile.Scans[idx] as StdScan;
                state.Result = new Qs2000Result(scan.Data, scan.Fraction, tp.Fraction);
            }
            return state;
        }
    }
}
