using UnityEngine.UI;

public class GetColorFromSect : GetColor
{
    public int id = 0;
    MaskableGraphic _graphic;

    new private void Awake()
    {
        base.Awake();
        _graphic = GetComponent<MaskableGraphic>();
    }

    protected override void GetGraphicColor(int planetId, PlanetInfoBox box)
    {
        base.GetGraphicColor(planetId, box);
        // ignore modern planets and nodes
        if (planetId > 7) return;

        if(_astroIdentities.listOfPlanets[planetId].sect == "Diurnal")
        {
            _graphic.color = _astroIdentities.listOfPlanets[0].secondaryColor;
            return;
        }

        _graphic.color = _astroIdentities.listOfPlanets[1].secondaryColor;
        return;

    }
}
