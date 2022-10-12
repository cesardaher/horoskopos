public class PlanetSymbolGetter : PlanetDataGetter
{
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        textMesh.text = astroIdentity.listOfPlanets[planetId].symbol;
    }
}
