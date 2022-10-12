using System;
using UnityEngine;


[CreateAssetMenu]
[Serializable]
public class ConstellationDatabase : ScriptableObject
{
    [System.Serializable]
    public class Constellation
    {
        public string name;
        public string[] stars;
    }

    [System.Serializable]
    public class ConstellationList
    {
        public Constellation[] constellations;
    }

    public ConstellationList listOfConstellations = new ConstellationList();
}
