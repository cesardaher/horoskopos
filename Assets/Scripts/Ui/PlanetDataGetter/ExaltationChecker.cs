public class ExaltationChecker : PlanetDataGetter
{    
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        int signID = PlanetData.PlanetDataList[planetId].SignID;
        if (signID == astroIdentity.listOfPlanets[planetId].exaltation)
        {
            textMesh.text = "Exalted";
        }

        else textMesh.text = "--";
    }
}
