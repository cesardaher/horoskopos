using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetSetDropdown : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] List<TogglePlanetOnSettings> planetTogglers;
    List<string> planetarySets = new List<string> { "Traditional", "Trad. + Nodes", "Modern", "None" };

    private void Start()
    {
        PopulateDropdown();
    }

    void PopulateDropdown()
    {
        // populate dropdown with house systems
        foreach (string item in planetarySets)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item });
        }

        // set traditional planets as default
        dropdown.value = 0;
        dropdown.captionText.text = planetarySets[0];
        DropdownItemSelected(dropdown);

        // add function to OnValueChanged
        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });
    }

    void DropdownItemSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;

        ChoosePlanetSet(index);
       
    }

    void ChoosePlanetSet(int i)
    {
        switch(i)
        {
            case 0:
                TurnOnTraditionalSet();
                break;
            case 1:
                TurnOnTraditionalNodesSet();
                break;
            case 2:
                TurnOnModernSet();
                break;
            case 3:
                TurnOnNoneSet();
                break;
        }    
    }

    void TurnOnTraditionalSet()
    {
        foreach(TogglePlanetOnSettings toggler in planetTogglers)
        {
            // turn on traditional planets
            if(toggler.planetId < 7)
            {
                if (!toggler.active) toggler.TogglePlanetVisibility();
                continue;
            }

            // turn off modern planets and nodes
            if (toggler.active) toggler.TogglePlanetVisibility();
        }

        EventManager.Instance.ToggleMultiplePlanets();
    }

    void TurnOnTraditionalNodesSet()
    {
        foreach (TogglePlanetOnSettings toggler in planetTogglers)
        {
            // turn on traditional planets and nodes
            if (toggler.planetId < 7 || toggler.planetId == 10)
            {
                if (!toggler.active) toggler.TogglePlanetVisibility();
                continue;
            }

            // turn off modern planets
            if (toggler.active) toggler.TogglePlanetVisibility();
        }

        EventManager.Instance.ToggleMultiplePlanets();
    }

    void TurnOnModernSet()
    {
        foreach (TogglePlanetOnSettings toggler in planetTogglers)
        {
            // turn on all planets
            
            if (!toggler.active) toggler.TogglePlanetVisibility();
        }

        EventManager.Instance.ToggleMultiplePlanets();
    }

    void TurnOnNoneSet()
    {
        foreach (TogglePlanetOnSettings toggler in planetTogglers)
        {
            // turn off all planets
            if (toggler.active) toggler.TogglePlanetVisibility();
        }

        EventManager.Instance.ToggleMultiplePlanets();
    }
}
