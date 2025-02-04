using System.Collections.Generic;
using UnityEngine;

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

            RotateAzimuth(rotation);

            // register value
            horizonPoints.Add(pointer.position);
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
