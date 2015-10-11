using System.Collections.Generic;

namespace DbExporter.Provider.QS2000
{
    public class PatientInfo
    {
        public List<string> Demographic = new List<string>(); // (10 * 22)
    }

    // sizeof(tScanCommon) = 842
    public class ScanCommon
    {
        public PatientInfo  Patient;
        public string       Status; //ASZSCANSTATUS
        public string       DateTimeStamp;//ASZDATETIMESCANNED
        public byte         SampleNumber;
        public TestIdentity TestIdentity;
        public string       RunIdentity;//ASZRUNID
        public string       Comments;//SZCOMMENTS
        public string       Characteristic;//ASZPATTERNCHAR
        public short        ImageContrast;
    }

    public class ScanIdentifier
    {
        public uint TimeStamp;
        public byte SampleNumber;
        public string Instrument; // (11)
    }

    // 扫描记录, 21150
    public class StdScan : ScanCommon
    {
        public List<ScanIdentifier> Reference;  // (4)
        // 共 906 个字节 
        // SKIP 894个字节
        public ushort               FractionIdentity;  // -1, FF FF
        public List<short>                Data; // (172)
                           // SKIP 1744 个字节
        public List<ushort>               RawData; // [130]
                               // SKIP 1252个字节
        public List<short>                Fraction; // (10)
        public List<short>                RawFraction; // (10)

        // union
        public short                RstrctBandCount;
        //short Reserved;

        // union
        //sbyte FracModulation; // (10)
        public short[]              RstrctBand; // (11)

        public List<short> OverlayAdjust; // (16)
        public short Sensitivity; // 0
        public float Amplification; // 2.973837
        public float DspScaleFactor; // 1
        public float NumScaleFactor;// 1
        public float ImageAspectRatio; // 3.51
        public byte[] Image; //SZIMAGEX * SZIMAGEY * 3
    }

    public class IFEScan : ScanCommon
    {
        public List<float> Units; // (9)
        public float ImageAspectRatio; // 7.6667
        public byte[] Image; // 3
    }

    public class DFile
    {
        public List<ScanCommon> Scans;
    }
}
