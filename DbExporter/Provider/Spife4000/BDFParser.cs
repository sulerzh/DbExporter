using System;
using System.IO;

namespace DbExporter.Provider.Spife4000
{
    public class BdfParser
    {
        private static string GetString(BinaryReader br)
        {
            int lenth = br.ReadInt32();
            byte[] bytes = new byte[2 * lenth];
            br.Read(bytes, 0, 2 * lenth);
            return System.Text.Encoding.Unicode.GetString(bytes, 0, 2 * (lenth - 1));
        }

        private static BasicInfo GetBasicInfo(BinaryReader br)
        {
            BasicInfo ret = new BasicInfo();

            //Read from the file and store the values to the variables
            ret.Identifier = GetString(br);
            ret.Test = GetString(br);
            ret.Gel_Identifier = GetString(br);
            ret.Scanned_User = GetString(br);
            ret.Rescanned_User = GetString(br);
            ret.Rescanned = GetString(br);
            ret.Reviewed_User = GetString(br);
            ret.Reviewed = GetString(br);
            ret.Edited_User = GetString(br);
            ret.Edited = GetString(br);
            ret.Comments = GetString(br);
            ret.Unused = GetString(br);

            return ret;
        }

        private static Curve GetCurve(BinaryReader br)
        {
            Curve cv = new Curve();
            int count = br.ReadInt32();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    int x = br.ReadInt32();
                    //pt.y = br.ReadInt32();
                    cv.AddPoint(x);
                }
            }
            return cv;
        }

        private static Fraction GetFraction(BinaryReader br)
        {
            Fraction fra = new Fraction();
            int count = br.ReadInt32();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    FractionPoint pt = new FractionPoint();
                    pt.X = br.ReadInt32();
                    pt.Y = br.ReadInt32();
                    pt.Z = br.ReadInt32();
                    fra.AddPoint(pt);
                }
            }
            return fra;
        }

        private static Spike GetSpike(BinaryReader br)
        {
            Spike spk = new Spike();
            int count = br.ReadInt32();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Block blk = new Block();
                    blk.FractionIndex = br.ReadInt32();
                    blk.StartIndex = br.ReadInt32();
                    blk.EndIndex = br.ReadInt32();
                    spk.AddBlock(blk);
                }
            }
            return spk;
        }

        private static byte[] GetBMPContent(BinaryReader br)
        {
            int total = (int)(br.BaseStream.Length - br.BaseStream.Position);
            byte[] imgContent = new byte[total];
            br.Read(imgContent, 0, total);

            return imgContent;
        }

        public static PatientInfo Parse(string file)
        {
            BinaryReader br = null;
            try
            {
                //Open a FileStream in Read mode
                var fin = new FileStream(file,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

                //Create a BinaryReader from the FileStream
                br = new BinaryReader(fin);

                //Seek to the start of the file
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                var patientInfo = new PatientInfo();
                patientInfo.BasicInfo = GetBasicInfo(br);

                // fix 2014-11-28 reviewtime
                //if (patientInfo.BasicInfo.Reviewed == string.Empty)
                //{
                    //var f = new FileInfo(file);
                    //patientInfo.BasicInfo.ScannedTime = f.CreationTime.ToString("yyyy/MM/dd hh:mm:ss");
                //}
                //else
                //{
                //    patientInfo.BasicInfo.TestTime = patientInfo.BasicInfo.Reviewed;
                //}

                //fix means???
                int Unused1 = br.ReadInt32();
                int Unused2 = br.ReadInt32();
                int Unused3 = br.ReadInt16();
                int Unused4 = br.ReadInt16();
                int Unused5 = br.ReadInt32();

                //read curve data
                patientInfo.Curve1 = GetCurve(br);
                patientInfo.Curve2 = GetCurve(br);

                //fix means???
                double Unused01 = br.ReadDouble();
                double Unused02 = br.ReadDouble();
                double Unused03 = br.ReadDouble();

                //read fraction
                patientInfo.Fraction = GetFraction(br);

                //read spike
                patientInfo.Spike = GetSpike(br);

                //read bmp content
                patientInfo.BMPContent = GetBMPContent(br);

                return patientInfo;
            }
            catch (Exception)
            { }
            finally
            {
                if (br != null)
                {
                    br.Close();
                }
            }
            return null;
        }
    }
}
