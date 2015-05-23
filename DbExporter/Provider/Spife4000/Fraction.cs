using System;
using System.Collections;
using System.Collections.Generic;

namespace DbExporter.Provider.Spife4000
{
    public class Fraction : IEnumerable
    {
        List<FractionPoint> m_fractions = new List<FractionPoint>();

        public int PointCount
        {
            get { return m_fractions.Count; }
        }

        public Fraction()
        {
        }

        public void AddPoint(FractionPoint pt)
        {
            m_fractions.Add(pt);
        }

        #region 获取分区点位置

        /// <summary>
        /// 获取最后一个分区点位置
        /// </summary>
        /// <returns></returns>
        public int GetLastFraction()
        {
            if (m_fractions.Count <= 0)
            {
                return 0;
            }

            return m_fractions[m_fractions.Count - 1].Z;
        }
        #endregion

        #region Read/Write With DB
        /// <summary>
        /// 数据库存储
        /// </summary>
        /// <param name="pts"></param>
        public void FromString(string pts)
        {
            m_fractions.Clear();

            string[] ptArray = pts.Split(new string[] { "," },
                StringSplitOptions.RemoveEmptyEntries);
            //
            int ptCnt = ptArray.Length / 3;
            int readTimes = 0;
            for (int i = 0; i < ptArray.Length && readTimes < ptCnt; i += 3)
            {
                FractionPoint pt = new FractionPoint();
                pt.X = System.Convert.ToInt32(ptArray[i]);
                pt.Y = System.Convert.ToInt32(ptArray[i+1]);
                pt.Z = System.Convert.ToInt32(ptArray[i+2]);
                AddPoint(pt);

                readTimes++;
            }
        }

        public override string ToString()
        {
            List<string> ptList = new List<string>();
            foreach (FractionPoint v in m_fractions)
            {
                ptList.Add(v.ToString());
            }
            return string.Join(",", ptList.ToArray());
        }

        #endregion

        #region IEnumerable 成员

        public IEnumerator GetEnumerator()
        {
            foreach (FractionPoint fp in m_fractions)
            {
                yield return fp.Z;
            }
        }

        #endregion
    }
}
