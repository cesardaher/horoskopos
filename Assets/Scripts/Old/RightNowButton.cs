using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unused
{
    public class RightNowButton : MonoBehaviour
    {
        [SerializeField] DropdownHandler dropdownHandler;
        [SerializeField] TitleScreenUI titleScreenUI;
        [SerializeField] TitleMenu titleMenu;
        [SerializeField] TMP_Text errorText;
        [SerializeField] CsvReader database;
        [SerializeField] TimezoneReader timezoneDb;
        [SerializeField] Toggle daylightSavings;


        public void CreateRightNow()
        {
            //only execute if the right now creator is on
            if (!titleScreenUI.rightNowCreator.activeSelf) return;

            System.DateTime moment = System.DateTime.Now;

            GeoData gd = ScriptableObject.CreateInstance<GeoData>();

            // validade the latitude, longitude and timezone data
            //if (!validateLatLonTz()) return;
            //
            GetLocationData(out double latitudeValue, out double longitudeValue, out float timezoneValue, out int height);

            Debug.Log(height);

            // initialize data with latitude, longitude, timezone
            // and current date and time data
            gd.InitializeData(
                "",
                moment.Day,
                moment.Month,
                moment.Year,
                moment.Hour,
                moment.Minute,
                0,
                timezoneValue,
                latitudeValue,
                longitudeValue,
                (double)height,
                'P',
                false);

            GeoData.ActiveData = gd;

            titleMenu.StartGame();
        }

        bool GetLocationData(out double latitudeValue, out double longitudeValue, out float timezoneValue, out int height)
        {
            int index = AutoCompleteDropdownHandler.currentCityIndex;

            height = database.listOfCities.cities[index].elevation;
            Debug.Log(height);
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

        // OLD SYSTEM
        /*
        bool validateLatLonTz()
        {
            double timezoneValue, latitudeValue, longitudeValue;

            if (!checkTimezone(out timezoneValue)) return false;

            if (!checkLatitude(out latitudeValue)) return false;

            if (!checkLongitude(out longitudeValue)) return false;

            Debug.Log("validated");

            return true;
        }

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

        bool checkLatitude(float latitude)
        {
            string fieldName = "Latitude";

            if (latitude < -90 || latitude > 90)
            {
                ShowFieldError(fieldName);
                return false;
            }

            return true;

        }

        bool checkLongitude(float longitude)
        {
            string fieldName = "Longitude";

            if (longitude < -180 || longitude > 180)
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

        }

        void ShowFieldError(string fieldName)
        {
            errorText.text = fieldName + " format is invalid.";

        }
        */
    }
}


