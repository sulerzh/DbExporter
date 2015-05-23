using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml.Serialization;

namespace DbExporter.Provider.Aggram
{
    [Serializable]
    public class AggRamCurve : IEnumerable
    {
        private string _label;
        private int _zeroODPoint;
        private int _maxPCPoint;
        private int _initialMax;
        private int _initialMin;
        private double _pointUnit;
        private int _count;
        private List<CurvePoint> _curvePoints;

        /// <summary>
        /// 曲线标签只
        /// </summary>
        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }

        /// <summary>
        /// OD0点点值
        /// </summary>
        public int ZeroODPoint
        {
            get { return _zeroODPoint; }
            set { _zeroODPoint = value; }
        }

        /// <summary>
        /// 最大PC点值
        /// </summary>
        public int MaxPCPoint
        {
            get { return _maxPCPoint; }
            set { _maxPCPoint = value; }
        }

        /// <summary>
        /// 点单位，大约为1.64e-9
        /// </summary>
        public double PointUnit
        {
            get { return _pointUnit; }
            set { _pointUnit = value; }
        }

        /// <summary>
        /// 点数
        /// </summary>
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        /// <summary>
        /// 点数组
        /// </summary>
        [XmlArrayItem("Point", typeof(CurvePoint))]
        [XmlArray("CurvePoints")]
        public List<CurvePoint> CurvePoints
        {
            get { return _curvePoints; }
            set { _curvePoints = value; }
        }

        /// <summary>
        /// Max%
        /// </summary>
        public double MaxPercent
        {
            get 
            {
                return ((double)(_maxPCPoint - _initialMax)) / ((double)(_initialMin - _initialMax));
            }
        }

        /// <summary>
        /// PPP
        /// </summary>
        public double PPP
        {
            get 
            {
                return ((double)(_initialMin - _zeroODPoint)) * _pointUnit;
            }
        }

        /// <summary>
        /// PRP
        /// </summary>
        public double PRP
        {
            get
            {
                return ((double)(_initialMax - _zeroODPoint)) * _pointUnit;
            }
        }

        public AggRamCurve(string label,
            int initialMax, int initialMin,
            int zeroODPoint, int maxPCPoint, double pointUnit, 
            int count, byte[] data)
        {
            _label = label;
            _initialMax = initialMax;
            _initialMin = initialMin;
            _zeroODPoint = zeroODPoint;
            _maxPCPoint = maxPCPoint;
            _pointUnit = pointUnit;
            _count = count;

            Create(count, data, true);
        }

        private void Create(int count, byte[] data, bool inverse)
        {
            System.Diagnostics.Debug.Assert(count == data.Length / 8);

            _curvePoints = new List<CurvePoint>();
            for (int i = 0; i < count; i++)
            {
                byte[] buffer = new byte[8];
                Buffer.BlockCopy(data, i * 8, buffer, 0, 8);
                CurvePoint crvPt = new CurvePoint(buffer);
                crvPt.ODValue = ((double)(crvPt.PointValue - _zeroODPoint)) * _pointUnit;
                if (inverse)
                {
                    crvPt.Percent = ((double)(crvPt.PointValue - _initialMax)) / ((double)(_initialMin - _initialMax));
                }
                else
                {
                    crvPt.Percent = ((double)(crvPt.PointValue - _initialMin)) / ((double)(_initialMax - _initialMin));
                }
                _curvePoints.Add(crvPt);
            }
        }

        #region IEnumerable 成员

        public IEnumerator GetEnumerator()
        {
            return CurvePoints.GetEnumerator();
        }

        #endregion
    }

    [Serializable]
    public class CurvePoint
    {
        private uint _pointValue;
        private double _ODValue;
        private double _percent;
        private uint _relativeTimeMS;

        /// <summary>
        /// 点值
        /// </summary>
        public uint PointValue
        {
            get { return _pointValue; }
            set { _pointValue = value; }
        }
        
        /// <summary>
        /// 透光度
        /// </summary>
        public double ODValue
        {
            get { return _ODValue; }
            set { _ODValue = value; }
        }
        
        /// <summary>
        /// 百分比
        /// </summary>
        public double Percent
        {
            get { return _percent; }
            set { _percent = value; }
        }

        /// <summary>
        /// 相对时间，单位毫秒
        /// </summary>
        public uint RelativeTimeMS
        {
            get { return _relativeTimeMS; }
            set { _relativeTimeMS = value; }
        }

        public CurvePoint()
        { }

        public CurvePoint(byte[] data)
        {
            Parse(data);
        }

        private void Parse(byte[] data)
        {
            _pointValue = BitConverter.ToUInt32(data, 0);
            _relativeTimeMS = BitConverter.ToUInt32(data, 4);
        }
    }
}
