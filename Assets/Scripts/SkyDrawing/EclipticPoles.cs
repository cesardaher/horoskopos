using AstroResources;
using UnityEngine;
using SwissEphNet;

public class EclipticPoles : MonoBehaviour
{
    GameObject northPoleObject;
    GameObject southPoleObject;
    Transform northPolePointer;
    Transform southPolePointer;

    public Vector3 northPolePosition;
    public Vector3 southPolePosition;

    void Start()
    {
        // tie to chart calculation event
        EventManager.Instance.OnRecalculationOfGeoData += CalculateSpherePoles;

        CreatePoles();

        CalculateSpherePoles();
    }

    private void OnDestroy()
    {
        // remove from chart calculation event
        EventManager.Instance.OnRecalculationOfGeoData -= CalculateSpherePoles;
    }


    void CreatePoles()
    {
        CreateNorthPole();
        CreateSouthPole();
    }

    void CreateNorthPole()
    {
        northPoleObject = new GameObject("North Ecliptic Pole");
        GameObject child = new GameObject();
        child.transform.parent = northPoleObject.transform;
        northPolePointer = child.transform;

        Vector3 posVector = new Vector3 { x = 10000 };
        child.transform.position = posVector;
    }

    void CreateSouthPole()
    {
        southPoleObject = new GameObject("South Ecliptic Pole");
        GameObject child = new GameObject();
        child.transform.parent = southPoleObject.transform;
        southPolePointer = child.transform;

        Vector3 posVector = new Vector3
        {
            x = 10000
        };
        child.transform.position = posVector;
    }

    public void CalculateSpherePoles()
    {
        CalculatePole(true);
        CalculatePole(false);
    }

    void CalculatePole(bool isNorth)
    {
        double poleLat;
        Transform pole;

        if (isNorth)
        {
            poleLat = 90;
            pole = northPoleObject.transform;
        }
        else
        {
            poleLat = -90;
            pole = southPoleObject.transform;

        }

        // return variable
        double[] poleAzalt = new double[3];

        // input variable
        double[] poleX2 = new double[3];
        poleX2[0] = 0;
        poleX2[1] = poleLat;
        poleX2[2] = 0;


        // calculate azalt positions
        SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, poleX2, poleAzalt);

        // calculates cartesian positions
        var pos = AstroFunctions.HorizontalToCartesian(poleAzalt[0], poleAzalt[2]);

        //retrieve pole position
        if(isNorth)
            northPolePosition = pos;
        else
            southPolePosition = pos;

    }

}
