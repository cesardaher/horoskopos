public class HouseNameGetter : PlanetDataGetter
{
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);
        textMesh.text = HouseInRomanNumerals(PlanetData.PlanetDataList[planetId].House);
    }

    string HouseInRomanNumerals(int val)
    {
        if (val == 1) return "I";
        if (val == 4) return "IV";
        if (val == 7) return "VII";
        if (val == 10) return "X";

        return val.ToString();
    }
}
