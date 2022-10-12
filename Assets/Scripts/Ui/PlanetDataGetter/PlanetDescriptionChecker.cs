using UnityEngine;

public class PlanetDescriptionChecker : PlanetDataGetter
{

    [SerializeField] BoxDescriptionText planetInfoText;

    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        if (planetId < planetInfoText.planetInformationTexts.Count)
        {
            
            textMesh.text = planetInfoText.planetInformationTexts[planetId].description.Replace("\r", "");
        }
    }
}
