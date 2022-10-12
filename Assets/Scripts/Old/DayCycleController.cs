using UnityEngine;
using UnityEngine.Rendering;

public class DayCycleController : MonoBehaviour
{
    [Range(0, 24)]
    public float timeOfDay;

    public float orbitSpeed = 1.0f;

    // z axis rotation
    public float seasonRotation = 0;

    [Header("Planets")]
    public Light sun;
    public Light moon;

    [Header("Lighting and PP")]
    public Volume skyVoume;
    public AnimationCurve starsCurve;

    bool isNight;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timeOfDay += Time.deltaTime * orbitSpeed;
        if (timeOfDay > 24)
            timeOfDay = 0;

        UpdateTime();
    }

    private void OnValidate()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        // normalized time of day
        float alpha = timeOfDay / 24.0f;

        // rotation, based on midnight and midlight
        float sunRotation = Mathf.Lerp(-90, 270, alpha);
        float moonRotation = sunRotation - 180;

        // application of sun rotation
        sun.transform.rotation = Quaternion.Euler(sunRotation, -150.0f, seasonRotation);
        moon.transform.rotation = Quaternion.Euler(moonRotation, -150.0f, seasonRotation);

        //sky.spaceEmissionMultiplier.value = starsCurve.Evaluate(alpha) * 1000.0f;

        //check if daytime is changed
        CheckNightDayTransition();

    }

    void CheckNightDayTransition()
    {
        //
        if (isNight)
        {
            if(moon.transform.rotation.eulerAngles.x > 180)
            {
                StartDay();
                return;
            }
        }

        if (moon.transform.rotation.eulerAngles.x > 180)
        {
            StartNight();
        }
    }

    // disable shadows on moon
    private void StartDay()
    {
        isNight = false;
        sun.shadows = LightShadows.Soft;
        moon.shadows = LightShadows.None;
    }

    // disable shadows on sun
    private void StartNight()
    {
        isNight = true;
        sun.shadows = LightShadows.None;
        moon.shadows = LightShadows.Soft;

    }
}
