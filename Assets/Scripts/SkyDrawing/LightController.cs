using UnityEngine;
using UnityEngine.Rendering;

public class LightController : MonoBehaviour
{

    public Light sunLight, moonLight;   //Directional light references for sun and moon
    public GameObject sunSphere, moonSphere;

    public float sunLightIntensity = 30f, moonLightIntensity = 2f; //Max intensities for directional lights and flares

    [Header("Lighting and PP")]
    public Volume skyVoume;
    public AnimationCurve starsCurve;

    private void OnValidate()
    {
        ManageShadows();
    }

    void ManageShadows()
    {
        //Setting the intensity of the sun/moon on update for editor purposes
        sunLight.intensity = sunLightIntensity;
        moonLight.intensity = moonLightIntensity;

        //Toggle directional lights based on whether sun or moon is above the horizon line so we're not trying to cast two directional shadows
        //Debug.Log(sunLight.transform.forward.y);

        if (sunLight.transform.forward.y < 0) // sun is up
        {

            //moonLight.gameObject.GetComponent<HDAdditionalLightData>().EnableShadows(false); // disable moon shadows
            //sunSphere.SetActive(false);

        }
        else // sun is down
        {
            //moonLight.gameObject.GetComponent<HDAdditionalLightData>().EnableShadows(true); // enable moon shadows
            //sunSphere.SetActive(true);

        }

        if (moonLight.transform.forward.y < 0) // moon is up
        {
            //moonSphere.SetActive(false);
        } // else moonSphere.SetActive(true); //moon is down
    }
    // Update is called once per frame
    void Update()
    {
        ManageShadows();
    }

}
