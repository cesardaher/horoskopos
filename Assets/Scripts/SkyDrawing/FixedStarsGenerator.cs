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

        //StartCoroutine("Coroutine");

        CompareValues();

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

        for (int i = 1; i < 500; i++)
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
            fixedStarObject.starComponent.transform.localScale = magScale;

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

    double[] fixstar_prepare_data(double[] stardata)
    {
        double[] finalStarData = new double[8];
        double KM_S_TO_AU_CTY = 21.095;
        string sde_d;
        double epoch, radv, parall, mag;
        double ra_s, ra_pm, de_pm, ra, de;
        double ra_h, ra_m, de_d, de_m, de_s;

        // star data
        epoch = stardata[0];
        ra_h = stardata[1];
        ra_m = stardata[2];
        ra_s = stardata[3];
        de_d = stardata[4];
        sde_d = stardata[4].ToString();
        de_m = stardata[5];
        de_s = stardata[6];
        ra_pm = stardata[7];
        de_pm = stardata[8];
        radv = stardata[9];
        parall = stardata[10];
        mag = stardata[11];
        /****************************************
         * position and speed (equinox)
         ****************************************/
        /* ra and de in degrees */
        ra = (ra_s / 3600.0 + ra_m / 60.0 + ra_h) * 15.0;
        if (sde_d.IndexOf('-') < 0)
            de = de_s / 3600.0 + de_m / 60.0 + de_d;
        else
            de = -de_s / 3600.0 - de_m / 60.0 + de_d;
        /* speed in ra and de, degrees per century */
        ra_pm = ra_pm / 10.0 / 3600.0;
        de_pm = de_pm / 10.0 / 3600.0;
        parall /= 1000.0;
        /* parallax, degrees */
        if (parall > 1)
            parall = (1 / parall / 3600.0);
        else
            parall /= 3600;
        /* radial velocity in AU per century */
        radv *= KM_S_TO_AU_CTY;
        /*printf("ra=%.17f,de=%.17f,ma=%.17f,md=%.17f,pa=%.17f,rv=%.17f\n",ra,de,ra_pm,de_pm,parall,radv);*/
        /* radians */
        ra *= SwissEph.DEGTORAD;
        de *= SwissEph.DEGTORAD;
        ra_pm *= SwissEph.DEGTORAD;
        de_pm *= SwissEph.DEGTORAD;
        ra_pm /= Math.Cos(de); /* catalogues give proper motion in RA as great circle */
        parall *= SwissEph.DEGTORAD;
        // stardata
        // [0] - epoch
        // [1] - ra
        // [2] - de
        // [3] - ramot
        // [4] - demot
        // [5] - radvel
        // [6] - parall
        // [7] - mag
        finalStarData[0] = epoch;
        finalStarData[1] = ra;
        finalStarData[2] = de;
        finalStarData[3] = ra_pm;
        finalStarData[4] = de_pm;
        finalStarData[5] = radv;
        finalStarData[6] = parall;
        finalStarData[7] = mag;
        return finalStarData;
    }

    void CompareValues()
    {
        // altair
        // ICRS,
        // 19,
        // 50,
        // 46.99855,
        // +08,
        // 52,
        // 05.9563,
        // 536.23,
        // 385.29,
        // -26.6,
        // 194.95,
        // 0.76,

        //epoch = stardata[0];
        //ra_h = stardata[1];
        //ra_m = stardata[2];
        //ra_s = stardata[3];
        //de_d = stardata[4];
        //sde_d = stardata[4].ToString();
        //de_m = stardata[5];
        //de_s = stardata[6];
        //ra_pm = stardata[7];
        //de_pm = stardata[8];
        //radv = stardata[9];
        //parall = stardata[10];
        //mag = stardata[11];

        // Apex,1950,18,03,50.2, 30,00,16.8,  0.000,   0.00,-16.5,0.0000,999.99

        double epoch = 1950;
        double ra_h = 18;
        double ra_m = 03;
        double ra_s = 50.2;
        double de_d = 30;
        double de_m = 00;
        double de_s = 16.8;
        double ra_pm = 0.000;
        double de_pm = 0.00;
        double radv = -16.5;
        double parall = 0.0000;
        double mag = 999.99;

        double[] stardata = { epoch,ra_h, ra_m, ra_s, de_d, de_m, de_s, ra_pm, de_pm, radv, parall, mag};
        stardata = fixstar_prepare_data(stardata);

        double[] x2 = new double[6];
        double[] x22 = new double[6];
        string star = "Apex";

        int iflgret = SwissEphemerisManager.swe.swe_fixstar2_ut(ref star, geodata.Tjd_ut, iflag, x2, ref serr);
        if (iflgret < 0)
            Debug.Log("1 error: " + serr);

        iflgret = SwissEphemerisManager.swe.swe_fixstar2_ut_array(ref stardata, geodata.Tjd_ut, iflag, x22, ref serr);
        if (iflgret < 0)
            Debug.Log("2 error: " + serr);

        for(int i =0; i < 6; i++)
        {
            Debug.Log(x2[i]);
            Debug.Log(x22[i]);
        }
       
    }

    public void CalculateFixedStars()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        if (geodata is null) return;

        for (int i = 0; i < listOfStarObjects.Count; i++)
        {
            
            string starForCalc = listOfStarObjects[i].starIndex.ToString();

            iflgret = SwissEphemerisManager.swe.swe_fixstar2_ut(ref starForCalc, geodata.Tjd_ut, iflag, x2, ref serr);

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

            SwissEphemerisManager.swe.swe_azalt(geodata.Tjd_ut, SwissEph.SE_ECL2HOR, geodata.Geopos, 0, 0, x2, xaz);
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
