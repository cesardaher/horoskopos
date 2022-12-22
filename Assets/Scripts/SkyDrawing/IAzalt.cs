using UnityEngine;

interface IAzalt
{
    void RotateAzimuth(double azimuth);
    void RotateAltitude(double altitude);
    Vector3 RotateCartesian(double azimuth, double altitude);

}
