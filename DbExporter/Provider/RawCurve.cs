using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DbExporter.Provider
{
    public class RawCurve
    {
        private List<int> points = new List<int>();

        public int Count
        {
            get { return points.Count; }
        }

        public int Max
        {
            get
            {
                return points.Max();
            }
        }

        public RawCurve()
        {
        }

        public RawCurve(List<int> pts)
        {
            points.AddRange(pts);
        }

        public void AddPoints(List<int> pts)
        {
            points.AddRange(pts);
        }

        public void AddPoint(int pt)
        {
            points.Add(pt);
        }

        public int GetPoint(int idx)
        {
            if (idx >= points.Count)
            {
                return 0;
            }
            return points[idx];
        }

        /// <summary>
        /// 数据库存储
        /// </summary>
        /// <param name="pts"></param>
        public void FromString(string pts)
        {
            points.Clear();
            string[] ptArray = pts.Split(new string[] { "," },
                StringSplitOptions.RemoveEmptyEntries);

            foreach (string v in ptArray)
            {
                int iv = 0;
                Int32.TryParse(v, out iv);
                AddPoint(iv);
            }
        }

        public override string ToString()
        {
            List<string> ptList = new List<string>();
            foreach (int v in points)
            {
                ptList.Add(v.ToString());
            }
            return string.Join(",", ptList.ToArray());
        }
    }
}
