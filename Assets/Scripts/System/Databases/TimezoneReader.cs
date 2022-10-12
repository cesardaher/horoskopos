using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class TimezoneReader : ScriptableObject
{
    public TextAsset textAssetData;

    [System.Serializable]
    public class Timezone
    {
        public string timezoneName;
        public float offset;
        public float offsetDst;

    }

    [System.Serializable]
    public class TimezoneList
    {
        public Timezone[] timezones;
    }

    public TimezoneList listOfTimezones = new TimezoneList();
    public List<string> listOfTimezoneNames = new List<string>();

    private void OnValidate()
    {
        ReadDatabase();
        ReadJustNames();
    }

    void ReadDatabase()
    {
        string[] data = textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / 3 - 1;
        listOfTimezones.timezones = new Timezone[tableSize];

        for (int i = 0; i < tableSize; i++)
        {

            listOfTimezones.timezones[i] = new Timezone();
            listOfTimezones.timezones[i].timezoneName = (data[3 * (i + 1)]).Replace("\"", "");
            listOfTimezones.timezones[i].offset = float.Parse(data[3 * (i + 1) + 1].Replace("\"", "")) / 3600;
            listOfTimezones.timezones[i].offsetDst = float.Parse(data[3 * (i + 1) + 2].Replace("\"", "")) / 3600;
        }
    }

    void ReadJustNames()
    {
        listOfTimezoneNames.Clear();
        for (int i = 0; i < listOfTimezones.timezones.Length; i++)
        {
            listOfTimezoneNames.Add(listOfTimezones.timezones[i].timezoneName);
        }
    }
}
