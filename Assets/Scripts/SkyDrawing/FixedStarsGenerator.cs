using SwissEphNet;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        
        //tjd_ut = GeoData.ActiveData.Tjd_ut;
        //CalculateStarByNumber();
        //CalculateFixedStars();
    }

    private void Update()
    {
        //if (tjd_ut != GeoData.ActiveData.Tjd_ut)
        //{
            //tjd_ut = GeoData.ActiveData.Tjd_ut;
            //CalculateFixedStars();
        //}

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

        for (int i = 0; i < 1140; i++)
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
        var watch = System.Diagnostics.Stopwatch.StartNew();
        if (geodata is null) return;
        for (int i = 1; i < 1140; i++)
        {
            string starForCalc = i.ToString();
            iflgret = SwissEphemerisManager.swe.swe_fixstar2_ut(ref starForCalc, geodata.Tjd_ut, iflag, x2, ref serr);

            if (iflgret < 0)
                Debug.Log("error: " + serr);

            serr = "";

            listOfStarObjects[i].starComponent.positionData = x2;

            double mag = 0;
            //iflgret = SwissEphemerisManager.swe.swe_fixstar2_mag(ref starForCalc, ref mag, ref serr);

            if (iflgret < 0)
                Debug.Log("error: " + serr);

            double[] x2ToXaz = new double[6];

            x2ToXaz[0] = x2[0];
            x2ToXaz[1] = x2ToXaz[2] = x2[1];

            // removed in favor of using ecliptic coordinates
            //SwissEphemerisManager.swe.swe_azalt(geodata.Tjd_ut, SwissEph.SE_ECL2HOR, geodata.Geopos, 0, 0, x2, xaz);

            listOfStarObjects[i].starComponent.AzAlt = x2ToXaz;
            //listOfStarObjects[i].magnitude = mag;

            //listOfStarObjects[i].linearMagnitude = StarSize(mag);

            //Vector3 magScale = new Vector3((float)listOfStarObjects[i].linearMagnitude, (float)listOfStarObjects[i].linearMagnitude, (float)listOfStarObjects[i].linearMagnitude);
            //Vector3 magScale = new Vector3((float)mag, (float)mag, (float)mag);
            //listOfStarObjects[i].starComponent.transform.GetChild(0).localScale = magScale;
        }

        watch.Stop();
        Debug.Log(watch.ElapsedMilliseconds);
    }

    double StarSize(double magnitude)
    {
        double size = magnitude;
        double mag_max = 0;
        double mag_step = 0.5f;
        double mag_alpha = 1.172f;
        double mag_size_norm = 5;


        // normalize
        size = (mag_max - size) / mag_step;
        size = mag_size_norm * 46.6 * Math.Pow(mag_alpha, size);
        size = 0.75 * 3 * size * 0.0014552083 * 60;

        return size;
    }
}
