using UnityEngine;

public class GetColor : MonoBehaviour
{
    [SerializeField] protected InfoBox infoBox;
    [SerializeField] protected AstrologicalIdentity _astroIdentities;

    protected void Awake()
    {
        _astroIdentities = Resources.Load<AstrologicalIdentity>("AstroIdentities");

        EventManager.Instance.OnPlanetInfoBoxInitialization += AssignPlanetInfoBox;
        EventManager.Instance.OnCalculatedPlanet += RefreshBoxData;
        EventManager.Instance.OnPlanetBoxSelect += GetGraphicColorParent;
        EventManager.Instance.OnSignBoxSelect += GetGraphicColorParent;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnPlanetInfoBoxInitialization -= AssignPlanetInfoBox;
        EventManager.Instance.OnCalculatedPlanet -= RefreshBoxData;
        EventManager.Instance.OnPlanetBoxSelect -= GetGraphicColorParent;
        EventManager.Instance.OnSignBoxSelect -= GetGraphicColorParent;
    }

    private void Start()
    {

    }
    protected void GetGraphicColorParent(int id, PlanetInfoBox box)
    {
        if (infoBox != box) return;
        if (!infoBox.gameObject.activeSelf) return;

        GetGraphicColor(id, box);
    }

    protected void GetGraphicColorParent(int id, SignInfoBox box)
    {
        if (infoBox != box) return;
        if (!infoBox.gameObject.activeSelf) return;

        GetGraphicColor(id, box);
    }

    protected virtual void GetGraphicColor(int id, PlanetInfoBox box)
    {

    }

    protected virtual void GetGraphicColor(int id, SignInfoBox box)
    {
    }

    private void RefreshBoxData(int astroID)
    {
        if (infoBox is PlanetInfoBox)
        {
            // if given planet matches the corresponding infoBox, refresh
            if (astroID == infoBox.astroID)
            {
                PlanetInfoBox planetInfoBox = infoBox as PlanetInfoBox;
                GetGraphicColorParent(astroID, planetInfoBox);
            }
            return;
        }

        /*
        if (infoBox is SignInfoBox)
        {
            // if given planet matches the corresponding infoBox, refresh
            if (astroID == infoBox.astroID)
            {
                SignInfoBox signInfoBox = infoBox as SignInfoBox;
                GetGraphicColorParent(astroID, signInfoBox);
            }
            return;
        }*/
    }

    void AssignPlanetInfoBox(PlanetInfoBox box)
    {
        infoBox = box;
    }
}
