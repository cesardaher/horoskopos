public class LatitudeGetter : PlanetDataGetter
{
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        textMesh.text = AstroFunctions.DegreesStringFormat(PlanetData.PlanetDataList[planetId].LatMinSec);
    }

}
