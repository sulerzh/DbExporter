using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbExporter.Provider
{
    public class FractionPosition
    {
        public int x { get; set; }
        public int y { get; set; }
        public int blp { get; set; }

        public int bly
        {
            get
            {
                return blp * y / 100;
            }
        }
    }

    public class BaseLineCorrectedCurve
    {
        public RawCurve Raw { get; set; }
        FractionPosition[] fractions = new FractionPosition[10];

        public BaseLineCorrectedCurve() { }

        private int GetFractionIndex(int x)
        {
            for (int i = 1; i < fractions.Count(); i++)
            {
                FractionPosition start = fractions[i - 1];
                FractionPosition end = fractions[i];
                if (start == null || end == null)
                {
                    return -1;
                }
                if (i == 1)
                {
                    if (x >= start.x && x <= end.x)
                    {
                        return i;
                    }
                }
                else
                {
                    if (x > start.x && x <= end.x)
                    {
                        return i;
                    }
                }

            }

            return -1;
        }

        public int GetVal(int x)
        {
            int fraIndex = GetFractionIndex(x);
            if (fraIndex == -1)
            {
                return 0;
            }
            FractionPosition start = fractions[fraIndex - 1];
            FractionPosition end = fractions[fraIndex];
            if(start.x == end.x)
            {
                return 0;
            }
            int rawVal = Raw.GetPoint(x);
            int discount = (end.bly - start.bly) * (x - start.x) / (end.x - start.x) + start.bly;
            return (rawVal > discount) ? (rawVal - discount) : 0;
        }

        public void SetFraction(int index, int x, int percent)
        {
            fractions[index] = new FractionPosition
            {
                x = x,
                y = Raw.GetPoint(x),
                blp = percent
            };
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
                total += GetVal(i);
            }

            return total;
        }

        public double GetFractionTotal()
        {
            return GetFractionTotal(0, Raw.Count - 1);
        }
    }
}
