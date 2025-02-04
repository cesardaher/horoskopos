using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwissEphNet;

public class EquatorDrawer : EllipseRenderer, IAzalt
{
    [SerializeField] List<Vector3> equatorPositions = new List<Vector3>();

    double[] x2 = new double[6];
    double[] xaz = new double[6];

    int vertexCount;
    int staticVertexCount = 36;
    int animVertexCount = 12;


    void Awake()
    {
        EventManager.Instance.OnRecalculationOfGeoData += FindEquatorPoints;
        EventManager.Instance.OnAnimationStart += AnimationVertexCount;
        EventManager.Instance.OnAnimationEnd += StaticVertexCount;

        vertexCount = staticVertexCount;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        EventManager.Instance.OnRecalculationOfGeoData -= FindEquatorPoints;
        EventManager.Instance.OnAnimationStart -= AnimationVertexCount;
        EventManager.Instance.OnAnimationEnd -= StaticVertexCount;
    }


    void FindEquatorPoints()
    {
        // make sure there's no overflowing positions
        equatorPositions.Clear();

        float arcStep = 360 / vertexCount;

        for (int i = 0; i <= vertexCount; i++) // IMPORTANT: <= instead of <
        {
            x2[0] = i * arcStep;

            if (x2[0] > 360) x2[0] = 0; // overflowing values go back to the first point

            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SEFLG_EQUATORIAL, GeoData.ActiveData.Geopos, 0, 0, x2, xaz);

            double azimuth = xaz[0];
            double appAlt = xaz[2];

            // rotate this
            RotateAzimuth(azimuth);
            RotateAltitude(appAlt);

            // register value
            equatorPositions.Add(pointer.position);

            // revert rotation
            transform.localEulerAngles = Vector3.zero;
        }


        DrawEllipse(equatorPositions);
    }

    public void RotateAzimuth(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;
        rotationVector.y = (float)rotation + 180;
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
