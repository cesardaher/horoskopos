public class ExaltedPlanetNameGetter : PlanetDataGetter
{
    protected override void GetSignInfo(int signId, SignInfoBox box)
    {
        base.GetSignInfo(signId, box);

        int planetId = astroIdentity.listOfSigns[signId].exaltation;

        if (planetId != -1)
        {
            textMesh.text = astroIdentity.listOfPlanets[planetId].name;
            return;
        }

        textMesh.text = "--";
        
    }
}
