public class SectGetter : PlanetDataGetter
{
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        if (astroIdentity.listOfPlanets[planetId].sect == "")
        {
            gameObject.SetActive(false);
            textMesh.text = "--";
            return;
        }

        if(!gameObject.activeSelf) gameObject.SetActive(true);

        // Mercury is a special case
        if (planetId == 2)
        {
            textMesh.text = "<color=#FFFF00FF>Diurnal Sect as a morning star</color> \n Nocturnal Sect as an evening star \n";
            return;
        }

        textMesh.text = astroIdentity.listOfPlanets[planetId].sect + " Sect \n";
    }
}
