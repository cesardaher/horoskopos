using TMPro;
using UnityEngine;
using AstroResources;

public class PlanetDataGetter : MonoBehaviour
{
    //protected int planetBoxID;
    protected int signBoxID;

    [SerializeField] protected InfoBox infoBox;
    protected TextMeshProUGUI textMesh;
    protected AstrologicalIdentity astroIdentity;


    protected void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        astroIdentity = Resources.Load<AstrologicalIdentity>("AstroIdentities");

        EventManager.Instance.OnPlanetBoxSelect += GetPlanetInfoParent;
        EventManager.Instance.OnSignBoxSelect += GetSignInfoParent;
        EventManager.Instance.OnCalculatedPlanet += RefreshBoxData;

    }

    private void OnDestroy()
    {
        EventManager.Instance.OnPlanetBoxSelect -= GetPlanetInfoParent;
        EventManager.Instance.OnSignBoxSelect -= GetSignInfoParent;
        EventManager.Instance.OnCalculatedPlanet -= RefreshBoxData;
    }
    protected void GetPlanetInfoParent(int planetID, PlanetInfoBox box)
    {
        if (box != infoBox) return;
        if (!infoBox.gameObject.activeSelf) return;

        GetPlanetInfo(planetID, box);
    }

    protected virtual void GetPlanetInfo(int planetID, PlanetInfoBox box)
    {

    }

    protected virtual void GetSignInfoParent(int signID, SignInfoBox box)
    {
        if (box != infoBox) return;
        if (!infoBox.gameObject.activeSelf) return;
        GetSignInfo(signID, box);
    }

    protected virtual void GetSignInfo(int signID, SignInfoBox box)
    {

    }


    private void RefreshBoxData(int astroID)
    {
        if (!infoBox.gameObject.activeSelf) return;

        if(infoBox is PlanetInfoBox)
        {
            // if given planet matches the corresponding infoBox, refresh
            if (astroID == infoBox.astroID)
            {
                PlanetInfoBox planetInfoBox = infoBox as PlanetInfoBox;
                GetPlanetInfoParent(astroID, planetInfoBox);
            }
            return;
        }

        
        if (infoBox is SignInfoBox)
        {
            // if given planet matches the corresponding infoBox, refresh
            if (astroID == infoBox.astroID)
            {
                SignInfoBox signInfoBox = infoBox as SignInfoBox;
                GetSignInfoParent(astroID, signInfoBox);
            }
            return;
        }
    }

    private void OnDisable()
    {
    }
}
