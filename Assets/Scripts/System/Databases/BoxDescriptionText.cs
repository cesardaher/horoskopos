using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu][Serializable]
public class BoxDescriptionText : ScriptableObject
{
    public List<DescriptionText> planetInformationTexts;
    public List<DescriptionText> signInformationTexts;
    public List<DescriptionText> houseInformationTexts;

    [Serializable]
    public class DescriptionText
    {
        public int id;
        public string name;
        [TextArea(10, 15)]
        public string description;
    }

    private void OnValidate()
    {
        for(int i = 0; i < planetInformationTexts.Count; i++)
        {
            planetInformationTexts[i].id = i;
        }

        for (int i = 0; i < signInformationTexts.Count; i++)
        {
            signInformationTexts[i].id = i;
        }

        for (int i = 0; i < houseInformationTexts.Count; i++)
        {
            houseInformationTexts[i].id = i;
        }
    }
}
