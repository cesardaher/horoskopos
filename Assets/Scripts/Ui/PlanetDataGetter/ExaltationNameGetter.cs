public class ExaltationNameGetter : PlanetDataGetter
{
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        if (astroIdentity.listOfPlanets[planetId].exaltation == 0)
        {
            textMesh.text = "--";
            return;
        }

        int signID = astroIdentity.listOfPlanets[planetId].exaltation;
        textMesh.text = astroIdentity.listOfSigns[signID].name;
    }
}
