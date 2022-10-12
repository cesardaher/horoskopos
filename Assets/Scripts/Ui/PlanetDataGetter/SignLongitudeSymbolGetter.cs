public class SignLongitudeSymbolGetter : PlanetDataGetter
{
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);
        
        textMesh.text = astroIdentity.listOfSigns[PlanetData.PlanetDataList[planetId].SignID].symbol;
    }
}
