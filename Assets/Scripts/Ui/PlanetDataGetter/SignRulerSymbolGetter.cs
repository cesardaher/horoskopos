public class SignRulerSymbolGetter : PlanetDataGetter
{
    protected override void GetSignInfo(int signId, SignInfoBox box)
    {
        base.GetSignInfo(signId, box);

        int rulerId = astroIdentity.listOfSigns[signId].rulerId;
        textMesh.text = astroIdentity.listOfPlanets[rulerId].symbol;
    }
}
