using System.Collections.Generic;
using TMPro;
using UnityEngine;
using AstroResources;

public class HouseSystemDropdownHandler : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;

    private void Start()
    {
        PopulateDropdown();   
    }

    void PopulateDropdown()
    {
        // populate dropdown with house systems
        foreach(KeyValuePair<char, string> entry in AstrologicalDatabase.houseSystemsByChar)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = entry.Value });
        }

        // whole signs is default house system, as it is always the last of the list
        dropdown.value = dropdown.options.Count - 1;

        // add function to OnValueChanged
        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });
    }

    void DropdownItemSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;
        string hsysText = dropdown.options[index].text;
        char houseSystemChar = AstrologicalDatabase.houseSystemsByName[hsysText];

        EventManager.Instance.RecalculateHouses(houseSystemChar);

    }
}
