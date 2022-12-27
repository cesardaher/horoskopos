using SwissEphNet;
using AstroResources;

public class FixedStar : Point3D
{ 
    // CONSTRUCTORS

    public FixedStar()
    {

    }

    public FixedStar(string n, double[] pos)
    {
        starName = n;
        pos = positionData;
    }
     
    // FIELDS
    public string starName;
    public double[] positionData = new double[6];
    double[] xaz = new double[3];

    // PARAMETERS
    double[] x2 = new double[6];

    public double[] PositionData
    {
        get { return positionData; }
        set {
            positionData = value;
            //AzAlt = CalculateAzalt(positionData);
        }
    }

    public double[] azAlt;
    public double[] AzAlt
    {
        get { return azAlt; }
        set
        {
            azAlt = value;
            //RotateAzimuth(azAlt[0]);
            //RotateAltitude(azAlt[1]);
            transform.position = AstroFunctions.HorizontalToCartesian(azAlt[0], azAlt[1]);

        }
    }

    double[] CalculateAzalt(double[] x2)
    {

        SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, x2, xaz);

        return xaz;

    }


}
