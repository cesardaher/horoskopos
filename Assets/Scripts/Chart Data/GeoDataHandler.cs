using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeoDataHandler : MonoBehaviour
{
    bool wasProfileChanged;
    [field: SerializeField]public bool WasProfileChange { get; set; }

    [Header("Data Time")]
    [SerializeField] DateTime _currentDateTime;

    [Header("Dropdown")]
    [SerializeField] DropdownHandler _dropdown;

    [Header("Data Panel")]
    [SerializeField] DataPanelOpener _dataPanelOpener;

    [Header("Input Fields")]
    [SerializeField] TMP_InputField _nameInput;
    [SerializeField] TMP_Dropdown _hourInput;
    [SerializeField] TMP_Dropdown _minInput;
    [SerializeField] TMP_Dropdown _dayInput;
    [SerializeField] TMP_Dropdown _monInput;
    [SerializeField] TMP_InputField _yearInput;
    [SerializeField] Toggle _daylightSavings;
    [SerializeField] AutoCompleteDropdownHandler _autoCompleteDropdown;

    [Header("Input Field Groups")]
    [SerializeField] GameObject _nameGroup;
    [SerializeField] GameObject _hourGroup;
    [SerializeField] GameObject _dateGroup;
    [SerializeField] GameObject _cityGroup;
    [SerializeField] GameObject _daylightSavGroup;

    [Header("Text Headers")]
    [SerializeField] TextMeshProUGUI _nameHeader;
    [SerializeField] TextMeshProUGUI _dateHeader;
    [SerializeField] TextMeshProUGUI _timeHeader;
    [SerializeField] TextMeshProUGUI _cityHeader;

    [Header("Buttons")]
    [SerializeField] Button _createProfileButton;
    [SerializeField] Button _updateProfileButton;
    [SerializeField] Button _refreshSkyButton;
    [SerializeField] Button _currentTimeButton;

    public List<GeoData> geoDataList = new List<GeoData>();

    private void Awake() => LoadGeoDataFromJSON();   

    private void Start()
    {
        EqualizeInputAndUITextObjects();


        EventManager.Instance.OnRecalculationOfGeoData += EqualizeInputAndUITextObjects;
    }

    private void OnDestroy() => EventManager.Instance.OnRecalculationOfGeoData -= EqualizeInputAndUITextObjects;
    

    private void Update() => EnableDataHandlingButtons();


    void LoadGeoDataFromJSON() => geoDataList = SaveManager.LoadFiles();

    public void OverwriteProfile()
    {
        GeoData gd = ScriptableObject.CreateInstance<GeoData>();

        // normal constructor when city is selected
        if(AutoCompleteDropdownHandler.currentCityIndex != 0)
            gd.InitializeDataWithCityIdDateTime(_nameInput.text, _currentDateTime, AutoCompleteDropdownHandler.currentCityIndex, GeoData.ActiveData.Hsys, _daylightSavings.isOn);
        else
            gd.InitializeDataDateTime(_nameInput.text, _currentDateTime, GeoData.ActiveData.Geolat, GeoData.ActiveData.Geolon, GeoData.ActiveData.Height, GeoData.ActiveData.D_timezone, GeoData.ActiveData.Hsys, _daylightSavings.isOn);

        SaveManager.StoreNewData(gd);
        LoadGeoDataFromJSON();
    }


    public void CreateNewProfileOption()
    {
        // activate name interaction
        _nameInput.interactable = true;

        // activate all text fields and headers
        _nameHeader.gameObject.SetActive(true);
        _dateHeader.gameObject.SetActive(true);
        _timeHeader.gameObject.SetActive(true);
        _cityHeader.gameObject.SetActive(true);
        _nameGroup.SetActive(true);
        _dateGroup.SetActive(true);
        _hourGroup.SetActive(true);
        _cityGroup.SetActive(true);
        _daylightSavGroup.SetActive(true);

        // enable creating new profile
        _createProfileButton.gameObject.SetActive(true);

        // deactivate other buttons
        _updateProfileButton.gameObject.SetActive(false);
        _currentTimeButton.gameObject.SetActive(false);
        _refreshSkyButton.gameObject.SetActive(false);
    }

    public void EnableSandboxOption()
    {
        // empty and deactivate name
        _nameInput.text = "";
        _nameInput.interactable = false;

        // activate all text fields and headers
        _nameHeader.gameObject.SetActive(true);
        _dateHeader.gameObject.SetActive(true);
        _timeHeader.gameObject.SetActive(true);
        _cityHeader.gameObject.SetActive(true);
        _nameGroup.SetActive(true);
        _dateGroup.SetActive(true);
        _hourGroup.SetActive(true);
        _cityGroup.SetActive(true);
        _daylightSavGroup.SetActive(true);

        // enable getting current time from computer
        // enable refreshing sky
        _currentTimeButton.gameObject.SetActive(true);
        _refreshSkyButton.gameObject.SetActive(true);

        // deactivate other buttons
        _updateProfileButton.gameObject.SetActive(false);
        _createProfileButton.gameObject.SetActive(false);
    }

    public void ProfileOption()
    {
        // activate name interaction
        _nameInput.interactable = false;

        // activate all text fields and headers
        _nameHeader.gameObject.SetActive(true);
        _dateHeader.gameObject.SetActive(true);
        _timeHeader.gameObject.SetActive(true);
        _cityHeader.gameObject.SetActive(true);
        _nameGroup.SetActive(true);
        _dateGroup.SetActive(true);
        _hourGroup.SetActive(true);
        _cityGroup.SetActive(true);
        _daylightSavGroup.SetActive(true);

        // enable editing profile
        _updateProfileButton.gameObject.SetActive(true);
        _refreshSkyButton.gameObject.SetActive(true);

        // deactivate other buttons
        _currentTimeButton.gameObject.SetActive(false);
        _createProfileButton.gameObject.SetActive(false);

        _autoCompleteDropdown.dropdown.Hide();
        _autoCompleteDropdown.inputField.DeactivateInputField();
    }

    public void UpdateGeoData()
    {
        if (AutoCompleteDropdownHandler.currentCityIndex != 0)
        {
            GeoData.ActiveData.CityId = AutoCompleteDropdownHandler.currentCityIndex;
        }

        // substitute geodata instead intializing new one
        if (wasProfileChanged && _dropdown.Value > 1)
        {
            GeoData.ActiveData = Instantiate(geoDataList[_dropdown.Value - 2]);
            return;
        }

        GeoData.ActiveData.InitializeDataDateTime(
            _nameInput.text, 
            _currentDateTime,
            GeoData.ActiveData.D_timezone,
            GeoData.ActiveData.Geolat,
            GeoData.ActiveData.Geolon,
            GeoData.ActiveData.Height,
            GeoData.ActiveData.Hsys,
            _daylightSavings.isOn);
    }

    public void CreateNewData()
    {
        GeoData gd = ScriptableObject.CreateInstance<GeoData>();

        gd.InitializeDataWithCityIdDateTime(_nameInput.text, _currentDateTime, AutoCompleteDropdownHandler.currentCityIndex, GeoData.ActiveData.Hsys, _daylightSavings.isOn);

        SaveManager.StoreNewData(gd);

        LoadGeoDataFromJSON();
    }

    // Get data from active GeoData object and apply it to both input and header text objects
    public void EqualizeInputAndUITextObjects()
    {
        _nameInput.text = GeoData.ActiveData.DataName;
        _dayInput.value = GeoData.ActiveData.Iday;
        _monInput.value = GeoData.ActiveData.Imon;
        _yearInput.text = GeoData.ActiveData.Iyear.ToString();
        _hourInput.value = GeoData.ActiveData.Ihour + 1;
        _minInput.value = GeoData.ActiveData.Imin + 1;
        _daylightSavings.isOn = GeoData.ActiveData.DaylightSavings;
        _autoCompleteDropdown.inputField.text = GeoData.ActiveData.City;

        // deactivate input field to prevent blockers from appearing
        _autoCompleteDropdown.dropdown.Hide();
        _autoCompleteDropdown.inputField.DeactivateInputField();

    }

    // 
    public void EqualizeValuesWithProfile(int profileId)
    {
        _nameInput.text = geoDataList[profileId].DataName;
        _dayInput.value = geoDataList[profileId].Iday;
        _monInput.value = geoDataList[profileId].Imon;
        _yearInput.text = geoDataList[profileId].Iyear.ToString();
        _hourInput.value = geoDataList[profileId].Ihour + 1;
        _minInput.value = geoDataList[profileId].Imin + 1;
        _daylightSavings.isOn = geoDataList[profileId].DaylightSavings;
        _autoCompleteDropdown.inputField.text = geoDataList[profileId].City;
        AutoCompleteDropdownHandler.currentCityIndex = geoDataList[profileId].CityId;
    }

    void EnableDataHandlingButtons()
    {
        if (!_dataPanelOpener.Opened) return;

        bool isDataValid = CheckIfValidData(out _currentDateTime);

        // turn off continue button if data is not filled correctly
        // this doesn't allow empty empty fields
        if (!isDataValid)
        {
            _refreshSkyButton.interactable = false;
            _updateProfileButton.interactable = false;
            _createProfileButton.interactable = false;
            return;
        }

        // turn on continue button if all data is filled
        if (isDataValid)
        {
            _refreshSkyButton.interactable = true;
            _updateProfileButton.interactable = true;
            _createProfileButton.interactable = true;
            return;
        }

        // returns true only when all data fields are filled
        bool CheckIfValidData(out DateTime dateTime)
        {
            dateTime = CreateNewDateTime();
            if (dateTime == DateTime.MaxValue)
                return false;
            if (_hourInput.value == 0)
                return false;
            if (_minInput.value == 0)
                return false;
            if (_dayInput.value == 0)
                return false;
            if (_monInput.value == 0)
                return false;
            if (_yearInput.text == "")
                return false;
            if (dateTime.Year > 5399)
                return false;
            if (_autoCompleteDropdown.inputField.text == "")
                return false;

            return true;
        }

        DateTime CreateNewDateTime()
        {
            DateTime dateTime;
            try
            {
                dateTime = new DateTime(int.Parse(_yearInput.text), _monInput.value, _dayInput.value, _hourInput.value - 1, _minInput.value - 1, 0);
            }
            catch
            {
                dateTime = DateTime.MaxValue;
            }

            return dateTime;
        }
    }

    // get current time from PC
    public void GetCurrentTime()
    {
        DateTime moment = DateTime.Now;

        _dayInput.value = moment.Day;
        _monInput.value = moment.Month;
        _yearInput.text = moment.Year.ToString();
        _hourInput.value = moment.Hour + 1;
        _minInput.value = moment.Minute + 1;
    }

}
