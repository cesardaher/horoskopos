using System;
using System.Collections.Generic;
using UnityEngine;

public class MeridianDrawer : EllipseRenderer, IAzalt
{
    List<Vector3> meridianPoints = new List<Vector3>();
    [Range(12, 72)]
    [SerializeField] int vertexCount;


    void Start()
    {
        DrawMeridian();
    }


    public void DrawMeridian()
    {
        meridianPoints.Clear();

        float arcStep = 360 / vertexCount;

        for (int i = 0; i <= vertexCount; i++)
        {
            double rotation = i * arcStep;
            
            RotateAltitude(rotation);

            // register value
            meridianPoints.Add(pointer.position);

            RotateAltitude(-rotation);
        }

        DrawEllipse(meridianPoints);
    }

    public void RotateAzimuth(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;

        rotationVector.y += (float)rotation;

        transform.localRotation = Quaternion.Euler(rotationVector);
    }

    //rotates on the Z axis
    public void RotateAltitude(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;
        rotationVector.z += (float)rotation;
        transform.localRotation = Quaternion.Euler(rotationVector);
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
}

