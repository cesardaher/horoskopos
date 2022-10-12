public class EssentialExileSymbolGetter : PlanetDataGetter
{
    public int id;

    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        // turn object off from parent
        // when sign has only one domicile
        if (astroIdentity.listOfPlanets[planetId].exiles.Length == 1 && id == 1)
        {
            transform.parent.gameObject.SetActive(false);
            return;
        }

        // empty for planets with no exiles
        if (astroIdentity.listOfPlanets[planetId].exiles.Length == 0)
        {
            //also turn off parent to make list smaller
            if (id == 1)
                transform.parent.gameObject.SetActive(false);

            textMesh.text = "";
            return;
        }


        if (!transform.parent.gameObject.activeSelf) transform.parent.gameObject.SetActive(true);
        int signID = astroIdentity.listOfPlanets[planetId].exiles[id];
        textMesh.text = astroIdentity.listOfSigns[signID].symbol;


    }
}
