using System;
using System.Collections.Generic;
using UnityEngine;
using AstroResources;

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

            meridianPoints.Add(AstroFunctions.HorizontalToCartesian(0, rotation));
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
}

