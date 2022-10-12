using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AutoCompleteDropdownHandler : MonoBehaviour
{
    // index that indicates position of city on database
    static public int currentCityIndex;
    public int currentCityIndexShowcase;
    // list of database indices on dropdown
    public List<int> indicesList;
    // copy of list used to check whether list has been changed
    List<int> _indicesListChecker;


    public TMP_InputField inputField;
    public TMP_Dropdown dropdown;
    public RectTransform item;
    public CsvReader database;

    // TODO: CHANGE THIS TO EVENT MANAGER
    private void Start()
    {
        inputField.onValueChanged.AddListener(OnInputValueChanged);
        _indicesListChecker = indicesList;

        if(GeoData.ActiveData != null)
            currentCityIndex = GeoData.ActiveData.CityId;
    }

    private void Update()
    {
        currentCityIndexShowcase = currentCityIndex;
        if(_indicesListChecker != indicesList) //check if indicesList changed
        {
            UpdateIndex();
            _indicesListChecker = indicesList;
        }
    }

    private void OnInputValueChanged(string newText)
    {
        ClearResults();

        if (inputField.text.Length >= 4) // show options when minimum character is 4
        {
            FillResults(GetResults(newText));
        }
    }

    private void ClearResults()
    {
        dropdown.ClearOptions();
        dropdown.Hide();
        inputField.ActivateInputField();
    }

    private void FillResults(List<string> results)
    {
        dropdown.interactable = true;
        dropdown.AddOptions(results);            
        dropdown.Show();
        dropdown.captionText.text = "";
        inputField.ActivateInputField();
    }
    
    // it was necessary to add an empty option at first to make sure the real options trigger value change
    private List<string> GetResults(string input)
    {
        // find indexes of names based on ASCII names
        var indexes = database.listOfAsciiCityNames.
            Select((element, index) => element.ToLower().IndexOf(input.ToLower()) >= 0 ? index : -1).
            Where(i => i >= 0).
            ToList<int>();

        // update indexes list if not empty
        // this prevents clearing the list when clicking on an option
        if(indexes.Count != 0)
        {
            indicesList = indexes;
        }

        // make a list of real names using these listings
        List<string> matchedNames = new List<string>();

        // add empty name
        matchedNames.Add(""); 
        foreach (int i in indexes)
        {
            matchedNames.Add(database.listOfCityNames[i]);
        }
        
        return matchedNames;
    }

    public void SelectToInputField(int val) // changes value on selection of option on dropdown
    {
        if (val == 0) return;

        inputField.text = dropdown.captionText.text;
        inputField.DeactivateInputField();

        //correct index to account for empty value
        int index = val - 1;

        currentCityIndex = indicesList[index];
    }

    void UpdateIndex()
    {
        currentCityIndex = indicesList[0]; //current index is equivalent to first item in dropdown
    }

    void DropdownItemSelected(int val)
    {

    }
}

