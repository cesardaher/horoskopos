using UnityEngine;

public class EclipticLineMaterialController : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Material eclipticLineMat;
    [SerializeField] Color skyViewColor;
    [SerializeField] Color chartViewColor;
    [SerializeField] Gradient normalColor;
    [SerializeField] Gradient elementGradient;
    [SerializeField] Gradient seasonGradient;


    private void Awake()
    {
        EventManager.Instance.OnRecalculationOfGeoData += FlipEclipticLine;
        EventManager.Instance.OnToggleEclipticGlow += ToggleGlow;
        EventManager.Instance.OnSkyMode += SkyViewColor;
        EventManager.Instance.OnChartMode += ChartViewColor;
        EventManager.Instance.OnZodiacSeasonSeparation += ApplySeasonGradient;
        EventManager.Instance.OnZodiacElementSeparation += ApplyElementGradient;
        EventManager.Instance.OnZodiacRevertColor += ApplyNormalGradient;
    }

    private void Start()
    {
        lineRenderer.startWidth = 2000;
        lineRenderer.endWidth = 2000;

        //lineRenderer.widthMultiplier = 2000;
        // disable glow always at start
        ToggleGlow(false);
    }

    void FlipEclipticLine()
    {
        if(GeoData.ActiveData.NorthernHemisphere)
            eclipticLineMat.SetFloat("_NorthernHemisphere", 1);
        else
            eclipticLineMat.SetFloat("_NorthernHemisphere", 0);
    }

    void GlowEclipticLine(float val)
    {
        eclipticLineMat.SetFloat("_GlowIntensity", val);
    }

    void ToggleGlow(bool val)
    {
        if(val)
            eclipticLineMat.SetFloat("_GlowIntensity", 0.25f);
        else
            eclipticLineMat.SetFloat("_GlowIntensity", 0);
    }

    void SkyViewColor()
    {
        eclipticLineMat.SetColor("_Color", skyViewColor);
    }

    void ChartViewColor()
    {
        eclipticLineMat.SetColor("_Color", chartViewColor);
    }

    void ApplySeasonGradient()
    {
        lineRenderer.colorGradient = seasonGradient;
    }

    void ApplyElementGradient()
    {
        lineRenderer.colorGradient = elementGradient;
    }

    void ApplyNormalGradient()
    {
        lineRenderer.colorGradient = normalColor;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnRecalculationOfGeoData -= FlipEclipticLine;
        EventManager.Instance.OnToggleEclipticGlow -= ToggleGlow;
        EventManager.Instance.OnSkyMode -= SkyViewColor;
        EventManager.Instance.OnChartMode -= ChartViewColor;
        EventManager.Instance.OnZodiacSeasonSeparation -= ApplySeasonGradient;
        EventManager.Instance.OnZodiacElementSeparation -= ApplyElementGradient;
        EventManager.Instance.OnZodiacRevertColor -= ApplyNormalGradient;
    }

}
