using System;
using System.Collections;
using System.Collections.Generic;

namespace DbExporter.Provider.Spife4000
{
    public class Spike : IEnumerable
    {
        List<Block> m_blocks = new List<Block>();

        public int BlockCount
        {
            get { return m_blocks.Count; }
        }

        public Spike()
        {
        }

        public void AddBlock(Block blk)
        {
            m_blocks.Add(blk);
        }

        #region Read/Write With DB
        /// <summary>
        /// 数据库存储
        /// </summary>
        /// <param name="pts"></param>
        public void FromString(string pts)
        {
            m_blocks.Clear();

            string[] ptArray = pts.Split(new string[] { "," },
                StringSplitOptions.RemoveEmptyEntries);
            //
            int ptCnt = ptArray.Length / 3;
            int readTimes = 0;
            for (int i = 0; i < ptArray.Length && readTimes < ptCnt; i += 3)
            {
                Block blk = new Block();
                blk.FractionIndex = System.Convert.ToInt32(ptArray[i]);
                blk.StartIndex = System.Convert.ToInt32(ptArray[i + 1]);
                blk.EndIndex = System.Convert.ToInt32(ptArray[i + 2]);
                AddBlock(blk);

                readTimes++;
            }
        }

        public override string ToString()
        {
            List<string> ptList = new List<string>();
            foreach (Block v in m_blocks)
            {
                ptList.Add(v.ToString());
            }
            return string.Join(",", ptList.ToArray());
        }

        #endregion

        #region IEnumerable 成员

        public IEnumerator GetEnumerator()
        {
            foreach (Block blk in m_blocks)
            {
                yield return blk;
            }
        }

        #endregion

    }
}
