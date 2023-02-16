using System;
using SwissEphNet;
using System.Collections.Generic;
using UnityEngine;
using AstroResources;

public class CuspExtender : EllipseRenderer, IAzalt
{
    public int houseId;
    public static GameObject southPole;
    public static GameObject northPole;
    public static GameObject mainPole;
    public static GameObject oppositePole;

    [SerializeField] Material houseMat;

    List<Vector3> cuspPoints = new List<Vector3>();
    [Range(12, 72)]
    [SerializeField] int vertexCount;

    double[] x2 = new double[6];
    double[] xaz = new double[6];

    int staticVertexCount = 72;
    int animVertexCount = 36;

    void Start()
    {
        EventManager.Instance.OnAnimationStart += AnimationVertexCount;
        EventManager.Instance.OnAnimationEnd += StaticVertexCount;

        StaticVertexCount();
    }

    void OnDestroy()
    {
        EventManager.Instance.OnAnimationStart -= AnimationVertexCount;
        EventManager.Instance.OnAnimationEnd -= StaticVertexCount;
    }

    public void ExtendCuspCampanus()
    {
        cuspPoints.Clear();
        float arcStep = 360 / vertexCount;

        
        for (int i = 0; i <= vertexCount; i++) // IMPORTANT: <= instead of <
        {
            double azimuth = i * arcStep;

            // rotate
            RotateAzimuth(azimuth);
            RotateX(30 * houseId);

            cuspPoints.Add(pointer.position);

            // reverse rotation to prepare for next iteration
            RotateX(-30 * houseId);
            RotateAzimuth(-azimuth);

        }

        lineRenderer.material = houseMat;
        DrawEllipse(cuspPoints);
    }

    public void ExtendCuspRegiomontanus()
    {
        cuspPoints.Clear();
        float arcStep = 360 / vertexCount;

        double altitude = NorthSouthAzimuth();

        for (int i = 0; i <= vertexCount; i++) // IMPORTANT: <= instead of <
        {
            double azimuth = i * arcStep;

            // rotate
            RotateAzimuth(azimuth);
            RotateX((float)altitude);

            cuspPoints.Add(pointer.position);

            // reverse rotation to prepare for next iteration
            RotateX((float)-altitude);
            RotateAzimuth(-azimuth);

        }

        lineRenderer.material = houseMat;
        DrawEllipse(cuspPoints);

        double NorthSouthAzimuth()
        {
            double[] cuspPos = new double[6];
            cuspPos[0] = HouseData.instance.houseDataList[houseId].Longitude;
            double[] cuspPosHor = new double[6];

            // HORIZONTAL COORDINATE FROM ECLIPTIC CUSP
            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, cuspPos, cuspPosHor);

            // CUSP TO CARTESIAN 
            Vector3 cartesianCusp = AstroFunctions.HorizontalToCartesian(cuspPosHor[0], cuspPosHor[1]);

            // CARTESIAN TO NORTHSOUTH
            double x, y, z;
            double r, theta, phi;

            x = cartesianCusp.x;
            y = cartesianCusp.y;
            z = cartesianCusp.z;

            r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
            //theta = Math.Atan2(x, z);
            theta = Math.Atan(y / z);
            phi = Math.Atan(Math.Sqrt(Math.Pow(y,2) + Math.Pow(z,2))/ x);

            Debug.Log("house iD: " + houseId);
            Debug.Log("r: " + r);
            Debug.Log("theta: " + theta * Mathf.Rad2Deg);
            Debug.Log("phi: " + phi * Mathf.Rad2Deg);

            return theta * Mathf.Rad2Deg;
        }
    }


    public void ExtendHouseCusp(double longitude)
    {
        cuspPoints.Clear();

        float arcStep = 360 / vertexCount;

        for (int i = 0; i <= vertexCount; i++) // IMPORTANT: <= instead of <
        {
            x2[0] = longitude; //longitude is gotten from cusps
            x2[1] = i * arcStep; //latitude

            if (x2[0] > 360) x2[0] = 0; // overflowing values go back to the first point

            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, x2, xaz);

            double azimuth = xaz[0];
            double trAlt = xaz[1];

            // register value
            cuspPoints.Add(AstroFunctions.HorizontalToCartesian(azimuth, trAlt));
        }

        lineRenderer.material = houseMat;
        DrawEllipse(cuspPoints);
    }

    void AnimationVertexCount()
    {
        vertexCount = animVertexCount;
    }

    void StaticVertexCount()
    {
        vertexCount = staticVertexCount;
    }

    // rotate on the Y axis
    // this represents a rotation on
    public void RotateAzimuth(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;

        rotationVector.y += (float)rotation + 180;

        transform.localRotation = Quaternion.Euler(rotationVector);
    }

    //rotates on the Z axis
    public void RotateAltitude(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;
        rotationVector.z += (float)rotation;
        transform.localRotation = Quaternion.Euler(rotationVector);
    }

    // rotate on the WORLD'S X axis
    public void RotateX(float rotation)
    {
        //add new rotation
        transform.Rotate(Vector3.right, rotation, Space.World);

    }
}

