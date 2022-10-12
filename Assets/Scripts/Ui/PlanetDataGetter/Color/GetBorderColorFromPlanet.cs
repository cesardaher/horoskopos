using UnityEngine.UI;

public class GetBorderColorFromPlanet : GetColor
{
    Outline _outline;

    new private void Awake()
    {
        base.Awake();
        _outline = GetComponent<Outline>();

    }

    protected override void GetGraphicColor(int planetId, PlanetInfoBox box)
    {
        base.GetGraphicColor(planetId, box);
        _outline.effectColor = _astroIdentities.listOfPlanets[planetId].secondaryColor;
    }
}
