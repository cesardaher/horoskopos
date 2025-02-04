using SwissEphNet;
using System.Collections.Generic;
using UnityEngine;
using AstroResources;

public class EclipticDrawer : EllipseRenderer, IAzalt
{
    public int objectID;
    [SerializeField] CuspExtender houseCuspExtender;
    [SerializeField] CuspExtender signCuspExtender;
    [SerializeField] GameObject cuspHolder;
    [SerializeField] GameObject midSignModel;
    [SerializeField] GameObject midHouseModel;

    double[] x2 = new double[6];
    double[] xaz = new double[6];

    [SerializeField] List<Vector3> eclipticPositions = new List<Vector3>();
    [SerializeField] List<Vector3> midSignsPositions = new List<Vector3>();
    public static GameObject[] midSignsObjects = new GameObject[13];

    // 72 value:
    // 12 cusps, 12 midsigns, 3 parts of signs, 5 degrees step
    const int signsCount = 6;
    public int vertexCount;

    int staticVertexCount = 36;
    int animVertexCount = 12;

    void Awake()
    {
        EventManager.Instance.OnRecalculationOfGeoData += DrawLines;
        EventManager.Instance.OnAnimationStart += AnimationVertexCount;
        EventManager.Instance.OnAnimationEnd += StaticVertexCount;

        vertexCount = staticVertexCount;

        CreateHalfEclipticObjects();
    }

    void OnDestroy()
    {
        EventManager.Instance.OnRecalculationOfGeoData -= DrawLines;
        EventManager.Instance.OnAnimationStart -= AnimationVertexCount;
        EventManager.Instance.OnAnimationEnd -= StaticVertexCount;
    }

   public void DrawLines()
   {
        FindHalfEclipticPoints();
   }

    public void FindHalfEclipticPoints()
    {
        // make sure there's no overflowing positions
        eclipticPositions.Clear();

        float arcStep = 180 / vertexCount;

        midSignsPositions.Clear();

        int signIndex = 0;

        for (int i = 0; i <= vertexCount; i++) // IMPORTANT: <= instead of <
        {
            int idIncrement = objectID * 180;
            int maxLong = 180 + idIncrement;
                
            x2[0] = (i * arcStep) + idIncrement;

            if (x2[0] > maxLong) x2[0] = 0; // overflowing values go back to the first point

            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, x2, xaz);

            double azimuth = xaz[0];
            double appAlt = xaz[2];

            // register values for sign cusps
            // doesn't allow 360
            if (x2[0] % 30 == 0 && x2[0] != 180)
            {
                //rotateCusp(i, azimuth, appAlt);                   
            }

            // register values for mid signs
            if (x2[0] % 30 == 15)
            {
                double[] tempxaz = new double[6];
                double[] tempx2 = new double[6];
                tempx2[0] = x2[0];
                if (GeoData.ActiveData.NorthernHemisphere) tempx2[1] = 5;
                else tempx2[1] = -5;
                SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, tempx2, tempxaz);
                RotateMidSign(signIndex + (6 * objectID), tempxaz[0], tempxaz[2]);

                // count next sign index
                signIndex++;
            }

            // rotate this
            RotateAzimuth(azimuth);
            RotateAltitude(appAlt);

            // register value
            eclipticPositions.Add(pointer.position);
        }

        DrawEllipse(eclipticPositions);
    }

    void CreateHalfEclipticObjects()
    {
        int signIncrement = 6 * objectID;

        for (int i = 0; i < signsCount; i++)
        {
            int signId = signIncrement + i + 1;

            var newSign = Instantiate(midSignModel, transform.parent);
            var signPoint = newSign.GetComponent<Point3D>();

            newSign.name = ((SIGN)signId).ToString() + " Mid";

            StartPoints(signPoint, signId);

            midSignsObjects[signId] = newSign;
        }

    }

    void StartPoints(Point3D point, int i)
    {
        var newPoint = (MidSign)point;

        newPoint.assignSprite(i);
        newPoint.signName = ((SIGN)i).ToString();
        newPoint.signID = i;

    }
     
    void RotateMidSign(int i, double azimuth, double appAlt)
    {
        // rotate
        midSignsObjects[i + 1].GetComponent<Point3D>().RotateAzimuth(azimuth);
        midSignsObjects[i + 1].GetComponent<Point3D>().RotateAltitude(appAlt);

        midSignsPositions.Add(midSignsObjects[i + 1].transform.GetChild(0).position);
    }

    public void RotateAzimuth(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;

        rotationVector.y = (float)rotation + 180;
        if (GeoData.ActiveData._northernHemisphere)
        {
            //rotationVector.y -= 180;
            //Debug.Log("northern");
        }

        transform.localRotation = Quaternion.Euler(rotationVector);
    }

    //rotates on the Z axis
    public void RotateAltitude(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;
        rotationVector.z = (float)rotation;
        transform.localRotation = Quaternion.Euler(rotationVector);
    }

    void AnimationVertexCount()
    {
        vertexCount = animVertexCount;
    }

    void StaticVertexCount()
    {
        vertexCount = staticVertexCount;
    }
}
