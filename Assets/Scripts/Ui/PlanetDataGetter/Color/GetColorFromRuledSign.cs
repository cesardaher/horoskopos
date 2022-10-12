using UnityEngine.UI;

public class GetColorFromRuledSign : GetColor
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
        _graphic.color = _astroIdentities.listOfSigns[PlanetData.PlanetDataList[planetId].SignID].secondaryColor;
    }
}
