using System;
using System.Collections.Generic;
using System.IO;
using DbExporter.Provider.Aggram.ParadoxReader;
using System.Collections;

namespace DbExporter.Provider.Aggram
{
    /// <summary>
    /// Paradox数据库连接类
    /// </summary>
    class ParadoxConnection 
    {
        /// <summary>
        /// 数据库目录路径
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Connection Cache
        /// </summary>
        Hashtable _ParadoxTableCache = new Hashtable();

        public ParadoxConnection(string dbPath)
        {
            ConnectionString = dbPath;
        }

        public bool IsOpen()
        {
            // 数据库路径不为空，同时是有效路径
            return !String.IsNullOrEmpty(ConnectionString) && Directory.Exists(ConnectionString);
        }

        /// <summary>
        /// 获取数据库表连接
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private ParadoxTable GetParadoxTable(string tableName)
        {
            ParadoxTable table = (ParadoxTable)_ParadoxTableCache[tableName];
            if (table == null)
            {
                table = new ParadoxTable(ConnectionString, tableName);
                _ParadoxTableCache[tableName] = table;
            }

            return table;
        }

        /// <summary>
        /// 获取数据库游标
        /// </summary>
        /// <param name="tableName">要查询的数据库表名称</param>
        /// <param name="where">数据库查询条件</param>
        /// <returns></returns>
        public ParadoxDataReader ExecuteQuery(string tableName, ParadoxCondition where)
        {
            return ExecuteQuery(tableName, where, true);
        }

        /// <summary>
        /// 获取数据库游标
        /// </summary>
        /// <param name="tableName">要查询的数据库表名称</param>
        /// <param name="where">数据库查询条件</param>
        /// <param name="useIndex">是否使用数据库索引</param>
        /// <returns></returns>
        public ParadoxDataReader ExecuteQuery(string tableName, ParadoxCondition where, bool useIndex)
        {
            var table = GetParadoxTable(tableName);
            IEnumerable<ParadoxRecord> qry = null;
            if (useIndex)
            {
                var index = table.PrimaryKeyIndex; // index
                qry = index.Enumerate(where); // query
            }
            else
            {
                qry = table.Enumerate(where); // query
            }
            return new ParadoxDataReader(table, qry); // reader
        }
    }
}
