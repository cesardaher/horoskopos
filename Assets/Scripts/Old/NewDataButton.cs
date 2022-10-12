using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class NewDataButton : MonoBehaviour
{
    [SerializeField] DropdownHandler dropdownHandler;
    [SerializeField] TitleScreenUI titleScreenUI;
    [SerializeField] TitleMenu titleMenu;
    [SerializeField] TMP_Text errorText;

    [Header("Databases")]
    [SerializeField] CsvReader database;
    [SerializeField] TimezoneReader timezoneDb;

    [Header("Data fields")]
    [SerializeField] TMP_InputField dataName;
    [SerializeField] TMP_Dropdown hour;
    [SerializeField] TMP_Dropdown minute;
    [SerializeField] TMP_Dropdown day;
    [SerializeField] TMP_Dropdown month;
    [SerializeField] TMP_InputField year;
    [SerializeField] Toggle daylightSavings;

    public void CreateNewGeoData()
    {
        //get location data
        double latitudeValue, longitudeValue;
        float timezoneValue;
        int height;
        GetLocationData(out latitudeValue, out longitudeValue, out timezoneValue, out height);

        GeoData gd = ScriptableObject.CreateInstance<GeoData>();
        if (!validateData(gd)) return;


        gd.InitializeData(
            dataName.text,
            int.Parse(day.captionText.text),
            int.Parse(month.captionText.text),
            int.Parse(year.text),
            int.Parse(hour.captionText.text),
            int.Parse(minute.captionText.text),
            0,
            timezoneValue,
            latitudeValue,
            longitudeValue,
            height,
            'P',
            false);

        SaveManager.StoreNewData(gd);
        titleScreenUI.geoList.Add(gd);

        dropdownHandler.RestartDropdown();
    }

    

    bool validateData(GeoData gd)
    {
        int dayValue, monthValue, yearValue, hourValue, minuteValue;
        //double timezoneValue, latitudeValue, longitudeValue;

        //if (!checkTimezone(out timezoneValue)) return false;

        //if (!checkLatitude(out latitudeValue)) return false;

        //if (!checkLongitude(out longitudeValue)) return false;

        if (AutoCompleteDropdownHandler.currentCityIndex == 0)
        {
            errorText.text = "Please choose a city.";
            return false;
        }

        if (!checkDay(out dayValue)) return false;

        if (!checkMonth(out monthValue)) return false;

        if (!checkYear(out yearValue)) return false;

        if (!checkHour(out hourValue)) return false;

        if (!checkMinute(out minuteValue)) return false;

        if (!checkValidDates(dayValue, monthValue, yearValue)) return false;

        return true;
    }

    bool GetLocationData(out double latitudeValue, out double longitudeValue, out float timezoneValue, out int height)
    {
        int index = AutoCompleteDropdownHandler.currentCityIndex;

        height = database.listOfCities.cities[index].elevation;
        latitudeValue = database.listOfCities.cities[index].latitude;
        longitudeValue = database.listOfCities.cities[index].longitude;
        timezoneValue = FindTimezone(database.listOfCities.cities[index].timezone);

        return true;
    }

    float FindTimezone(string val)
    {
        int index = timezoneDb.listOfTimezoneNames.FindIndex(s => s == val);
        float timezoneOffset;

        if (daylightSavings)
        {
            timezoneOffset = timezoneDb.listOfTimezones.timezones[index].offsetDst;
        }
        else
        {
            timezoneOffset = timezoneDb.listOfTimezones.timezones[index].offset;
        }
        return timezoneOffset;
    }


    void ShowFieldError(string fieldName)
    {
        errorText.text = fieldName + " format is invalid.";

    }

    void ShowDropdownError(string fieldName)
    {
        errorText.text = "Please choose " + fieldName + ".";
    }

    bool checkDropdown(TMP_Dropdown dropdown, string fieldName, out int value)
    {
        bool success;

        success = int.TryParse(dropdown.captionText.text, out value);
        if(!success)
        {
            ShowDropdownError(fieldName);
            return false;
        }

        return true;
    }

    void ShowDateError()
    {
        errorText.text = "The date you entered is not valid. Please check the Day and Month.";
    }

    void ShowYearError()
    {
        errorText.text = "The year you entered is not supported. Please enter a value between 1 and 5399.";
    }

    bool checkValidDates(int dayValue, int monthValue, int yearValue)
    {
        bool isLeapYear = IsLeapYear(yearValue);

        // check months that don't have 31 days
        if (dayValue == 31)
        {
            if (monthValue == 2 ||
                 monthValue == 4 ||
                 monthValue == 6 ||
                 monthValue == 9 ||
                 monthValue == 11)
            {
                // error
                ShowDateError();
                return false;
            }
        }
        
        if(dayValue == 30)
        {
            if (monthValue == 2 ) // february
            {
                // error
                ShowDateError();
                return false;
            }
        }

        if(dayValue == 29) // 29 days
        {
            if(monthValue == 2 && !isLeapYear) // on february and not a leap year
            {
                // error
                ShowDateError();
                return false;
            }

        }

        return true;
    }

    bool IsLeapYear(int yearValue)
    {
        if(yearValue % 4 != 0) // not divisible by 4
        {
            return false;
        } 
        else if (yearValue % 100 != 0)
        {
            return true;
        }
        else if(yearValue % 400 != 0)
        {
            return false;
        }

        return true;
    }


    bool checkDay(out int dayValue)
    {
        string fieldName = "Day";

        return checkDropdown(day, fieldName, out dayValue);
    }

    bool checkMonth(out int monthValue)
    {
        string fieldName = "Month";

        return checkDropdown(month, fieldName, out monthValue);
    }

    bool checkYear(out int yearValue)
    {
        string fieldName = "Year";
        bool success;

        success = int.TryParse(year.text, out yearValue);
        if (success)
        {
            if (yearValue < 1 || yearValue > 5399)
            {
                ShowYearError();
                return false;
            }
        }
        else
        {
            ShowFieldError(fieldName);
            return false;
        }

        return true;
    }

    bool checkHour(out int hourValue)
    {
        string fieldName = "Hour";

        return checkDropdown(hour, fieldName, out hourValue);
    }

    bool checkMinute(out int minuteValue)
    {
        string fieldName = "Minute";

        return checkDropdown(minute, fieldName, out minuteValue);
    }

    /* Location data on old system
     * 
    bool checkTimezone(out double timezoneValue)
    {
        string fieldName = "Timezone";
        bool success;

        success = double.TryParse(timezone.text, out timezoneValue);
        if (success)
        {
            if (timezoneValue < -12 || timezoneValue > 14)
            {
                ShowFieldError(fieldName);
                return false;
            }
        }
        else
        {
            ShowFieldError(fieldName);
            return false;
        }

        return true;
    }

    bool checkLatitude(out double latitudeValue)
    {
        string fieldName = "Latitude";
        bool success;

        success = double.TryParse(latitude.text, out latitudeValue);
        if (success)
        {
            if (latitudeValue < -90 || latitudeValue > 90)
            {
                ShowFieldError(fieldName);
                return false;
            }
        }
        else
        {
            ShowFieldError(fieldName);
            return false;
        }

        return true;
    }
    
    bool checkLongitude(out double longitudeValue)
    {
        string fieldName = "Longitude";
        bool success;

        success = double.TryParse(longitude.text, out longitudeValue);
        if (success)
        {
            if (longitudeValue < -180 || longitudeValue > 180)
            {
                ShowFieldError(fieldName);
                return false;
            }
        }
        else
        {
            ShowFieldError(fieldName);
            return false;
        }

        return true;

    }*/
}
