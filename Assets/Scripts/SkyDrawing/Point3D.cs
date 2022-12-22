using System;
using UnityEngine;

public class Point3D : MonoBehaviour, IAzalt
{
    //rotates on the Y axis
    public void ResetRotation()
    {
        transform.localEulerAngles = Vector3.zero;
    }

    //rotates on the Azimuth 
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

    // rotate on the WORLD'S X axis
    public void RotateWorldX(float rotation)
    {
        //add new rotation
        transform.Rotate(Vector3.right, rotation, Space.World); 
    }

}
