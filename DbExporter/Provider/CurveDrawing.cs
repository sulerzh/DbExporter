using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DbExporter.Provider
{
    public class CurveDrawing
    {
        private BaseLineCorrectedCurve baseLine;
        public CurveDrawing(RawCurve c)
        {
            baseLine = new BaseLineCorrectedCurve { Raw = c };
        }

        /// <summary>
        /// 构建曲线绘制点数组
        /// </summary>
        /// <param name="rec">曲线绘制区域</param>
        /// <returns>曲线点列表</returns>
        public Point[] GetDrawingPoints(Rectangle rec)
        {
            int count = baseLine.Raw.Count;
            int max = baseLine.Raw.Max;
            Point[] ptList = new Point[count];

            float deltaX = (float)rec.Width / (float)count;
            float zoomFactor = (float)rec.Height / (float)max;
            for (int i = 0; i < count; i++)
            {
                ptList[i].X = rec.Left + (int)((float)i * deltaX);
                ptList[i].Y = rec.Bottom - (int)((float)baseLine.Raw.GetPoint(i) * zoomFactor);
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
            int count = baseLine.Raw.Count;
            int max = baseLine.Raw.Max;
            Point[] ptList = new Point[count];

            float deltaX = (float)rec.Width / (float)count;
            float zoomFactor = (float)rec.Height / (float)max;
            for (int i = 0; i < count; i++)
            {
                ptList[i].X = rec.Left + (int)((float)i * deltaX);
                ptList[i].Y = rec.Bottom - (int)((float)baseLine.GetVal(i) * zoomFactor);
            }
            return ptList;
        }

    }
}
