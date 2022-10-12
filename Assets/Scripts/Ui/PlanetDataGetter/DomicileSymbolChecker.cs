public class DomicileSymbolChecker : PlanetDataGetter
{
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);
        int signID = PlanetData.PlanetDataList[planetId].SignID;
        if (planetId == astroIdentity.listOfSigns[signID].rulerId)
        {
            gameObject.SetActive(true);
            textMesh.text = astroIdentity.listOfSigns[signID].symbol;
        }
        else
        {
            if(gameObject != null)
            {
                gameObject.SetActive(false);
                textMesh.text = "";
            }
        }
    }
}
