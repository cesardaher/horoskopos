using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FollowPlanetController : MonoBehaviour
{
    public int CurrentPlanet { get; private set; }
    AstrologicalIdentity astroIdentity;
    [SerializeField] List<Toggle> planetToggles;
    [SerializeField] TextMeshProUGUI label;

    private void Start()
    {
        astroIdentity = Resources.Load<AstrologicalIdentity>("AstroIdentities");
        EventManager.Instance.OnSelectFollowedPlanet += SelectPlanet;

        AssignIds();
        planetToggles[0].isOn = true;

    }

    public void AssignIds()
    {
        int id = 0;

        foreach (Toggle toggle in planetToggles)
        {
            toggle.GetComponent<PlanetToggle>().PlanetId = id;
            id++;
        }
    }


    public void SelectPlanet(int p)
    {
        CurrentPlanet = p;
        label.text = "Follow " + astroIdentity.listOfPlanets[p].name;

        for (int i = 0; i < planetToggles.Count; i++)
        {
            if (i == p) continue;

            planetToggles[i].isOn = false;
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnSelectFollowedPlanet -= SelectPlanet;
    }
}
