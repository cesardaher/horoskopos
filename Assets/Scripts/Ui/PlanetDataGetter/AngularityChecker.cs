public class AngularityChecker : PlanetDataGetter
{
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);
        int house = PlanetData.PlanetDataList[planetId].House;

        if(house == 1 || house == 4 || house == 7 || house == 10)
            textMesh.text = "Angular";
        else if (house == 2 || house == 5 || house == 8 || house == 11)
            textMesh.text = "Succedent";
        else if (house == 3 || house == 6 || house == 9 || house == 12)
            textMesh.text = "Cadent";
    }
}
