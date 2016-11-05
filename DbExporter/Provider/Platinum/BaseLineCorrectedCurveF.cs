using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbExporter.Provider.Platinum
{
    public class FractionPositionF
    {
        public int x { get; set; }
        public float y { get; set; }
    }

    public class BaseLineCorrectedCurveF
    {
        public RawCurveF Raw { get; set; }
        FractionPositionF[] fractions = new FractionPositionF[10];

        public float BaseLeft { get; set; }
        public float BaseRight { get; set; }

        public BaseLineCorrectedCurveF() { }

        private int GetFractionIndex(int x)
        {
            for (int i = 1; i < fractions.Count(); i++)
            {
                FractionPositionF start = fractions[i - 1];
                FractionPositionF end = fractions[i];
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

        public float GetVal(int x)
        {
            float rawVal = Raw.GetPoint(x);
            float discount = (BaseRight - BaseLeft) * x / Raw.Count + BaseLeft;
            return (rawVal > discount) ? (rawVal - discount) : 0;
        }

        public void SetFraction(int index, int x)
        {
            fractions[index] = new FractionPositionF
            {
                x = x,
                y = Raw.GetPoint(x)
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
            double result = 0;
            for (int i = 1; i < fractions.Count(); i++)
            {
                FractionPositionF start = fractions[i - 1];
                FractionPositionF end = fractions[i];
                if (start == null || end == null)
                {
                    break;
                }
                if (i == 1)
                {
                    result += GetFractionTotal(start.x, end.x);
                }
                else
                {
                    result += GetFractionTotal(start.x + 1, end.x);
                }
            }
            return result;
        }
    }
}
