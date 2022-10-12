using UnityEngine;

public class PlanetInfoBox : InfoBox
{
    [SerializeField] GameObject descriptionDignitiesHeader;
    [SerializeField] GameObject descriptionDignitiesInfo;

    override public int AstroID
    {
        get { return astroID; }
        set {
            astroID = value;

            EventManager.Instance.ApplyPlanetIdentity(astroID, this);

            ToggleDignitiesByPlanetGroup();
        }
    }

    void ToggleDignitiesByPlanetGroup()
    {

        // show essential dignities only for traditional planets
        if (astroID < 7)
        {
            descriptionDignitiesInfo.SetActive(true);
            descriptionDignitiesHeader.SetActive(true);
            return;
        }

        descriptionDignitiesInfo.SetActive(false);
        descriptionDignitiesHeader.SetActive(false);
    }

    private void OnDisable()
    {
        EventManager.Instance.ClosePlanetBox(AstroID);
    }

    private void OnDestroy()
    {
        InfoBoxSpawner.planetInfoBoxes.Remove(this);
    }

}
