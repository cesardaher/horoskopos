using UnityEngine.UI;

public class GetExiledPlanetColor : GetColor
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
        int planetId = _astroIdentities.listOfSigns[signId].exile;
        _graphic.color = _astroIdentities.listOfPlanets[planetId].secondaryColor;
    }
}
