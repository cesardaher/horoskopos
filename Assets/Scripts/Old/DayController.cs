using UnityEngine;
using UnityEngine.Rendering;

public class DayController : MonoBehaviour
{
    const float seasonSpan = 91.25f;
    const float tilt = 23.5f;

    public Light sunLight, moonLight;   //Directional light references for sun and moon

    public float sunLightIntensity = 30f, moonLightIntensity = 2f; //Max intensities for directional lights and flares

    public float seasonAngle = 30f;   //Extra axis control for sun/moon to simulate longer or shorter days

    public bool cycleTimeOfDay = false;   //Use this to pause the time of day cycle

    public float latitude;
    float subsolarPoint;

    public bool northern;
    
    [Range(0, 2400)]
    public float currentTime = 1600f; //Current time of day in 24-hour clock system

    [Range(0, 365)]
    public float seasonState;

    string currentSeason;

    public float timeScale = 1f;

    [Header("Lighting and PP")]
    public Volume skyVoume;
    public AnimationCurve starsCurve;

    private void OnValidate()
    {
        UpdateSeason();
        Debug.Log(subsolarPoint);
        Debug.Log(currentSeason);
        UpdateSeasonAngle();
        UpdateTime();
    }

    void UpdateTime()
    {
        //Starts the day timer over at the end of 24 hours
        currentTime = Mathf.Repeat(currentTime, 2400);

        //Controls the angle of the sun by mapping the current time to a 360 degree cycle, with time offset to account for correcting initial rotation
        sunLight.transform.eulerAngles = Vector3.right * ((currentTime - 600) / 2400) * 360;

        //Apply the season modifier
        sunLight.transform.Rotate(Vector3.forward, seasonAngle, Space.World);

        //Setting the intensity of the sun/moon on update for editor purposes
        sunLight.intensity = sunLightIntensity;
        moonLight.intensity = moonLightIntensity;

        //Toggle directional lights based on whether sun or moon is above the horizon line so we're not trying to cast two directional shadows
        if (-sunLight.transform.forward.y > 0)
        {
            if (!sunLight.enabled)
            {
                sunLight.enabled = true;
                moonLight.enabled = false;
            }

        }
        else
        {
            if (sunLight.enabled)
            {
                sunLight.enabled = false;
                moonLight.enabled = true;
            }

        }

    }

    void UpdateSeason()
    {
        // adjust season state to seasons and normalize it to the tilt of 23.5
        if(seasonState >= 0 && seasonState < seasonSpan)
        {
            currentSeason = "Spring";
            subsolarPoint = (tilt * seasonState) / seasonSpan;
            return;
        }

        if (seasonState >= seasonSpan && seasonState < seasonSpan * 2)
        {
            currentSeason = "Summer";
            subsolarPoint = tilt - ((tilt * (seasonState - seasonSpan)) / seasonSpan);
            return;
        }

        if (seasonState >= 2 * seasonSpan && seasonState < seasonSpan * 3)
        {
            currentSeason = "Autumn";
            subsolarPoint = (tilt * (seasonState - 2 * seasonSpan)) / seasonSpan;
            return;
        }

        if (seasonState >= 3 * seasonSpan && seasonState < seasonSpan * 4)
        {
            currentSeason = "Winter";
            subsolarPoint = tilt - ((tilt * (seasonState - 3 * seasonSpan)) / seasonSpan);
            return;
        }
    }

    void UpdateSeasonAngle()
    {

        // adjust subsolar point based on latitude's direction + season
        if(northern)
        {
            if(currentSeason == "Winter" || currentSeason == "Autumn")
            {
                subsolarPoint = subsolarPoint * (-1);
            }
        }
        else
        {
            if (currentSeason == "Summer" || currentSeason == "Spring")
            {
                subsolarPoint = subsolarPoint * (-1);
            }
        }

        float zenithAngle;

        zenithAngle = latitude + subsolarPoint;

        seasonAngle = 90 - zenithAngle;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSeason();
        UpdateSeasonAngle();

        //Advance the time of day cycle
        if (cycleTimeOfDay)
        {
            currentTime += Time.deltaTime * timeScale;
        }

        UpdateTime();
    }

}
