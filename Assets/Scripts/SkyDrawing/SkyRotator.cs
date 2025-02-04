using SwissEphNet;
using UnityEngine;

public class SkyRotator : MonoBehaviour
{    
    public Material skybox;

    GameObject northPoleObject;
    GameObject southPoleObject;

    public Vector3 northPolePosition;
    public Vector3 southPolePosition;

    [SerializeField] float axisRotation;
    float currentAxisRotation;

    private void Start()
    {
        // tie to chart calculation event
        EventManager.Instance.OnRecalculationOfGeoData += ReRotateSpheres;

        //create poles
        CreatePoles();

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

    void CreatePoles()
    {
        CreateNorthPole();
        CreateSouthPole();
    }

    void CreateNorthPole()
    {
        northPoleObject = new GameObject("North Pole");
        GameObject child = new GameObject();
        child.transform.parent = northPoleObject.transform;

        Vector3 posVector = new Vector3{x = 100000 };
        child.transform.position = posVector;
    }

    void CreateSouthPole()
    {
        southPoleObject = new GameObject("South Pole");
        GameObject child = new GameObject();
        child.transform.parent = southPoleObject.transform;

        Vector3 posVector = new Vector3
        {
            x = 100000
        };
        child.transform.position = posVector;
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

        // generate rotation
        var rotationVector = transform.localRotation.eulerAngles;
        rotationVector.y = (float)northPoleAzalt[0] + 180;
        rotationVector.z = (float)northPoleAzalt[1];
        northPoleObject.transform.localRotation = Quaternion.Euler(rotationVector);

        //retrieve pole position
        northPolePosition = northPoleObject.transform.GetChild(0).position;
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

        // generate rotation
        var rotationVector = transform.localRotation.eulerAngles;
        rotationVector.y = (float)southPoleAzalt[0] + 180;
        rotationVector.z = (float)southPoleAzalt[1];
        southPoleObject.transform.localRotation = Quaternion.Euler(rotationVector);
             
        //retrieve pole position
        southPolePosition = southPoleObject.transform.GetChild(0).position;
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
