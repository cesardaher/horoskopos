public class CurrentSpeedGetter : PlanetDataGetter
{
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        textMesh.text = AstroFunctions.DegreesStringFormat(PlanetData.PlanetDataList[planetId].speedLongMinSec) + "/day";

        if (PlanetData.PlanetDataList[planetId].Retrograde)
            textMesh.text += " (R)";
    }

}
