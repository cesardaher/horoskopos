using System;
using TMPro;
using UnityEngine;

public class ValueCollector : MonoBehaviour
{
    public enum Datatype // your custom enumeration
    {
        Name,
        Date,
        Time,
        LatLon,
        City
        
    };

    public Datatype datatype;

    TextMeshProUGUI _textField;
    private void Start()
    {
        EventManager.Instance.OnRecalculationOfGeoData += ChangeValue;

        _textField = GetComponent<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnRecalculationOfGeoData -= ChangeValue;
    }

    void ChangeValue()
    {

        switch (datatype)
        {
            case Datatype.Name:
                if (GeoData.ActiveData.DataName == "")
                    _textField.text = "--";
                else _textField.text = GeoData.ActiveData.DataName;               
                break;
            case Datatype.Date:
                _textField.text = ToDoubleDigit(TimeManager.Instance.ActiveDateTime.Day) + "." + ToDoubleDigit(TimeManager.Instance.ActiveDateTime.Month) + "." + TimeManager.Instance.ActiveDateTime.Year;
                break;
            case Datatype.Time:
                _textField.text = ToDoubleDigit(TimeManager.Instance.ActiveDateTime.Hour) + ":" + ToDoubleDigit(TimeManager.Instance.ActiveDateTime.Minute) + ":" + ToDoubleDigit(TimeManager.Instance.ActiveDateTime.Second) + " (" + GetSignTimezone(GeoData.ActiveData.F_timezone) + " UTC)";
                break;
            case Datatype.LatLon:
                _textField.text = GetLatitude(GeoData.ActiveData.Geolat) + " " + GetLongitude(GeoData.ActiveData.Geolon) + " " + GeoData.ActiveData.Height + "m";
                break;
            case Datatype.City:
                _textField.text = GeoData.ActiveData.City;
                break;
            default:
                Debug.Log(" no type");
                break;
        }

        string GetLatitude(double lat)
        {

            DecimalToMinSec(lat, out int[] latDegMinSec);

            string finalLat = Math.Abs(latDegMinSec[0]) + "°" + Math.Abs(latDegMinSec[1]) + "\'" + Math.Abs(latDegMinSec[2]) + "\"";

            if(lat >= 0)
                finalLat += "E";
            else
                finalLat += "W";

            return finalLat;
        }

        string GetLongitude(double lon)
        {

            DecimalToMinSec(lon, out int[] lonDegMinSec);

            string finalLon = Math.Abs(lonDegMinSec[0]) + "°" + Math.Abs(lonDegMinSec[1]) + "\'" + Math.Abs(lonDegMinSec[2]) + "\"";

            if (lon >= 0)
                finalLon += "N";
            else
                finalLon += "S";

            return finalLon;
        }

        string ToDoubleDigit(int val)
        {
            string finalVal = val.ToString();

            if(val < 10)
            {
                finalVal = "0" + finalVal;
                return finalVal;
            }

            return finalVal;
        }

        string GetSignTimezone(double val)
        {
            string finalVal = val.ToString();

            if (val > 0) finalVal = "+" + finalVal;

            return finalVal;
        }
    }

    void DecimalToMinSec(double val, out int[] degMinSec)
    {
        degMinSec = new int[3];

        degMinSec[0] = (int)Math.Truncate(val);

        double tempMin = val - Math.Truncate(val);

        degMinSec[1] = (int)Math.Truncate(tempMin * 60);

        double tempSec = tempMin * 60 - Math.Truncate(tempMin * 60);

        degMinSec[2] = (int)Math.Truncate(tempSec * 60);

    }
}
