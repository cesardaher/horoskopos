using UnityEngine.UI;

public class GetExaltationColor : GetColor
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
        // get planet color for planets with no exiles
        if (_astroIdentities.listOfPlanets[planetId].exaltation == 0)
        {
            _graphic.color = _astroIdentities.listOfPlanets[planetId].secondaryColor;
            return;
        }

        int signID = _astroIdentities.listOfPlanets[planetId].exaltation;
        _graphic.color = _astroIdentities.listOfSigns[signID].secondaryColor;
    }
}
