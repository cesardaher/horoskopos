using UnityEngine.UI;

public class GetFallPlanetColor : GetColor
{
    MaskableGraphic _graphic;

    new private void Awake()
    {
        base.Awake();
        _graphic = GetComponent<MaskableGraphic>();
    }

    protected override void GetGraphicColor(int signId, SignInfoBox box)
    {
        base.GetGraphicColor(signId, box);

        int planetId = _astroIdentities.listOfSigns[signId].fall;
        if (planetId != -1)
            _graphic.color = _astroIdentities.listOfPlanets[planetId].secondaryColor;
    }
}
