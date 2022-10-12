public class FallNameGetter : PlanetDataGetter
{
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        if (astroIdentity.listOfPlanets[planetId].fall == 0)
        {
            textMesh.text = "--";
            return;
        }

        int signID = astroIdentity.listOfPlanets[planetId].fall;
        textMesh.text = astroIdentity.listOfSigns[signID].name;
    }
}
