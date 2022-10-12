using TMPro;
using UnityEngine;

public class TogglePlanetOnSettings : MonoBehaviour
{
    public bool active { get { return planetData.IsActive; } }
    PlanetData planetData;
    public int planetId;
    TextMeshProUGUI textMesh;
    AstrologicalIdentity astrologicalIdentity;

    private void Awake()
    {
        astrologicalIdentity = Resources.Load<AstrologicalIdentity>("AstroIdentities");
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        planetData = PlanetData.PlanetDataList[planetId];
        GetPlanetarySymbolAndColor();
    }

    void GetPlanetarySymbolAndColor()
    {
        if (planetId > 10) return;
        textMesh.text = astrologicalIdentity.listOfPlanets[planetId].symbol;
        textMesh.color = astrologicalIdentity.listOfPlanets[planetId].secondaryColor;
    }

    public void TogglePlanetIndividually()
    {
        planetData.IsActive = !planetData.IsActive;

        if (planetData.IsActive)
        {
            SetFullColor();
        }
        else
        {
            SetHalfColor();
        }

        EventManager.Instance.ToggleMultiplePlanets();
    }

    public void TogglePlanetVisibility()
    {
        planetData.IsActive = !planetData.IsActive;

        if(planetData.IsActive)
        {
            SetFullColor();
        }
        else
        {
            SetHalfColor();
        }
    }

    void SetFullColor()
    {
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1);
    }

    void SetHalfColor()
    {
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0.15f);
    }
}
