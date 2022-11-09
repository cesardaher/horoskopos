using SwissEphNet;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FixedStarsGenerator : MonoBehaviour
{
    public GeoData geodata;
    double tjd_ut;
    [SerializeField] ConstellationDatabase constellationDatabase;
    Int32 iflag = SwissEph.SEFLG_SWIEPH, iflgret;
    double[] x2 = new double[6];
    double[] xaz = new double[6];
    string serr = "";
    [SerializeField] GameObject fixedStarPrefab;
    [SerializeField] List<FixedStarObject> listOfStarObjects = new List<FixedStarObject>();

    [Serializable]
    public class FixedStarObject
    {
        public string nameOfConstellation;
        public string name;
        public double magnitude;
        public double linearMagnitude;
        public GameObject starObject;
        public FixedStar starComponent;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        tjd_ut = GeoData.ActiveData.Tjd_ut;
        //CalculateStarByNumber();
        //CalculateFixedStars();
    }

    private void Update()
    {
        if (tjd_ut != GeoData.ActiveData.Tjd_ut)
        {
            tjd_ut = GeoData.ActiveData.Tjd_ut;
            //CalculateFixedStars();
        }

        //CalculateFixedStars();
    }

    private void OnValidate()
    {
        CreateStarObjects();
        CalculateFixedStars();
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

        for (int i = 0; i < 1600; i++)
        {
            FixedStarObject fixedStarObject = new FixedStarObject
            {
                starObject = SpawnStar("")
            };

            fixedStarObject.starComponent = fixedStarObject.starObject.GetComponent<FixedStar>();
            fixedStarObject.starComponent.starName = "";

            listOfStarObjects.Add(fixedStarObject);
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
        if (geodata is null) return;
        for (int i = 1; i < 1600; i++)
        {
            string starForCalc = i.ToString();
            iflgret = SwissEphemerisManager.swe.swe_fixstar2_ut(ref starForCalc, geodata.Tjd_ut, iflag, x2, ref serr);

            if (iflgret < 0)
                Debug.Log("error: " + serr);

            serr = "";

            listOfStarObjects[i].starComponent.positionData = x2;

            double mag = 0;
            iflgret = SwissEphemerisManager.swe.swe_fixstar2_mag(ref starForCalc, ref mag, ref serr);

            if (iflgret < 0)
                Debug.Log("error: " + serr);

            double[] x2ToXaz = new double[6];

            x2ToXaz[0] = x2[0];
            x2ToXaz[1] = x2ToXaz[2] = x2[1];

            // removed in favor of using ecliptic coordinates
            //SwissEphemerisManager.swe.swe_azalt(geodata.Tjd_ut, SwissEph.SE_ECL2HOR, geodata.Geopos, 0, 0, x2, xaz);

            listOfStarObjects[i].starComponent.AzAlt = x2ToXaz;
            listOfStarObjects[i].magnitude = mag;
            listOfStarObjects[i].linearMagnitude = Math.Pow(10, -0.4*(mag - 4.74));
            if (mag == 0) listOfStarObjects[i].linearMagnitude = 20;

            Vector3 magScale = new Vector3((float)listOfStarObjects[i].linearMagnitude * 5, (float)listOfStarObjects[i].linearMagnitude * 5, (float)listOfStarObjects[i].linearMagnitude * 5);
            //Vector3 magScale = new Vector3((float)mag, (float)mag, (float)mag);
            listOfStarObjects[i].starComponent.transform.GetChild(0).localScale = magScale;
        }
    }
}
