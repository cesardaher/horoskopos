using System;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

[CreateAssetMenu]
[Serializable]
public class CsvReader : ScriptableObject
{
    public TextAsset textAssetData;
    CultureInfo cultureInfo = new CultureInfo("pt-BR");

    [System.Serializable]
    public class City
    {  
        public string cityName; // 2 
        public string cityAscii; // 3
        public float latitude; // 5
        public float longitude; // 6
        public string countryCode; // 9
        public int elevation; // 16
        public string timezone; // 18
        public string cityId; //1
    }

    [System.Serializable]
    public class CityList
    {
        public City[] cities;

    }

    public CityList listOfCities = new CityList();
    public List<string> listOfCityNames = new List<string>();
    public List<string> listOfAsciiCityNames = new List<string>();

    void OnValidate()
    {
        ReadDatabase();
        ReadJustNames();
        ReadAsciiNames();
       
    }

    void ReadDatabase()
    {
        string[] data = textAssetData.text.Split(new string[] { "\t", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / 19;
        listOfCities.cities = new City[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            //Debug.Log(i);

            if(i == 0)
            {
                listOfCities.cities[i] = new City();
                continue;
            }

            listOfCities.cities[i] = new City();
            listOfCities.cities[i].cityId = data[19 * (i-1)];
            listOfCities.cities[i].cityName = data[19 * (i-1) + 1];
            listOfCities.cities[i].cityAscii = data[19 * (i - 1) + 2];
  
            var latitude = float.Parse(data[19 * (i - 1) + 4].Replace('.',','), cultureInfo);
            var longitude = float.Parse(data[19 * (i - 1) + 5].Replace('.', ','), cultureInfo);

            listOfCities.cities[i].latitude = latitude;
            listOfCities.cities[i].longitude = longitude;
            listOfCities.cities[i].countryCode = data[19 * (i - 1) + 8];
            listOfCities.cities[i].elevation = int.Parse(data[19 * (i - 1) + 16]);
            listOfCities.cities[i].timezone = data[19 * (i - 1) + 17];
        }
    }

    void ReadJustNames()
    {
        listOfCityNames.Clear();
        for (int i = 0; i < listOfCities.cities.Length; i++)
        {
            if (i == 0)
            {
                listOfCityNames.Add("");
                continue;
            }

            listOfCityNames.Add(listOfCities.cities[i].cityName + "/" + listOfCities.cities[i].countryCode);            
        }
    }

    void ReadAsciiNames()
    {
        listOfAsciiCityNames.Clear();

        for (int i = 0; i < listOfCities.cities.Length; i++)
        {
            if (i == 0)
            {
                listOfAsciiCityNames.Add("");
                continue;
            }

            listOfAsciiCityNames.Add(listOfCities.cities[i].cityAscii);
        }

    }

}
