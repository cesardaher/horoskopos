public class AboutPlanetGetter : PlanetDataGetter
{
    string defaultText;
    new private void Awake()
    {
        base.Awake();
        defaultText = textMesh.text;
    }

    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        textMesh.text = string.Format(defaultText, astroIdentity.listOfPlanets[planetId].name);
    }
}
