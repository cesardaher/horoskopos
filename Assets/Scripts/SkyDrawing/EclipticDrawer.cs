using System;
using SwissEphNet;
using System.Collections.Generic;
using UnityEngine;
using AstroResources;

public class EclipticDrawer : EllipseRenderer
{
    /* This class calculates the positions of half of the ecliptic and draws
     * a line based on them. It also creates and manages the sign marker objects.
     * 
     * This requires two EclipticDrawers, one for each half of the zodiac.
     * This was to account for the possibility of adding a 12-color gradient to 
     * the zodiac. Unity's gradient, however only allows up to 8 colors. Therefore,
     * the ecliptic is divided into 2.
     
     */

    // Id defines which EclipticDrawer is in use
    public int objectID;

    // number of signs in half of ecliptic
    const int signsCount = 6;

    // vertex count: variable
    public int vertexCount;

    // Total vertex count: 72
    // 12 cusps, 12 midsigns, 3 parts of signs, 5 degrees step
    const int staticVertexCount = 36;
    const int animVertexCount = 12;

    // Auxiliary variables for holding ecliptic and horizontal positions
    double[] _x2 = new double[6];
    double[] _xaz = new double[6];

    // Holders of ecliptic positions
    [SerializeField] List<Vector3> _eclipticPositions = new List<Vector3>();
    [SerializeField] List<Vector3> _midSignsPositions = new List<Vector3>();

    // Holder of mid sign objects
    public static GameObject[] midSignsObjects = new GameObject[13];

    // MidSign prefab
    [SerializeField] GameObject _midSignModel;

    // Subscribes events
    // Prepares ecliptic objects
    void Awake()
    {
        EventManager.Instance.OnRecalculationOfGeoData += DrawLines;
        EventManager.Instance.OnAnimationStart += AnimationVertexCount;
        EventManager.Instance.OnAnimationEnd += StaticVertexCount;

        vertexCount = staticVertexCount;

        CreateHalfEclipticObjects();
    }

    // Dettaches events
    void OnDestroy()
    {
        EventManager.Instance.OnRecalculationOfGeoData -= DrawLines;
        EventManager.Instance.OnAnimationStart -= AnimationVertexCount;
        EventManager.Instance.OnAnimationEnd -= StaticVertexCount;
    }


    // Draws ecliptic/zodiac line
    public void DrawLines()
    {
        FindHalfEclipticPoints();
    }

    // Calculates positions of half of the ecliptic
    public void FindHalfEclipticPoints()
    {
        // clears existing positions, if there are any
        _eclipticPositions.Clear();
        _midSignsPositions.Clear();

        // defines the step for each calculation based on desired number of vertices
        float arcStep = 180 / vertexCount;

        // secondary index for sign count (from 0 to 12)
        int signIndex = 0;

        // iterates through half of the ecliptic
        for (int i = 0; i <= vertexCount; i++) // IMPORTANT: <= instead of <
        {
            // this loop uses the ecliptic longitude to calculate
            // the desired positions
            // 0 - Aries
            // ...
            // 180 - Libra
            // ...
            // 330 - Pisces

            // selects minimum and maximum angle
            // based on which EclipticDrawer is in use
            int idIncrement = objectID * 180;
            int maxLong = 180 + idIncrement;
            
            // incremets
            _x2[0] = (i * arcStep) + idIncrement;

            // overflowing values go back to the first point
            if (_x2[0] > maxLong) _x2[0] = 0;

            // calculates horizontal coordinates (AzAlt) based on ecliptic positions
            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, _x2, _xaz);
            double azimuth = _xaz[0];
            double trAlt = _xaz[1];

            // register values for sign cusps
            // doesn't allow 360
            if (_x2[0] % 30 == 0 && _x2[0] != 180)
            {
                // might be usable in the future for extending sign cusps
            }

            // register values for mid signs (%15)
            // this is for positioning the sign symbols
            if (_x2[0] % 30 == 15)
            {
                // presets current sign position
                double[] tempxaz = new double[6];
                double[] tempx2 = new double[6];
                tempx2[0] = _x2[0];

                // adds 5 to altitude, so that the symbol floats above ecliptic
                // up or down according to hemisphere
                if (GeoData.ActiveData.NorthernHemisphere) tempx2[1] = 5;
                else tempx2[1] = -5;

                // calculates horizontal coordinates of sign symbol
                // places midsign object
                SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, tempx2, tempxaz);
                RotateMidSign(signIndex + (6 * objectID), tempxaz[0], tempxaz[1]);

                // count next sign index
                signIndex++;
            }

            // adds horizontal position to list
            _eclipticPositions.Add(AstroFunctions.HorizontalToCartesian(azimuth, trAlt));
        }

        // draws ellipse using calculated ecliptic positions
        DrawEllipse(_eclipticPositions);
    }

    // Creates objects for sign symbols
    void CreateHalfEclipticObjects()
    {
        // set appropriate increment based on appropriate ecliptic half
        int signIncrement = 6 * objectID;

        // for the given ecliptic half, create objects and initialize them
        for (int i = 0; i < signsCount; i++)
        {
            // gets id of Sign
            int signId = signIncrement + i + 1;

            // creates object
            var newSign = Instantiate(_midSignModel, transform.parent);
            var signPoint = newSign.GetComponent<Point3D>();

            // give proper name in the inspector
            newSign.name = ((SIGN)signId).ToString() + " Mid";

            // initalize sign Info
            StartMidSigns(signPoint, signId);

            // add to array
            midSignsObjects[signId] = newSign;
        }

        // Stats the midSigns objects
        // assigns the desired info
        void StartMidSigns(Point3D point, int i)
        {
            var newPoint = (MidSign)point;

            newPoint.assignSprite(i);
            newPoint.signName = ((SIGN)i).ToString();
            newPoint.signID = i;

        }
    }
     
    // Positions MidSign objects for symbols
    void RotateMidSign(int i, double azimuth, double trAlt)
    {
        midSignsObjects[i + 1].transform.position = AstroFunctions.HorizontalToCartesian(azimuth, trAlt);
        _midSignsPositions.Add(AstroFunctions.HorizontalToCartesian(azimuth, trAlt));
    }

    // Lowers number of vertices to be used/calculated when animation is used
    // for performance
    void AnimationVertexCount()
    {
        vertexCount = animVertexCount;
    }

    // Raises number of vertices to be used/calculated when animation is used
    // for performance
    void StaticVertexCount()
    {
        vertexCount = staticVertexCount;
    }
}
