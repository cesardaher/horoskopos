public class ExiledPlanetSymbolGetter : PlanetDataGetter
{
    protected override void GetSignInfo(int signId, SignInfoBox box)
    {
        base.GetSignInfo(signId, box);

        int planetId = astroIdentity.listOfSigns[signId].exile;
        textMesh.text = astroIdentity.listOfPlanets[planetId].symbol;
    }
}
