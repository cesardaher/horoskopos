public class SignNameGetter : PlanetDataGetter
{
    protected override void GetSignInfo(int signId, SignInfoBox box)
    {
        base.GetSignInfo(signId, box);

        textMesh.text = astroIdentity.listOfSigns[signId].title;
    }
}
