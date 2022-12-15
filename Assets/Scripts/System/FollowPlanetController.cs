using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FollowPlanetController : MonoBehaviour
{
    public int CurrentPlanet { get; private set; }
    [SerializeField] Toggle mainToggle;
    [SerializeField] AstrologicalIdentity astroIdentity;
    [SerializeField] List<PlanetToggleGroup> toggleGroups = new List<PlanetToggleGroup>();
    [SerializeField] TextMeshProUGUI label;

    void Start()
    {
        EventManager.Instance.OnSelectFollowedPlanet += SelectPlanet;
        EventManager.Instance.OnMultiplePlanetsToggle += ChangeSelectablePlanets;

        AssignPlanetIds();
        toggleGroups[0].toggle.isOn = true;
    }

    void AssignPlanetIds()
    {
        foreach (var group in toggleGroups)
            group.planetSelecter.PlanetId = group.id;
    }


    void SelectPlanet(int p)
    {
        CurrentPlanet = p;
        label.text = "Follow " + astroIdentity.listOfPlanets[p].name;

        for (int i = 0; i < toggleGroups.Count; i++)
        {
            if (i == p) continue;

            toggleGroups[i].toggle.isOn = false;
        }
    }

    void ChangeSelectablePlanets()
    {
        foreach(var planet in PlanetData.PlanetDataList)
        {
            int id = planet.planetID;

            // for disabled planets
            if(!planet.IsActive)
            {
                toggleGroups[id].toggle.interactable = false;

                FailsafeDisabledPlanets(id);
            }
            else
                toggleGroups[id].toggle.interactable = true;
        }

        // if current planet is disabled, stop following
        // select sun by default
        // if sun is also disabled, disable following until available planet is selected
        void FailsafeDisabledPlanets(int id)
        {
            if (id == CurrentPlanet)
            {
                mainToggle.isOn = false;
                toggleGroups[0].toggle.isOn = true;
                SelectPlanet(0);
            }
        }
    }

    void OnDestroy()
    {
        EventManager.Instance.OnSelectFollowedPlanet -= SelectPlanet;
        EventManager.Instance.OnMultiplePlanetsToggle -= ChangeSelectablePlanets;
    }

    

    [System.Serializable]
    public class PlanetToggleGroup
    {
        public int id;
        public Toggle toggle;
        public FollowPlanetSelecter planetSelecter;

        public PlanetToggleGroup(int id, Toggle toggle, FollowPlanetSelecter planetSelecter)
        {
            this.id = id;
            this.toggle = toggle;
            this.planetSelecter = planetSelecter;
        }
    }
}
