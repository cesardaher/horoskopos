using SwissEphNet;
using System;
using System.Collections;
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
        public int starIndex;
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
        CreateStarObjects();
        CalculateFixedStars();

        StartCoroutine("Coroutine");

        //CreateStarObjects();

        //tjd_ut = GeoData.ActiveData.Tjd_ut;
        //CalculateStarByNumber();
        //CalculateFixedStars();
    }

    private void FixedUpdate()
    {
        //CalculateFixedStars();
        //CalculateFixedStars();
        //if (tjd_ut != GeoData.ActiveData.Tjd_ut)
        //{
        //tjd_ut = GeoData.ActiveData.Tjd_ut;
        //CalculateFixedStars();
        //}

        //CalculateFixedStars();
    }

    IEnumerator Coroutine()
    {
        while(true)
        {
            CalculateFixedStars();
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnValidate()
    {

    }

    void CreateStarObjects()
    {
        if (transform.childCount != 0) return;

        listOfStarObjects.Clear();

        for (int i = 1; i < 1139; i++)
        {
            string starForCalc = i.ToString();
            double mag = 0;
            iflgret = SwissEphemerisManager.swe.swe_fixstar2_mag(ref starForCalc, ref mag, ref serr);

            if (mag > 3) continue;

            FixedStarObject fixedStarObject = new FixedStarObject
            {
                starObject = SpawnStar("")
            };

            fixedStarObject.starComponent = fixedStarObject.starObject.GetComponent<FixedStar>();
            fixedStarObject.starComponent.starName = "";

            fixedStarObject.magnitude = mag;
            float linearMag = StarSize(mag);

            fixedStarObject.linearMagnitude = linearMag;

            Vector3 magScale = new Vector3(linearMag, linearMag, linearMag);
            fixedStarObject.starComponent.transform.GetChild(0).localScale = magScale;

            fixedStarObject.starIndex = i;

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
        SwissEph swe = SwissEphemerisManager.swe;
        var watch = System.Diagnostics.Stopwatch.StartNew();
        if (geodata is null) return;

        for (int i = 0; i < listOfStarObjects.Count; i++)
        {
            
            string starForCalc = listOfStarObjects[i].starIndex.ToString();

            iflgret = swe.swe_fixstar2_ut(ref starForCalc, geodata.Tjd_ut, iflag, x2, ref serr);

            if (iflgret < 0)
                Debug.Log("error: " + serr);

            serr = "";

            listOfStarObjects[i].starComponent.positionData = x2;

            if (iflgret < 0)
                Debug.Log("error: " + serr);

            double[] x2ToXaz = new double[6];

            x2ToXaz[0] = x2[0];
            x2ToXaz[1] = x2ToXaz[2] = x2[1];

            listOfStarObjects[i].starComponent.AzAlt = x2ToXaz;
        }

        watch.Stop();
        //Debug.Log("count: " + listOfStarObjects.Count);
        Debug.Log("elapsed: " + watch.ElapsedMilliseconds);
    }

    float StarSize(double magnitude)
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

        return (float)size;
    }
}
