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

    // rotate on the WORLD'S X axis
    public void RotateWorldX(float rotation)
    {
        //add new rotation
        transform.Rotate(Vector3.right, rotation, Space.World); 
    }

}
