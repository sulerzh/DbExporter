﻿using DbExporter.Common;
using System;
using System.IO;
using System.Windows.Forms;

namespace DbExporter
{
    /// <summary>
    /// 全局配置类
    /// </summary>
    public class GlobalConfigVars
    {
        public static SupportedDbType GetDbType(string dbType)
        {
            SupportedDbType dbModel = SupportedDbType.Spife4000;
            try
            {
                dbModel = (SupportedDbType)Enum.Parse(typeof(SupportedDbType), dbType);
            }
            catch
            { }
            return dbModel;
        }

        private static bool ValidateDbType(SupportedDbType dbType)
        {
            if (dbType == SupportedDbType.None)
            {
                MessageBox.Show(
                            "请设置支持的数据库类型！",
                            "软件设置无效",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private static bool ValidateEmptyPath(string path)
        {
            // 为空
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show(
                        "文件路径没有设置！",
                        "软件设置无效",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private static bool ValidateExportPath(string expPath)
        {
            if (!ValidateEmptyPath(expPath))
            {
                return false;
            }
            // 不存在则新建
            if (!Directory.Exists(expPath))
            {
                try
                {
                    Directory.CreateDirectory(Properties.Settings.Default.XmlPath);
                }
                catch (Exception)
                {
                    MessageBox.Show(
                        string.Format("创建导出目录失败！\n请检查导出目录\"{0}\"的设置！", Properties.Settings.Default.XmlPath),
                        "软件设置无效",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private static bool ValidateDbPath(SupportedDbType dbType, string dbPath)
        {
            if (!ValidateEmptyPath(dbPath))
            {
                return false;
            }

            if (dbType != SupportedDbType.Platinum)
            {
                return Directory.Exists(dbPath);
            }
            return File.Exists(dbPath + "\\platinum.mdb");
        }

        public static bool IsWellSetting()
        {
            SupportedDbType dbType = GetDbType(Properties.Settings.Default.DbType);
            if (!ValidateDbType(dbType))
            {
                return false;
            }
            if (!ValidateExportPath(Properties.Settings.Default.XmlPath))
            {
                return false;
            }
            return ValidateDbPath(dbType, Properties.Settings.Default.DbPath);
        }

        public static bool IsWellSetting(SupportedDbType dbType, string dbPath, string expPath)
        {
            if (!ValidateDbType(dbType))
            {
                return false;
            }
            if (!ValidateExportPath(expPath))
            {
                return false;
            }
            return ValidateDbPath(dbType, dbPath);
        }

        public static void SaveSetting(string strDbType, string dbPath, string expPath, int[] blps)
        {
            Properties.Settings.Default.DbType = strDbType;
            Properties.Settings.Default.DbPath = dbPath;
            Properties.Settings.Default.XmlPath = expPath;
            Properties.Settings.Default.Blp0 = blps[0];
            Properties.Settings.Default.Blp1 = blps[1];
            Properties.Settings.Default.Blp2 = blps[2];
            Properties.Settings.Default.Blp3 = blps[3];
            Properties.Settings.Default.Blp4 = blps[4];
            Properties.Settings.Default.Blp5 = blps[5];
            Properties.Settings.Default.Save();
        }

        public static string DbType
        {
            get { return Properties.Settings.Default.DbType; }
        }

        public static string DbPath
        {
            get
            {
                SupportedDbType dbType = GetDbType(Properties.Settings.Default.DbType);
                if (dbType == SupportedDbType.Platinum)
                {
                    return Properties.Settings.Default.DbPath + "\\platinum.mdb";
                }
                return Properties.Settings.Default.DbPath;
            }
        }

        public static string XmlPath
        {
            get { return Properties.Settings.Default.XmlPath; }
        }

        public static int[] BaseLinePercent
        {

            get
            {
                return new int[6]
                {
                    Properties.Settings.Default.Blp0,
                    Properties.Settings.Default.Blp1,
                    Properties.Settings.Default.Blp2,
                    Properties.Settings.Default.Blp3,
                    Properties.Settings.Default.Blp4,
                    Properties.Settings.Default.Blp5
                };
            }
        }

        public static IDbProvider GetDbProvider()
        {
            SupportedDbType dbType = GetDbType(Properties.Settings.Default.DbType);
            return DbProviderFactory.Create(dbType);
        }

        public static IExporter GetExporter()
        {
            SupportedDbType dbType = GetDbType(Properties.Settings.Default.DbType);
            return ExporterFactory.Create(dbType);
        }
    }
}
