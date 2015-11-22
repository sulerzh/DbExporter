using System;
using System.Collections.Generic;
using System.Drawing;

namespace DbExporter.Provider.Platinum
{
    public class Curve
    {
        List<int> points = new List<int>();
        public int max = 0;

        public double Slope
        {
            get
            {
                return 0;
            }
        }

        public void AddPoint(int pt)
        {
            points.Add(pt);
            if (pt > max)
            {
                max = pt;
            }
        }

        public int PointCount
        {
            get { return points.Count; }
        }

        public int GetPoint(int idx)
        {
            if (idx >= points.Count)
            {
                return 0;
            }
            return points[idx];
        }

        public int CalculateBaseLineValue(int index)
        {
            if (index >= points.Count) return 0;

            int ret = points[index] - (int)(Slope * (double)index);
            return ret > 0 ? ret : 0;
        }

        /// <summary>
        /// 获取分区值[]大小
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public double GetFractionTotal(int startIndex, int endIndex)
        {
            //System.Diagnostics.Debug.Assert(startIndex >= 0 && startIndex < 200);
            //System.Diagnostics.Debug.Assert(endIndex > 0 && endIndex < 200);
            //System.Diagnostics.Debug.Assert(startIndex <= endIndex);

            double total = 0;
            //用积分方法
            //计算起始点与终点间[startIndex, endIndex)的曲线梯形的面积
            for (int i = startIndex; i <= endIndex; i++)
            {
                total += CalculateBaseLineValue(i);
            }

            return total;
        }

        /// <summary>
        /// 构建曲线绘制点数组
        /// </summary>
        /// <param name="rec">曲线绘制区域</param>
        /// <returns>曲线点列表</returns>
        public Point[] GetDrawingPoints(Rectangle rec)
        {
            int count = points.Count;
            Point[] ptList = new Point[count];

            float deltaX = (float)rec.Width / (float)count;
            float zoomFactor = (float)rec.Height / (float)max;
            for (int i = 0; i < count; i++)
            {
                ptList[i].X = rec.Left + (int)((float)i * deltaX);
                ptList[i].Y = rec.Bottom - (int)((float)this.GetPoint(i) * zoomFactor);
            }
            return ptList;
        }

        /// <summary>
        /// 构建曲线绘制点数组
        /// </summary>
        /// <param name="rec">曲线绘制区域</param>
        /// <returns>曲线点列表</returns>
        public Point[] GetCorrectedDrawingPoints(Rectangle rec)
        {
            int count = points.Count;
            Point[] ptList = new Point[count];

            float deltaX = (float)rec.Width / (float)count;
            float zoomFactor = (float)rec.Height / (float)max;
            for (int i = 0; i < count; i++)
            {
                ptList[i].X = rec.Left + (int)((float)i * deltaX);
                ptList[i].Y = rec.Bottom - (int)((float)CalculateBaseLineValue(i) * zoomFactor);
            }
            return ptList;
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
