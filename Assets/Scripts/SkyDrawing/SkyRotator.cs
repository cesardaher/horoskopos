using SwissEphNet;
using UnityEngine;
using AstroResources;

public class SkyRotator : MonoBehaviour
{    
    public Material skybox;

    public Vector3 northPolePosition;
    public Vector3 southPolePosition;

    [SerializeField] float axisRotation;
    float currentAxisRotation;

    private void Start()
    {
        // tie to chart calculation event
        EventManager.Instance.OnRecalculationOfGeoData += ReRotateSpheres;

        //initialize rotations
        currentAxisRotation = axisRotation;

        AssignShaderAxis();

        ReRotateSpheres();
    }

    private void OnDestroy()
    {
        // remove on event on destroy
        EventManager.Instance.OnRecalculationOfGeoData -= ReRotateSpheres;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(axisRotation != currentAxisRotation)
        {
            RotateOnAxis(axisRotation);
        }
    }
#endif

    void ReRotateSpheres()
    {
        RotateOnAxis(0);
        CalculateSpherePoles();
        FindRotation();
        RotateOnArmc();
    }

    public void CalculateSpherePoles()
    {
        CalculateNorthPole();
        CalculateSouthPole();
    }

    void CalculateNorthPole()
    {
        // return variable
        double[] northPoleAzalt = new double[3];

        // input variable
        double[] northPole = new double[3];
        northPole[0] = 0;
        northPole[1] = 90;
        northPole[2] = 0;


        // calculate azalt positions
        SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_EQU2HOR, GeoData.ActiveData.Geopos, 0, 0, northPole, northPoleAzalt);

        // calculates cartesian positions
        var pos = AstroFunctions.HorizontalToCartesian(northPoleAzalt[0], northPoleAzalt[2]);

        //retrieve pole position
        northPolePosition = pos;
    }

    void CalculateSouthPole()
    {
        // return variable
        double[] southPoleAzalt = new double[3];

        // input variable
        double[] southPole = new double[3];
        southPole[0] = 0;
        southPole[1] = -90;
        southPole[2] = 0;

        // calculate azalt positions
        SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_EQU2HOR, GeoData.ActiveData.Geopos, 0, 0, southPole, southPoleAzalt);

        // calculates cartesian positions
        var pos = AstroFunctions.HorizontalToCartesian(southPoleAzalt[0], southPoleAzalt[2]);
             
        //retrieve pole position
        southPolePosition = pos;
    }

    // find rotation of skybox in relation to poles
    public void FindRotation()
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, northPolePosition);
        transform.rotation = rotation;
    }

    public void RotateOnArmc()
    {
        RotateOnAxis((float)-GeoData.ActiveData.Armc);
    }

    public void RotateOnAxis(float degrees)
    {
         // revert rotation to 0
        transform.RotateAround(northPolePosition, southPolePosition, -currentAxisRotation);
        // rotate
        transform.RotateAround(northPolePosition, southPolePosition, degrees);

        axisRotation = degrees;
        currentAxisRotation = degrees;
    }

    public void AssignShaderAxis()
    {
        skybox.SetVector("_NorthPoleDirection", northPolePosition);
        skybox.SetVector("_SouthPoleDirection", southPolePosition);
        skybox.SetFloat("_ArmcRotation", (float)GeoData.ActiveData.Armc);
    }
}
