public class DomicileChecker : PlanetDataGetter
{
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        int signID = PlanetData.PlanetDataList[planetId].SignID;
        if (planetId == astroIdentity.listOfSigns[signID].rulerId)
        {
            textMesh.text = "In domicile";
        }

        else textMesh.text = "--";
    }
}
