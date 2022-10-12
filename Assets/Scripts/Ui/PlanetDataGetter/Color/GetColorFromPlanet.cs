using UnityEngine;
using UnityEngine.UI;

public class GetColorFromPlanet : GetColor
{
    MaskableGraphic _graphic;

    new private void Awake()
    {
        base.Awake();
        _graphic = GetComponent<MaskableGraphic>();
    }

    protected override void GetGraphicColor(int planetId, PlanetInfoBox box)
    {
        base.GetGraphicColor(planetId, box);
        _graphic.color = _astroIdentities.listOfPlanets[planetId].secondaryColor;
    }

    protected override void GetGraphicColor(int signId, SignInfoBox box)
    {
        base.GetGraphicColor(signId, box);
        _graphic.color = _astroIdentities.listOfSigns[signId].secondaryColor;
    }
}
