using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownHandler : MonoBehaviour
{
    [SerializeField] GeoDataHandler dataManager;

    TMP_Dropdown dropdown;
    public int DropdownCount
    {
        get
        { return dropdown.options.Count; }
    }

    public int Value { get
        {
            return dropdown.value;
        }
    }
    public TextMeshProUGUI textBox;

    string sandbox = "Sandbox";
    string createNew = "New Profile";
    

    // Start is called before the first frame update
    void Start()
    {
        dropdown = transform.GetComponent<TMP_Dropdown>();

        dropdown.options.Clear();

        List<string> geoListNames = new List<string>();

        // add Sandbox option to list
        geoListNames.Add(sandbox);

        // add New Profile option to list
        geoListNames.Add(createNew);

        // define items
        geoListNames.AddRange(ListGeoData());


        // add items to dropdown menu
        foreach (string item in geoListNames)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item });
        }
        

        DropdownItemSelected(dropdown);

        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });
    }

    public void RestartDropdown()
    {
        dropdown.options.Clear();

        List<string> geoListNames = new List<string>();

        // add Sandbox option to list
        geoListNames.Add(sandbox);

        // add New Profile option to list
        geoListNames.Add(createNew);

        // define items
        geoListNames.AddRange(ListGeoData());


        // add items to dropdown menu
        foreach (string item in geoListNames)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item });
        }


        DropdownItemSelected(dropdown);

        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });
    }

    public List<string> ListGeoData()
    {
        List<string> geoListNames = new List<string>();

        foreach (GeoData geodata in dataManager.geoDataList)
        {
            geoListNames.Add(geodata.DataName);
        }

        return geoListNames;
    }

    void DropdownItemSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;

        textBox.text = dropdown.options[index].text;

        // if it's the option for sandbox
        if (index == 0)
        {
            dataManager.EnableSandboxOption();
            return;
        }

        // if it's the option to create new profile
        if (index == 1)
        {
            dataManager.CreateNewProfileOption();
            return;
        }

        // show profile values on screen
        // enable profile options
        dataManager.EqualizeValuesWithProfile(index - 2);
        dataManager.ProfileOption();
        dataManager.WasProfileChange = true;

    }

    public void SetSandboxAsLabel()
    {
        dropdown.value = 0;
    }

}
