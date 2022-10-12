using UnityEngine;
using AstroResources;

public class ChartElement : MonoBehaviour, IRotatable
{

    [SerializeField] GameObject chartElement2D;
    double rotation;
    public double Rotation
    {
        get { return rotation; }
        set
        {
            rotation = value;
            RotateObject(value);
            AssignSignData(value);

        }
    }

    int signID;
    [SerializeField] string sign;
    [SerializeField] double degrees;

    // functions

    public virtual void RotateObject(double rotation)
    {
        var rotationVector = chartElement2D.transform.localRotation.eulerAngles;
        rotationVector.z = (float)rotation;
        chartElement2D.transform.localRotation = Quaternion.Euler(rotationVector);
    }

    void AssignSignData(double value)
    {
        // assign sign, ID and degrees
        if (value > 360) value -= 360;
        signID = (int)System.Math.Abs(value / 30) + 1;
        sign = ((SIGN)signID).ToString();
        degrees = System.Math.Abs(value % 30);
    }
}
