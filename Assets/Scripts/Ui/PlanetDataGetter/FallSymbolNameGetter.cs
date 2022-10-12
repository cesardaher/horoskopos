public class FallSymbolNameGetter : PlanetDataGetter
{
    protected override void GetSignInfo(int signId, SignInfoBox box)
    {
        base.GetSignInfo(signId, box);

        int planetId = astroIdentity.listOfSigns[signId].fall;

        if (planetId != -1)
        {
            textMesh.text = astroIdentity.listOfPlanets[planetId].symbol;
            return;
        }

        textMesh.text = "";

    }
}
