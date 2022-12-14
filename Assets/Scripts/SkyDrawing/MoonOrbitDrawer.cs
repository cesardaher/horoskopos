using SwissEphNet;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MoonOrbitDrawer : EllipseRenderer, IAzalt
{
    // final date
    [SerializeField] int fday;
    [SerializeField] int fmon;
    [SerializeField] int fyear;
    [SerializeField] int fhour;
    [SerializeField] int fmin;
    [SerializeField] double fsec;
    string serr;

    [SerializeField] GameObject moonOnOrbit;
    [SerializeField] OrbitMarkerToggler orbitMarkerToggler;

    List<Vector3> cuspPoints = new List<Vector3>();

    void Start()
    {
        EventManager.Instance.OnRecalculationOfGeoData += CalculateMoonPositions;
        EventManager.Instance.OnPlanetSelect += ShowMoonOrbit;
        EventManager.Instance.OnPlanetBoxClose += HideMoonOrbit;
        //EventManager.instance.OnAnimationStart += HideMoonOrbit;

        orbitMarkerToggler.OutsideStart();

        gameObject.SetActive(false);
    }


    void CalculateMoonPositions()
    {
        // only work when active
        if (!gameObject.activeSelf) return;

        cuspPoints.Clear();

        for(int i = 0; i < 29; i++)
        {
            double tempTjd_ut;

            if (i == 0) tempTjd_ut = GeoData.ActiveData.Tjd_ut;
            else if (i == 29) tempTjd_ut = CalculateTime(0, 7, 43, 5);
            else tempTjd_ut = CalculateTime(i, 0, 0, 0); //add i to days

            int iflgret;
            int iflag = SwissEph.SEFLG_SPEED;
            double[] x2 = new double[6]; 

            //calculate ecliptic coordinates
            iflgret = SwissEphemerisManager.swe.swe_calc_ut(tempTjd_ut, SwissEph.SE_MOON, iflag, x2, ref serr);
            if (iflgret < 0)
                Debug.LogWarning("error: " + serr);

            double[] xaz = new double[6];

            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, x2, xaz);

            RotateAzimuth(xaz[0]);
            RotateAltitude(xaz[1]);

            cuspPoints.Add(pointer.position);
        }

        DrawEllipse(cuspPoints);

        // reset position completely
        transform.eulerAngles = Vector3.zero;
        transform.position = Vector3.zero;

        // set position of Moon marker to match real position
        moonOnOrbit.transform.eulerAngles = PlanetData.PlanetDataList[1].realPlanet.planet.transform.eulerAngles;

    }

    void ShowMoonOrbit(int id, Vector3 pos)
    {
        if(id == 1|| id == 10 || id == 11)
        {
            gameObject.SetActive(true);
            CalculateMoonPositions();

        }
    }

    void HideMoonOrbit(int id)
    {
        if (id == 1 || id == 10 || id == 11)
        {
            gameObject.SetActive(false);
        }
    }

    void HideMoonOrbit()
    {
        gameObject.SetActive(false);

    }

    double CalculateTime(int day, int hour, int min, double sec)
    {
        DateTime tempDateTime = TimeManager.Instance.ActiveDateTime;
        tempDateTime = tempDateTime.AddDays(day);
        tempDateTime = tempDateTime.AddHours(hour);
        tempDateTime = tempDateTime.AddMinutes(min);
        tempDateTime = tempDateTime.AddSeconds(sec);

        Int32 retval;

        Int32 gregflag = SwissEph.SE_GREG_CAL;

        /* time zone = Indian Standard Time; NOTE: east is positive */

        double[] dret = new double[2];

        SwissEphemerisManager.swe.swe_utc_time_zone(
            tempDateTime.Year,
            tempDateTime.Month,
            tempDateTime.Day,
            tempDateTime.Hour,
            tempDateTime.Minute,
            tempDateTime.Second, 
            GeoData.ActiveData.D_timezone, 
            ref fyear, ref fmon, ref fday, ref fhour, ref fmin, ref fsec);

        retval = SwissEphemerisManager.swe.swe_utc_to_jd(fyear, fmon, fday, fhour, fmin, fsec, gregflag, dret, ref serr);
        if (retval == SwissEph.ERR)
        {
            Debug.LogWarning("Error: " + serr);
        }

        // if there is a problem, a negative value is returned and an 
        // errpr message is in serr.

        return dret[1];
    }

    public void RotateAzimuth(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;

        rotationVector.y = (float)rotation + 180;
        if (GeoData.ActiveData._northernHemisphere)
        {
            //rotationVector.y -= 180;
            //Debug.Log("northern");
        }

        transform.localRotation = Quaternion.Euler(rotationVector);
    }

    //rotates on the Z axis
    public void RotateAltitude(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;
        rotationVector.z = (float)rotation;
        transform.localRotation = Quaternion.Euler(rotationVector);
    }

    void OnDestroy()
    {
        EventManager.Instance.OnRecalculationOfGeoData -= CalculateMoonPositions;
        EventManager.Instance.OnPlanetSelect -= ShowMoonOrbit;
        EventManager.Instance.OnPlanetBoxClose -= HideMoonOrbit;
    }
}
