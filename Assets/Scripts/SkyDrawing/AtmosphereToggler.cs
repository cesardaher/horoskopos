using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereToggler : MonoBehaviour
{
    [SerializeField] Material material;
    float sunsetIntensity = 0.08f;
    public bool isAtmosphereOn { get; private set; }

    [ColorUsage(true, true)] [SerializeField] Color dayZenithColor;
    [ColorUsage(true, true)] [SerializeField] Color dayHorizonColor;
    [ColorUsage(true, true)] [SerializeField] Color nightZenithColor;
    [ColorUsage(true, true)] [SerializeField] Color nightHorizonColor;

    [ColorUsage(true, true)] [SerializeField] Color chartDayColor;
    [ColorUsage(true, true)] [SerializeField] Color chartNightColor;

   
    private void OnValidate()
    {

    }

    private void Start()
    {
        ToggleAtmosphere(true);
    }

    public void ToggleAtmosphere(bool val)
    {
        isAtmosphereOn = val;
        if (ModesMenu.chartModeOn) return;

        if (val)
        {
            material.SetColor("_DayZenithColor", dayZenithColor);
            material.SetColor("_DayHorizonColor", dayHorizonColor);
            material.SetColor("_NightZenithColor", nightZenithColor);
            material.SetColor("_NightHorizonColor", nightHorizonColor);
            material.SetFloat("_SunsetIntensity", sunsetIntensity);
            return;
        }

        material.SetColor("_DayZenithColor", nightZenithColor);
        material.SetColor("_DayHorizonColor", nightZenithColor);
        material.SetColor("_NightZenithColor", nightZenithColor);
        material.SetColor("_NightHorizonColor", nightZenithColor);
        material.SetFloat("_SunsetIntensity", 0);
    }

    public void ToggleChartModeColors(bool val)
    {
        if (val)
        {
            material.SetColor("_DayZenithColor", chartDayColor);
            material.SetColor("_DayHorizonColor", chartDayColor);
            material.SetColor("_NightZenithColor", chartNightColor);
            material.SetColor("_NightHorizonColor", chartNightColor);
            material.SetFloat("_SunsetIntensity", sunsetIntensity);
            return;
        }

        if (isAtmosphereOn)
        {
            ToggleAtmosphere(true);
            return;
        }

        ToggleAtmosphere(false);
    }

    public void SetSunsetIntensity(bool val)
    {
        if(val)
            material.SetFloat("_SunsetIntensity", sunsetIntensity);
        else
            material.SetFloat("_SunsetIntensity", 0);
    }

    public void SetGround(bool val)
    {
        if (val)
            material.SetFloat("_UseGround", 1);
        else
            material.SetFloat("_UseGround", 0);
    }
}

