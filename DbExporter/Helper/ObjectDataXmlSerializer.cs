using System;
using System.IO;
using System.Xml.Serialization;

namespace DbExporter.Helper
{
    public class OjbectDataXmlSerializer
    {
        public static void Save(object obj, string fileName)
        {
            XmlSerializer ser = null;

            try
            {
                ser = new XmlSerializer(obj.GetType());
                using (TextWriter tw = new StreamWriter(fileName))
                {
                    ser.Serialize(tw, obj);
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception(String.Format("保存配置 '{0}' 到文件 {1} 失败！", 
                    obj.GetType().ToString(), fileName), ex);
            }
        }

        public static object Load(Type type, string fileName)
        {
            if (!File.Exists(fileName)) return null;

            object obj = null;
            try
            {
                XmlSerializer ser = new XmlSerializer(type);

                using (TextReader tr = new StreamReader(fileName))
                {
                    obj = ser.Deserialize(tr);
                    tr.Close();
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception(String.Format("读取文件 '{1}' to {0} 失败！",
                    type.ToString(), fileName), ex);
            }
            return obj;
        }
    }
}
