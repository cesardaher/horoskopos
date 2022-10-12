using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDataManager : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_Dropdown hourInput;
    public TMP_Dropdown minInput;
    public TMP_Dropdown dayInput;
    public TMP_Dropdown monInput;
    public TMP_InputField yearInput;
    public Toggle daylightSavings;
    public Toggle unknownTime;
    public AutoCompleteDropdownHandler cityAutoComplete;

    [Header("Buttons")]
    public Button birthDataContinueButton;

    [Header("CheckPoints")]
    public bool isOnBirthDateInput;
    public static bool isUsingBirthChart; // changed from tutorial button
    public static bool isBirthTimeUnknown;
    public static bool isUsingDaylightSavings;

    private void Update()
    {
        if (isOnBirthDateInput)
        {
            EnableContinueButtonOnBirthData();
        }
    }

    // Used on Button for birth date input
    public void CreateGeoDataForBirthDate()
    {
        GeoData gd = ScriptableObject.CreateInstance<GeoData>();

        if(!isUsingDaylightSavings)
        {
            gd.InitializeDataWithCityId(
                "",
                int.Parse(dayInput.captionText.text),
                int.Parse(monInput.captionText.text),
                int.Parse(yearInput.text),
                int.Parse(hourInput.captionText.text),
                int.Parse(minInput.captionText.text),
                0,
                AutoCompleteDropdownHandler.currentCityIndex,
                'W',
                isUsingDaylightSavings);
        }
        else
        {
            gd.InitializeDataWithCityId(
                "",
                int.Parse(dayInput.captionText.text),
                int.Parse(monInput.captionText.text),
                int.Parse(yearInput.text),
                12,
                30,
                0,
                AutoCompleteDropdownHandler.currentCityIndex,
                'W',
                false);
        }

        GeoData.ActiveData = gd;
    }

    public void CreateGeoDataForCurrentTime()
    {
        GeoData gd = ScriptableObject.CreateInstance<GeoData>();

        DateTime dateTime = DateTime.Now;
        DateTimeOffset dateTimeOffset = DateTimeOffset.Now;
        DateTimeOffset dateTimeUtc = DateTimeOffset.UtcNow;
        TimeSpan diff = dateTime - dateTimeUtc;
        double utcOffset = dateTimeOffset.Offset.Hours;

        gd.InitializeDataWithCityIdAndTimeOffset(
            "", 
            dateTime.Day, 
            dateTime.Month,
            dateTime.Year,
            dateTime.Hour, 
            dateTime.Minute,
            dateTime.Second,
            10336, // TODO: CHANGE HARD CODING
            'W',
            utcOffset);

        GeoData.ActiveData = gd;
    }

    // to be used with Continue Button 
    public void ToggleBirthDateInputState(bool val)
    {
        isOnBirthDateInput = val;
    }

    void EnableContinueButtonOnBirthData()
    {
        bool isReadyToStart = CheckIfValidData();

        // turn off continue button if all data is not filled
        if (!isReadyToStart)
        {
            birthDataContinueButton.interactable = false;
            return;
        }

        // turn on continue button if all data is filled
        if (!birthDataContinueButton.interactable && isReadyToStart)
            birthDataContinueButton.interactable = true;

        bool CheckIfValidData()
        {
            if (hourInput.value == 0)
                return false;
            if (minInput.value == 0)
                return false;
            if (dayInput.value == 0)
                return false;
            if (monInput.value == 0)
                return false;
            if (yearInput.text == "")
                return false;
            if (AutoCompleteDropdownHandler.currentCityIndex == 0)
                return false;

            return true;
        }
    }

    public void ConfirmBirthChartUsage()
    {
        isUsingBirthChart = true;
    }

    public void ConfirmCurrentChartUsage()
    {
        isUsingBirthChart = false;
    }

}
