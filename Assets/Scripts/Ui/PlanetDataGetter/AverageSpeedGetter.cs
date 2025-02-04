public class AverageSpeedGetter : PlanetDataGetter
{
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        if (planetId == 10 || planetId == 11)
            textMesh.text = "--";
        else
            textMesh.text = AstroFunctions.DegreesStringFormat(PlanetData.PlanetDataList[planetId].speedAverageLonMinSec) + "/day";
    }
}
