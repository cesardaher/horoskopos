using AstroResources;

public class LongitudeGetter : PlanetDataGetter
{
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        textMesh.text = AstroFunctions.DegreesStringFormat(PlanetData.PlanetDataList[planetId].LongMinSec);
    }

}
