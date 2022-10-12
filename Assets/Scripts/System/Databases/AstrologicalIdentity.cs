using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AstrologicalIdentity : ScriptableObject
{
    [System.Serializable]
    public class Planet
    {
        public string name;
        public int id;
        public string symbol;
        public Color color;
        public Color secondaryColor;
        public int[] domiciles;
        public int[] exiles;
        public int exaltation;
        public int fall;
        public string sect;
        public int[] triplicityRulership = new int[3];
    }

    [System.Serializable]
    public class Sign
    {
        public string name;
        public string title;
        public int id;
        public int rulerId;
        public int element;
        public int mode;
        public int exaltation;
        public int fall;
        public int exile;
        public string symbol;
        public Color color;
        public Color secondaryColor;
    }

    [System.Serializable]
    public class Element
    {
        public string name;
        public int id;
        public Color color;
    }

    [System.Serializable]
    public class Season
    {
        public string name;
        public int id;
        public Color color;
    }

    public List<Planet> listOfPlanets = new List<Planet>();
    public List<Sign> listOfSigns = new List<Sign>();
    public List<Element> listOfElements = new List<Element>();
    public List<Season> listOfSeasons = new List<Season>();
}
