using SwissEphNet;
using UnityEngine;
using AstroResources;

public class PositionChartModeSymbols : Point3D
{
    public PlanetData planetData;
    //float[] _symDegMin = { 5, 10, 15f, 20, 25, 30, 35, 40, 45}; // latitudes for the symbol, degrees and possibily retrograde symbol
    float[] _symDegMin = { 4.5f, 9, 13.5f, 18f, 22.5f, 27f, 31.5f, 36f }; // latitudes for the symbol, degrees and possibily retrograde symbol
    public bool wasRotated;

    [SerializeField] Point3D _symbol;
    [SerializeField] Point3D _degrees;
    [SerializeField] Point3D _retrograde;

    public void FindLabelsPositions(double longitude, bool isRetrograde)
    {
        float[] symDegMin2 = _symDegMin;
         
        // place lattitudes of symbols as positive or negative according to hemisphere
        // if norther hemisphere, symbol has negative latitude
        // if southern hemisphere, symbol has positive latitude
        if(GeoData.ActiveData.NorthernHemisphere)
        {
            CalculateSymbolAzAltPosition(_symbol, longitude, -symDegMin2[0]);
            CalculateSymbolAzAltPosition(_degrees, longitude, -symDegMin2[1]);
            if (isRetrograde)
            {
                if (!_retrograde.gameObject.activeSelf) _retrograde.gameObject.SetActive(true);
                CalculateSymbolAzAltPosition(_retrograde, longitude, -symDegMin2[2]);
                return;
            }

            if (_retrograde.gameObject.activeSelf) _retrograde.gameObject.SetActive(false);
            return;
        }

        CalculateSymbolAzAltPosition(_symbol, longitude, symDegMin2[0]);
        CalculateSymbolAzAltPosition(_degrees, longitude, symDegMin2[1]);
        if (isRetrograde)
        {
            if (!_retrograde.gameObject.activeSelf) _retrograde.gameObject.SetActive(true);
            CalculateSymbolAzAltPosition(_retrograde, longitude, symDegMin2[2]);
            return;
        }

        if (_retrograde.gameObject.activeSelf) _retrograde.gameObject.SetActive(false);

    }

    public void PositionConjunctLabels(double longitude, int i, bool isRetrograde, bool isReverse)
    {
        float[] symDegMin2 = _symDegMin;
        float increment = 4;

        if (isReverse) increment *= -1;

        // place lattitudes of symbols as positive or negative according to hemisphere
        // if norther hemisphere, symbol has negative latitude
        // if southern hemisphere, symbol has positive latitude
        if (GeoData.ActiveData.NorthernHemisphere)
        {
            CalculateSymbolAzAltPosition(_symbol, longitude, -symDegMin2[i]);
            CalculateSymbolAzAltPosition(_degrees, longitude - increment, -symDegMin2[i]);
            if (isRetrograde)
            {
                if (!_retrograde.gameObject.activeSelf) _retrograde.gameObject.SetActive(true);
                CalculateSymbolAzAltPosition(_retrograde, longitude - 2 * increment, -symDegMin2[i]);
                return;
            }

            if (_retrograde.gameObject.activeSelf) _retrograde.gameObject.SetActive(false);
            return;
        }

        CalculateSymbolAzAltPosition(_symbol, longitude, symDegMin2[i]);
        CalculateSymbolAzAltPosition(_degrees, longitude + increment, symDegMin2[i]);
        if (isRetrograde)
        {
            if (!_retrograde.gameObject.activeSelf) _retrograde.gameObject.SetActive(true);
            CalculateSymbolAzAltPosition(_retrograde, longitude + 2 * increment, symDegMin2[i]);
            return;
        }

        if (_retrograde.gameObject.activeSelf) _retrograde.gameObject.SetActive(false);

    }

    void CalculateSymbolAzAltPosition(Point3D point, double longitude, double latitude)
    {
        double[] x2 = new double[6];
        double[] xaz = new double[6];

        x2[0] = longitude;
        x2[1] = latitude;

        // calculate horizontal coordinates

        SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, x2, xaz);


        //rotate point
        RotatePoint(point, xaz[0], xaz[2]);
    }

    void CalculateDegreesAzAltPosition(Point3D point, double longitude, double latitude)
    {
        double[] x2 = new double[6];
        double[] xaz = new double[6];

        x2[0] = longitude - 5;
        x2[1] = latitude;

        // calculate horizontal coordinates
        SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, x2, xaz);

        //rotate point
        RotatePoint(point, xaz[0], xaz[2]);
    }

    void RotatePoint(Point3D point, double az, double alt)
    {
        point.transform.GetChild(0).position = AstroFunctions.HorizontalToCartesian(az, alt);

    }
}
