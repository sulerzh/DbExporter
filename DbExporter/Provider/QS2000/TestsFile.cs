using System;
using System.Collections.Generic;

namespace DbExporter.Provider.QS2000
{
    public class TestIdentity
    {
        public string Name;// SPE-18 OR IFE(21)
        public string Instrument; // 600512515(11)
        public short GelSize; // 18 or 1
        public short StainType; // 0 or 1(酸蓝，酸紫)
    }

    public class DemoIdentity
    {
        public string Label; // (21)
        public byte Type; // 
        public byte ASTMField; // 
    }

    public class Range
    {
        public string Sex; // (2)
        public byte Type;
        public byte AgeLow;
        public byte AgeLowUnits;
        public byte AgeHigh;
        public byte AgeHighUnits;
    }

    public class Fraction
    {
        public string Label; // (13)
        public List<byte> Ratio; // (2)
        public bool bIstd;
        public List<Tuple<double, double>> dPctRange; // (9,2)
        public List<Tuple<double, double>> dUnitsRange; // (9, 2)
    }

    public class Ratio
    {
        public string Label; // (12)
        public List<Tuple<double, double>> dRange; // (9, 2)
    }

    public class TestProperties
    {
        public TestIdentity Identity;
        public bool bIFE; // true or false（1）
        public string Units; // mg/dL(11)
        public string TotalUnits; // g/dL(11)
        public byte AutoEditOptions; // 0
        public short Precision; // 1
        public short UnitsFullScale; // 0
        public short ImageContrast; // 0
        public List<DemoIdentity> Demo; // (10)
        public List<Range> RangeSet; // (9)
        public List<Ratio> Ratio; // (2)
        public List<Fraction> Fraction; //(10+1)
        public string RstrctBandLabel; //(14)
        public string Reserved; // 2
        public byte Options;
    }

    public class TestsFileHeader
    {
        public string RecSizeLabel;
        public int RecSize;
        public int Unknown;
    }

    public class TestsFile
    {
        public TestsFileHeader Header;
        public List<TestProperties> Tests;
    }
}
