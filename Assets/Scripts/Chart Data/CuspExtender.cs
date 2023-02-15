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

        //GetEquatorSliceSize();

        for (int i = 0; i <= vertexCount; i++) // IMPORTANT: <= instead of <
        {
            double azimuth = i * arcStep;
            double altitude = GetEquatorAltitude(houseId);
            // rotate
            RotateAzimuth(azimuth);
            RotateX((float)altitude);

            cuspPoints.Add(pointer.position);

            // reverse rotation to prepare for next iteration
            RotateX(-(float)altitude);
            RotateAzimuth(-azimuth);

        }

        lineRenderer.material = houseMat;
        DrawEllipse(cuspPoints);

        double GetAltitude()
        {
            double[] eclipticPos = new double[6];
            eclipticPos[0] = HouseData.instance.houseDataList[houseId].Longitude;

            double[] azaltPos = new double[6];

            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, eclipticPos, azaltPos);

            return -azaltPos[1];
        }

        /*
        double GetEquatorSliceSize()
        {
            double size = 0;

            double[] ascPos = new double[6];
            ascPos[0] = HouseData.instance.houseDataList[10].Longitude;
            double[] mcPos = new double[6];
            mcPos[0] = HouseData.instance.houseDataList[7].Longitude;

            double[] ascEquator = new double[6];
            double[] mcEquator = new double[6];

            SwissEphemerisManager.swe.swe_cotrans(ascPos, ascEquator, -23.437404);
            SwissEphemerisManager.swe.swe_cotrans(mcPos, mcEquator, -23.437404);

            Debug.Log("asc: " + ascPos[0] + " mc: " + mcPos[0]);
            Debug.Log("ascEq: " + ascEquator[0] + " mcEq: " + mcEquator[0]);
            size = ascEquator[0] - mcEquator[0];

            Debug.Log("diff: " + size);

            size /= 3;

            Debug.Log("final: " + size);

            return size;
        }*/

        double GetEquatorSliceSize()
        {
            double[] eastPosHor = new double[6];
            double[] westPosHor = new double[6];
            westPosHor[0] = 180;

            double[] eastPos = new double[6];
            double[] westPos = new double[6];

            SwissEphemerisManager.swe.swe_azalt_rev(GeoData.ActiveData.Tjd_ut, SwissEph.SE_HOR2EQU, GeoData.ActiveData.Geopos, eastPosHor, eastPos);
            SwissEphemerisManager.swe.swe_azalt_rev(GeoData.ActiveData.Tjd_ut, SwissEph.SE_HOR2EQU, GeoData.ActiveData.Geopos, westPosHor, westPos);

            Debug.Log("east: " + eastPos[0]);
            Debug.Log("west: " + westPos[0]);

            double size = eastPos[0] - westPos[0];

            Debug.Log("size: " + size);
            return size;
        }

        double GetEquatorAltitude(int i)
        {
            double[] eastPosHor = new double[6];
            double[] eastPosEqu = new double[6];
            SwissEphemerisManager.swe.swe_azalt_rev(GeoData.ActiveData.Tjd_ut, SwissEph.SE_HOR2EQU, GeoData.ActiveData.Geopos, eastPosHor, eastPosEqu);

            double[] posEcl = eastPosEqu;
            posEcl[0] += i * 30;
            double[] posEclFinal = new double[6];
            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_EQU2HOR, GeoData.ActiveData.Geopos, 0, 0, posEcl, posEclFinal);

            return posEclFinal[0];
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

