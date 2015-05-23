namespace DbExporter.Provider.Spife4000
{
    //Spife Curve Point
    public class FractionPoint
    {
        public int X = -1;
        public int Y = -1;
        public int Z = 0;

        public override string ToString()
        {
            return X.ToString() + "," + Y.ToString() + "," + Z.ToString();
        }
    }
}
