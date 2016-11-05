using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbExporter.Provider.Platinum
{
    public class RawCurveF
    {
        private List<float> points = new List<float>();

        public int Count
        {
            get { return points.Count; }
        }

        public float Max
        {
            get
            {
                return points.Max();
            }
        }

        public RawCurveF()
        {
        }

        public RawCurveF(List<float> pts)
        {
            points.AddRange(pts);
        }

        public void AddPoints(List<float> pts)
        {
            points.AddRange(pts);
        }

        public void AddPoint(float pt)
        {
            points.Add(pt);
        }

        public float GetPoint(int idx)
        {
            if (idx >= points.Count)
            {
                return 0;
            }
            return points[idx];
        }


        public override string ToString()
        {
            List<string> ptList = new List<string>();
            foreach (float v in points)
            {
                ptList.Add(v.ToString());
            }
            return string.Join(",", ptList.ToArray());
        }
    }
}
