using System.Collections.Generic;
using UnityEngine;

public class ZodiacData : MonoBehaviour
{
    public static ZodiacData instance;
    public List<SignData> signDataList = new List<SignData>();
    static int planetDataSize = 13;

    private void Awake()
    {
        if (instance is null)
            instance = this;
        else Debug.LogWarning("More than one Zodiac Data. Delete this.");

        InitializeSigns();
        EventManager.Instance.OnCalculatedPlanet += CheckIfPlanetIsInSign;
    }

    private void Start()
    {
        planetDataSize = PlanetData.PlanetDataList.Count;
    }
    private void OnDestroy()
    {
        instance = null;
        EventManager.Instance.OnCalculatedPlanet -= CheckIfPlanetIsInSign;
    }

    void InitializeSigns()
    {
        for (int i = 0; i < 13; i++)
        {
            var cusp = new SignData(i);
            signDataList.Add(cusp);
        }
    }

    void CheckIfPlanetIsInSign(int id)
    {
        // iterate through all signs
        for(int y = 0; y < signDataList.Count; y++)
        {
            // check if that particular planet (id) is in the given sign
            if(signDataList[y].signId == PlanetData.PlanetDataList[id].SignID)
            {
                // assign that planet as true
                signDataList[y].planets[id] = 1;
                continue;
            }
             
            // keep planet as false
            signDataList[y].planets[id] = 0;
        }
    }

    [System.Serializable]
    public class SignData
    {
        public int signId;
        public double longitude;
        public double midLongitude;
        public double endLongitude;
        public int[] planets;
        // public List<int> houseCusps = new List<int>();

        public SignData(int id)
        {
            signId = id;
            SetLongitude(id, out longitude);
            SetMidLongitude(out midLongitude);
            SetEndLongitude(out endLongitude);

            planets = new int[planetDataSize];
        }

        void SetLongitude(int id, out double longitude)
        {
            double value = (id * 30) - 30;
            longitude = value;
        }

        void SetMidLongitude(out double midLongitude)
        {
            double value = longitude + 15;
            midLongitude = value;
        }

        void SetEndLongitude(out double endLongitude)
        {
            double value = longitude + 30;
            endLongitude = value;
        }

        public void FindPlanetsInSigns()
        {
            foreach (PlanetData planet in PlanetData.PlanetDataList)
            {
                if(planet.SignID == this.signId)
                {
                    Debug.Log("found planet");
                }
            }
        }
    }

}
