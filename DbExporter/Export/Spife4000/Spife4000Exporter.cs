using DbExporter.Common;
using DbExporter.Helper;
using DbExporter.Provider.Spife4000;
using System.Collections.Generic;

namespace DbExporter.Export.Spife4000
{
    public class Spife4000Exporter : IExporter
    {
        public void Export(List<ShowBase> selectedItems)
        {
            foreach (TdfInfo tdfInfo in selectedItems)
            {
                var sampleInfo = Spife4000DbProvider.GetSampleInfo(tdfInfo);
                if (sampleInfo == null) continue;

                var state = new SpifeDbState();
                state.GelId = tdfInfo.GelId;
                state.SampleNum = tdfInfo.SampleNum;
                state.ScannedDate = sampleInfo.BasicInfo.ScannedTime.ToString("yyyy/MM/dd HH:mm:ss");
                state.SampleId = tdfInfo.SampleId;
                if (sampleInfo.BasicInfo.Test.Contains("Proteins"))
                {
                    List<MSpike> mSpikes = new List<MSpike>();
                    for (int i = 0; i < sampleInfo.Spike.BlockCount; i++)
                    {
                        KeyValuePair<string, float> kvp = sampleInfo.GetSpike(i);
                        mSpikes.Add(new MSpike
                        {
                            Label = kvp.Key,
                            Value = kvp.Value
                        });
                    }
                    state.ProteinsTest = new ProteinsTest
                    {
                        BasicInfo = sampleInfo.BasicInfo,

                        Graphic = new CurveInfo
                        {
                            Curve = sampleInfo.Curve1.ToString(),
                            Fraction = sampleInfo.Fraction.ToString(),
                            Spike = sampleInfo.Spike.ToString()
                        },
                        Result = new ResultInfo
                        {
                            Albumin = sampleInfo.GetAlbumin(),
                            Alpha1 = sampleInfo.GetAlpha1(),
                            Alpha2 = sampleInfo.GetAlpha2(),
                            Beta = sampleInfo.GetBeta(),
                            Gamma = sampleInfo.GetGamma(),
                            AG = sampleInfo.GetRatioAG(),
                            MSpike = mSpikes
                        },
                        Base64Bmp = sampleInfo.Base64Image
                    };
                }
                else if (sampleInfo.BasicInfo.Test.Contains("Immunofixation"))
                {
                    state.IFETest = new IFETest
                    {
                        BasicInfo = sampleInfo.BasicInfo,
                        Base64Bmp = sampleInfo.Base64Image
                    };
                }

                string filePath = string.Format(@"{0}\{1}_{2}.xml",
                    GlobalConfigVars.XmlPath,
                    tdfInfo.GelId,
                    tdfInfo.SampleNum);
                OjbectDataXmlSerializer.Save(state, filePath);
            }
        }
    }
}
