public class ExaltationSymbolChecker : PlanetDataGetter
{
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        int signID = PlanetData.PlanetDataList[planetId].SignID;
        if (signID == astroIdentity.listOfPlanets[planetId].exaltation)
        {
            gameObject.SetActive(true);
            textMesh.text = astroIdentity.listOfSigns[signID].symbol;
        }

        else
        {
            gameObject.SetActive(false);
            textMesh.text = "";
        }
    }
}
