using System;
using SwissEphNet;
using System.Collections.Generic;
using UnityEngine;

public class CuspExtender : EllipseRenderer, IAzalt
{
    public int houseId;
    public static GameObject southPole;
    public static GameObject northPole;
    public static GameObject mainPole;
    public static GameObject oppositePole;

    [SerializeField] Material houseMat;
    //[SerializeField] Material signMat;

    List<Vector3> cuspPoints = new List<Vector3>();
    [Range(12, 72)]
    [SerializeField] int vertexCount;

    double[] x2 = new double[6];
    double[] xaz = new double[6];
    [SerializeField]  float xRotation;
    float currentXRotation;

    // width values
    float houseWidthMultiplier = 100f;
    float houseWidth = 0.1f;
    int staticVertexCount = 72;
    int animVertexCount = 36;

    void Start()
    {
        EventManager.Instance.OnAnimationStart += AnimationVertexCount;
        EventManager.Instance.OnAnimationEnd += StaticVertexCount;

        StaticVertexCount();
        currentXRotation = xRotation;
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

            // REVERT
            cuspPoints.Add(pointer.position);

            // reverse rotation to prepare for next iteration
            RotateX(-30 * houseId);
            RotateAzimuth(-azimuth);

        }

        lineRenderer.material = houseMat;
        DrawEllipse(cuspPoints);
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
            double appAlt = xaz[2];

            // rotate this
            RotateAzimuth(azimuth);
            RotateAltitude(appAlt);

            // register value
            cuspPoints.Add(pointer.position);

            // revert rotation
            transform.localEulerAngles = Vector3.zero;
        }

        lineRenderer.material = houseMat;
        DrawEllipse(cuspPoints);
    }

    public void CreateSpherePoles(int i)
    {
        if (i == 0 && mainPole == null)
        {
            mainPole = CreatePolePoint(transform.GetChild(0).position);
            mainPole.name = "Main Pole";
            return;
        }

        if (i == vertexCount/4 && southPole == null)
        {
            southPole = CreatePolePoint(transform.GetChild(0).position);
            southPole.name = "South Pole";
            return;
        }

        if (i == vertexCount/2 && oppositePole == null)
        {
            oppositePole = CreatePolePoint(transform.GetChild(0).position);
            oppositePole.name = "Opposite Pole";
            return;
        }

        if (i == 3* vertexCount / 4 && northPole == null)
        {
            northPole = CreatePolePoint(transform.GetChild(0).position);
            northPole.name = "North Pole";
            return;
        }
    }


    public GameObject CreatePolePoint(Vector3 pos)
    {

        GameObject obj = new GameObject();

        obj.transform.position = pos;

        return obj;
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

    public Vector3 RotateCartesian(double azimuth, double altitude)
    {
        double x, y, z;

        double alt = 90 - altitude;
        double az = 180 + azimuth;

        x = 10000 * Math.Sin(alt * Mathf.Deg2Rad) * Math.Cos(az * Mathf.Deg2Rad);
        y = 10000 * Math.Cos(alt * Mathf.Deg2Rad);
        z = 10000 * Math.Sin(alt * Mathf.Deg2Rad) * Math.Sin(az * Mathf.Deg2Rad);

        return new Vector3((float)x, (float)y, (float)z);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if(currentXRotation != xRotation)
        {
            RotateX(-currentXRotation);
            RotateX(xRotation);

            currentXRotation = xRotation;
        }
        //SetHouseWidth();

    }
    void SetHouseWidth()
    {
        AnimationCurve curve = new AnimationCurve();
    
        //clear keys
        for (int i = 0; i < curve.length; i++)
        {
            curve.RemoveKey(i);
        }

        // first segment
        curve.AddKey(0.0f, houseWidth);
        curve.AddKey(1.0f, houseWidth);

        lineRenderer.widthCurve = curve;
        lineRenderer.widthMultiplier = houseWidthMultiplier;
    }
#endif
}

