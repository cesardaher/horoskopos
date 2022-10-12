using System.Collections.Generic;
using UnityEngine;

public class InfoBoxSpawner : MonoBehaviour
{
    [SerializeField] GameObject planetInfoBoxPrefab;
    [SerializeField] GameObject signInfoBoxPrefab;

    static public List<SignInfoBox> signInfoBoxes = new List<SignInfoBox>();
    static public List<PlanetInfoBox> planetInfoBoxes = new List<PlanetInfoBox>();

    private void Awake()
    {
        EventManager.Instance.OnPlanetSelect += SpawnPlanetBox;
        EventManager.Instance.OnSignSelect += SpawnSignBox;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnPlanetSelect -= SpawnPlanetBox;
        EventManager.Instance.OnSignSelect -= SpawnSignBox;
    }

    void SpawnPlanetBox(int planetId, Vector3 pos)
    {
        if(planetInfoBoxes.Count != 0)
        {
            // check if there is already a box with this planet
            foreach (PlanetInfoBox box in planetInfoBoxes)
            {
                if (box.astroID == planetId)
                {
                    // turn on box if off
                    if(!box.gameObject.activeSelf)
                        box.gameObject.SetActive(true);
                   
                    // bring box to mouse position and set color
                    box.transform.SetAsLastSibling();
                    box.transform.position = pos;
                    EventManager.Instance.ApplyPlanetIdentity(planetId, box);

                    return;
                }
            }
        }

        // create new Box and give it the appropriate Id
        PlanetInfoBox infoBox = Instantiate(planetInfoBoxPrefab.GetComponent<PlanetInfoBox>(), transform);
        infoBox.transform.position = pos;
        planetInfoBoxes.Add(infoBox);
        infoBox.AstroID = planetId;
        infoBox.gameObject.name = PlanetData.PlanetDataList[planetId].astroName + "Box";
    }

    void SpawnSignBox(int signId, Vector3 pos)
    {
        if (signInfoBoxes.Count != 0)
        {
            // check if there is already a box with this planet
            foreach (SignInfoBox box in signInfoBoxes)
            {
                if (box.astroID == signId)
                {
                    // turn on box if off
                    if (!box.gameObject.activeSelf)
                        box.gameObject.SetActive(true);

                    // bring box to mouse position
                    box.transform.SetAsLastSibling();
                    box.transform.position = pos;
                    EventManager.Instance.ApplySignIdentity(signId, box);

                    return;
                }
            }
        }

        // create new Box and give it the appropriate Id
        SignInfoBox infoBox = Instantiate(signInfoBoxPrefab.GetComponent<SignInfoBox>(), transform);
        infoBox.transform.position = pos;
        signInfoBoxes.Add(infoBox);

        infoBox.AstroID = signId;
    }
}
