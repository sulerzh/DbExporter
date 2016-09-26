using System;
using System.Collections.Generic;

namespace DbExporter.Provider.Spife4000
{
    public class PatientInfo
    {
        #region Fields
        //基本信息
        BasicInfo m_basicInfo = null;

        //曲线1数据
        RawCurve m_cv1 = null;
        //曲线2数据
        RawCurve m_cv2 = null;

        //分段点数据
        Fraction m_fra = null;

        //双尖峰区数据
        Spike m_spike = null;

        //条带图数据
        byte[] m_BMPContent = null;



        //各分段区比重数据
        List<double> m_fractions = null;

        List<KeyValuePair<string, double>> m_spikes = null;

        #endregion

        #region Properties

        public BasicInfo BasicInfo
        {
            get
            {
                return m_basicInfo;
            }
            set
            {
                m_basicInfo = value;
            }
        }

        public RawCurve Curve1
        {
            get
            {
                return m_cv1;
            }
            set
            {
                m_cv1 = value;
            }
        }

        public RawCurve Curve2
        {
            get
            {
                return m_cv2;
            }
            set
            {
                m_cv2 = value;
            }
        }

        public Fraction Fraction
        {
            get
            {
                return m_fra;
            }
            set
            {
                m_fra = value;
            }
        }

        public Spike Spike
        {
            get
            {
                return m_spike;
            }
            set
            {
                m_spike = value;
            }
        }

        public byte[] BMPContent
        {
            get
            {
                return m_BMPContent;
            }
            set
            {
                m_BMPContent = value;
            }
        }

        #endregion

        public PatientInfo()
        { }

        #region Public Method

        public string GetProteinName(int index)
        {
            string proteinName = "";
            switch (index)
            {
                case 0:
                    proteinName = "Albumin";
                    break;
                case 1:
                    proteinName = "Alpha1";
                    break;
                case 2:
                    proteinName = "Alpha2";
                    break;
                case 3:
                    proteinName = "Beta";
                    break;
                case 4:
                    proteinName = "Gamma";
                    break;
                default:
                    proteinName = (index + 1).ToString();
                    break;
            }
            return proteinName;
        }

        /// <summary>
        /// 根据分段点位置计算各分区的比重
        /// </summary>
        private void CalculateFraction()
        {
            BaseLineCorrectedCurve correctedCurve = new BaseLineCorrectedCurve { Raw = m_cv1 };
            correctedCurve.SetFraction(0, 0, GlobalConfigVars.BaseLinePercent[0]);
            if (m_fra != null && m_fra.PointCount > 0)
            {
                int i = 1;
                foreach (int currPos in m_fra)
                {
                    correctedCurve.SetFraction(i, currPos, GlobalConfigVars.BaseLinePercent[i]);
                    i++;
                }

                m_fractions = new List<double>();
                //初始值为曲线的第一个点
                int prevPos = 0;
                //计算总值[]
                double total = correctedCurve.GetFractionTotal();
                i = 0;
                foreach (int currPos in m_fra)
                {
                    if (i == 0)
                    {
                        //albumin[]
                        double currFra = correctedCurve.GetFractionTotal(prevPos, currPos);
                        m_fractions.Add(currFra / total);
                    }
                    else
                    {
                        //other(]
                        double currFra = correctedCurve.GetFractionTotal(prevPos + 1, currPos);
                        m_fractions.Add(currFra / total);
                    }
                    prevPos = currPos;
                }

                if (m_spike != null)
                {
                    m_spikes = new List<KeyValuePair<string, double>>();
                    foreach (Block blk in m_spike)
                    {
                        string currSpikeName = GetProteinName(blk.FractionIndex);
                        //m-spike()
                        double currSpike = correctedCurve.GetFractionTotal(blk.StartIndex + 1, blk.EndIndex - 1);
                        m_spikes.Add(new KeyValuePair<string, double>(currSpikeName, currSpike / total));
                    }
                }
            }
        }

        /// <summary>
        /// 计算第fraindex个分区的值
        /// </summary>
        /// <param name="fraIndex"></param>
        /// <returns></returns>
        public double GetFraction(int fraIndex)
        {
            if (m_fractions == null)
            {
                CalculateFraction();
            }

            if (m_fractions.Count > fraIndex)
            {
                return m_fractions[fraIndex];
            }

            return 0f;
        }

        /// <summary>
        /// 计算白蛋白的比重
        /// </summary>
        /// <returns></returns>
        public double GetAlbumin()
        {
            return GetFraction(0);
        }

        /// <summary>
        /// 计算alpha1的比重
        /// </summary>
        /// <returns></returns>
        public double GetAlpha1()
        {
            return GetFraction(1);
        }

        public double GetAlpha2()
        {
            return GetFraction(2);
        }

        public double GetBeta()
        {
            return GetFraction(3);
        }

        public double GetGamma()
        {
            return GetFraction(4);
        }

        public double GetRest()
        {
            double ret = 0f;

            if (m_fractions == null)
            {
                CalculateFraction();
            }

            if (m_fractions.Count > 5)
            {
                for (int i = 5; i < m_fractions.Count; i++)
                {
                    ret += m_fractions[i];
                }
            }
            //return m_fractions[5];
            return ret;
        }

        /// <summary>
        /// 计算所有球蛋白所占的比重
        /// </summary>
        /// <returns></returns>
        private double GetAllFractionExceptAlbmin()
        {
            double ret = 0f;
            if (m_fractions == null)
            {
                CalculateFraction();
            }

            for (int i = 1; i < m_fractions.Count && i < 5; i++)
            {
                ret += m_fractions[i];
            }

            return ret;
        }

        /// <summary>
        /// 计算白蛋白与球蛋白的比值
        /// </summary>
        /// <returns></returns>
        public double GetRatioAG()
        {
            if (m_fractions == null)
            {
                CalculateFraction();
            }

            if (m_fractions.Count > 1)
            {
                return GetAlbumin() / GetAllFractionExceptAlbmin();
            }
            return 0f;
        }

        #endregion


        /// <summary>
        /// 计算第spkIndex个m-spike的值
        /// </summary>
        /// <param name="fraIndex"></param>
        /// <returns></returns>
        public KeyValuePair<string, double> GetSpike(int spkIndex)
        {
            if (m_spikes == null)
            {
                CalculateFraction();
            }

            if (m_spikes.Count > spkIndex)
            {
                return m_spikes[spkIndex];
            }

            return new KeyValuePair<string, double>("", 0f);
        }

        public string Base64Image
        {
            get { return ImageToBase64(this.m_BMPContent); }
        }

        private static byte[] Base64ToImage(string base64)
        {
            if (string.IsNullOrEmpty(base64))
            {
                return null;
            }
            return Convert.FromBase64String(base64);
        }

        private static string ImageToBase64(byte[] bitmap)
        {
            if (bitmap == null)
            {
                return string.Empty;
            }
            return Convert.ToBase64String(bitmap);
        }
    }
}
