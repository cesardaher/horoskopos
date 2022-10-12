using SwissEphNet;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FixedStarsCalculator : MonoBehaviour
{
    double tjd_ut;
    [SerializeField] ConstellationDatabase constellationDatabase;
    Int32 iflag = SwissEph.SEFLG_SWIEPH, iflgret;
    double[] x2 = new double[6];
    double[] xaz = new double[6];
    string serr = "";
    [SerializeField] GameObject fixedStarPrefab;
    [SerializeField] List<FixedStarObject> listOfStarObjects = new List<FixedStarObject>();

    [Serializable]
    public class FixedStarObject {
        public string nameOfConstellation;
        public string name;
        public GameObject starObject;
        public FixedStar starComponent;
    }

    // Start is called before the first frame update
    void Start()
    {
        tjd_ut = GeoData.ActiveData.Tjd_ut;
        CalculateStarByNumber();
        CalculateFixedStars();
    }
    
    private void Update()
    {
        if(tjd_ut != GeoData.ActiveData.Tjd_ut)
        {
            tjd_ut = GeoData.ActiveData.Tjd_ut;
            CalculateFixedStars();
        }
    }

    private void OnValidate()
    {

        CreateStarObjects();
    }

    void CalculateStarByNumber()
    {
        string starForCalc = "1";
        iflgret = SwissEphemerisManager.swe.swe_fixstar2_ut(ref starForCalc, GeoData.ActiveData.Tjd_ut, iflag, x2, ref serr);

        if (iflgret < 0)
            Debug.Log("error: " + serr);

        Debug.Log(starForCalc);
    }

    void CreateStarObjects()
    {
        if (transform.childCount != 0) return;

        listOfStarObjects.Clear();
        foreach (ConstellationDatabase.Constellation constellation in constellationDatabase.listOfConstellations.constellations)
        {
            foreach (string starName in constellation.stars)
            {
                FixedStarObject fixedStarObject = new FixedStarObject
                {
                    name = starName,
                    nameOfConstellation = constellation.name,
                    starObject = SpawnStar(starName)
                };

                fixedStarObject.starComponent = fixedStarObject.starObject.GetComponent<FixedStar>();
                fixedStarObject.starComponent.starName = starName;

                listOfStarObjects.Add(fixedStarObject);
            }
        }
    }
    GameObject SpawnStar(string name)
    {
        FixedStar newStar = Instantiate(fixedStarPrefab, transform).GetComponent<FixedStar>();

        newStar.gameObject.name = name;

        return newStar.gameObject;
    }

    public void CalculateFixedStars()
    {

        foreach(FixedStarObject star in listOfStarObjects)
        {
            CalculateStars(star.name);

            star.starComponent.positionData = x2;
            star.starComponent.AzAlt = CalculateAzAlt();
        }

        double[] CalculateAzAlt()
        {
            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, x2, xaz);
            return xaz;
        }

        void CalculateStars(string starName)
        {
                string starForCalc = starName;
                iflgret = SwissEphemerisManager.swe.swe_fixstar2_ut(ref starForCalc, GeoData.ActiveData.Tjd_ut, iflag, x2, ref serr);

                if (iflgret < 0)
                    Debug.Log("error: " + serr);

        }
    }

}
