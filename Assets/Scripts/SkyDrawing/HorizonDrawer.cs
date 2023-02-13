using System;
using System.Collections.Generic;
using UnityEngine;
using AstroResources;

public class HorizonDrawer : EllipseRenderer, IAzalt
{
    List<Vector3> horizonPoints = new List<Vector3>();
    [Range(12, 72)]
    [SerializeField] int vertexCount;

    void Start()
    {

        DrawHorizon();
    }


    public void DrawHorizon()
    {
        horizonPoints.Clear();

        float arcStep = 360 / vertexCount;

        for (int i = 0; i <= vertexCount; i++)
        {
            double rotation = i * arcStep;

            horizonPoints.Add(AstroFunctions.HorizontalToCartesian(rotation, 0));
        }

        DrawEllipse(horizonPoints);
    }

    public void RotateAzimuth(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;

        rotationVector.y = (float)rotation;

        transform.localRotation = Quaternion.Euler(rotationVector);
    }

    //rotates on the Z axis
    public void RotateAltitude(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;
        rotationVector.z = (float)rotation;
        transform.localRotation = Quaternion.Euler(rotationVector);
    }
}
