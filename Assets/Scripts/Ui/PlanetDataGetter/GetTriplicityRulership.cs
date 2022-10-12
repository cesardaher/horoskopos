public class GetTriplicityRulership : PlanetDataGetter
{
    public int id;
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        // empty components for planets with no rulership
        if (astroIdentity.listOfPlanets[planetId].triplicityRulership.Length == 0)
        {
            if(id == 0)
            {
                gameObject.SetActive(true);

                textMesh.text = "--"; 
                return;
            }

            gameObject.SetActive(false);
            return;
        }

        int elementID = astroIdentity.listOfPlanets[planetId].triplicityRulership[id];

        // hide component if there is no rulership on that period
        if (elementID == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        // assign string based on rulership
        string period;
        switch(id)
        {
            case 0:
                period = "Day";
                break;
            case 1:
                period = "Night";
                break;
            case 2:
                period = "Part.";
                break;
            default:
                period = "";
                break;
        }

        if (!gameObject.activeSelf) gameObject.SetActive(true);
        textMesh.text = string.Format("{0} - {1}", astroIdentity.listOfElements[elementID].name, period);
    }
}
