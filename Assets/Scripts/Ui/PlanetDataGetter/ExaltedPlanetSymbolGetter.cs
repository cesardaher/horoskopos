public class ExaltedPlanetSymbolGetter : PlanetDataGetter
{
    protected override void GetSignInfo(int signId, SignInfoBox box)
    {
        base.GetSignInfo(signId, box);

        int planetId = astroIdentity.listOfSigns[signId].exaltation;
        if (planetId != -1)
        {
            textMesh.text = astroIdentity.listOfPlanets[planetId].symbol;
            return;
        }

        textMesh.text = "";
    }
}
