using System;
using System.Collections.Generic;
using UnityEngine;
using AstroResources;

public class HouseData : MonoBehaviour
{

    public static HouseData instance;

    [SerializeField] public List<HouseCusp> houseDataList = new List<HouseCusp>();
    public List<ChartElement> houseCusp = new List<ChartElement>();
    public List<ChartElement> midHouseNumber = new List<ChartElement>();

    private void Awake()
    {
        if (instance is null)
            instance = this;
        else Debug.LogWarning("More than one HouseData. Delete this.");

        EventManager.Instance.OnRecalculationOfGeoData += AssignHouseCusps;
        EventManager.Instance.OnRecalculationOfGeoData += Draw2DCusps;
        EventManager.Instance.OnCalculatedPlanet += FindPlanetInEachHouse;
        EventManager.Instance.OnHouseReassignment += RecalculateHouses;

        for (int i = 0; i < 13; i++)
        {
            var cusp = new HouseCusp();
            houseDataList.Add(cusp);
            houseDataList[i].houseId = i;
        }
    }


    private void Start()
    {
    }

    void AssignHouseCusps()
    {
        for (int i = 0; i < 13; i++)
        {
            houseDataList[i].Longitude = GeoData.ActiveData.HouseCusps[i];
        }

        for (int i = 1; i < 13; i++)
        {
            double arg1 = houseDataList[i].Longitude;
            double arg2;

            if (i == 12) arg2 = houseDataList[1].Longitude; // overflow indexes on the last one
            else arg2 = houseDataList[i + 1].Longitude;


            if (arg1 > arg2) // if longitudes are overflowing the 360 marker
            {
                arg2 = arg2 + 360; // put the second argument in the same scale as the first
                var res = ((arg1 + arg2) / 2) - 360;

                if (res <= 360) res += 360; // get positive longitude value if negative

                houseDataList[i].midLongitude = res;

                continue;
            }

            houseDataList[i].midLongitude = (arg1 + arg2) / 2;
        }
    }

    void AssignHouseCuspsFromNewHouseSystem(char hSys)
    {
        for (int i = 0; i < 13; i++)
        {
            houseDataList[i].Longitude = GeoData.ActiveData.HouseCusps[i];
        }

        for (int i = 1; i < 13; i++)
        {
            double arg1 = houseDataList[i].Longitude;
            double arg2;

            if (i == 12) arg2 = houseDataList[1].Longitude; // overflow indexes on the last one
            else arg2 = houseDataList[i + 1].Longitude;


            if (arg1 > arg2) // if longitudes are overflowing the 360 marker
            {
                arg2 = arg2 + 360; // put the second argument in the same scale as the first
                var res = ((arg1 + arg2) / 2) - 360;

                if (res <= 360) res += 360; // get positive longitude value if negative

                houseDataList[i].midLongitude = res;

                continue;
            }

            houseDataList[i].midLongitude = (arg1 + arg2) / 2;
        }
    }

    void RecalculateHouses(char Hsys)
    {
        GeoData.GetHouseSystemFromEvent(Hsys);
        GeoData.RecalculateHouses();
        AssignHouseCusps();
        Draw2DCusps();
    }


    void FindPlanetInEachHouse(int id)
    {
        if (id > 11) return;
        foreach(HouseCusp cusp in houseDataList)
        {
            cusp.FindPlanetInHouse(id);
        }
    }

    public void Draw2DCusps()
    {
        for (int i = 0; i < houseCusp.Count; i++)
        {
            int n = i + 2; //start from house cusp 2

            houseCusp[i].Rotation = GeoData.ActiveData.HouseCusps[n];
        }

        for(int i = 0; i < midHouseNumber.Count; i++)
        {
            // increment 1, because there is no house 0
            midHouseNumber[i].Rotation = houseDataList[i + 1].midLongitude;
            
        }
    }


    [System.Serializable]
    public class HouseCusp
    {
        public int houseId;
        [SerializeField] double longitude;

        public double Longitude
        {
            set
            {
                longitude = value;
                AssignSignData(longitude);
                DecimalToMinSec(longitude, out longMinSec);
            }
            get
            {
                return longitude;
            }
        }

        public List<int> planets = new List<int>();
        public double midLongitude;
        public int signID;
        public string sign;
        public float degrees;

        public int[] longMinSec = new int[3];

        void AssignSignData(double value)
        {
            // assign sign, ID and degrees
            signID = (int)System.Math.Abs(value / 30) + 1;
            sign = ((SIGN)signID).ToString();
            degrees = (float)System.Math.Abs(value % 30);
        }

        void DecimalToMinSec(double val, out int[] degMinSec)
        {
            degMinSec = new int[3];

            degMinSec[0] = (int)Math.Truncate(val % 30);

            double tempMin = val - Math.Truncate(val);

            degMinSec[1] = (int)Math.Truncate(tempMin * 60);

            double tempSec = tempMin * 60 - Math.Truncate(tempMin * 60);

            degMinSec[2] = (int)Math.Truncate(tempSec * 60);

        }

        public void FindPlanetInHouse(int id)
        {
            planets.Clear();

            if (PlanetData.PlanetDataList[id].House == houseId)
            {
                planets.Add(id);
            }
        }
    }

    private void OnDestroy()
    {
        instance = null;
        EventManager.Instance.OnRecalculationOfGeoData -= AssignHouseCusps;
        EventManager.Instance.OnRecalculationOfGeoData -= Draw2DCusps;
        EventManager.Instance.OnCalculatedPlanet -= FindPlanetInEachHouse;
        EventManager.Instance.OnHouseReassignment -= RecalculateHouses;
    }
}
