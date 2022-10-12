using UnityEngine.UI;

public class GetExileColor : GetColor
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

        // prevent errors for Sun and Moon, which have only one exile each
        if (_astroIdentities.listOfPlanets[planetId].exiles.Length == 1 && id == 1)
        return;

        // get planet color for planets with no exiles
        if (_astroIdentities.listOfPlanets[planetId].exiles.Length == 0)
        {
            _graphic.color = _astroIdentities.listOfPlanets[planetId].secondaryColor;
            return;
        }

        int signID = _astroIdentities.listOfPlanets[planetId].exiles[id];
        _graphic.color = _astroIdentities.listOfSigns[signID].secondaryColor;
    }
}
