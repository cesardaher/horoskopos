using SwissEphNet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstroResources;

public class CalculateAllPlanets : MonoBehaviour
{
    private int _iflgret;
    readonly int _iflag = SwissEph.SEFLG_SPEED;
    string _serr = "";
    double[] _x2 = new double[6];

    [SerializeField] public Transform _target;
    [SerializeField] public Transform _ariesReference;
    [SerializeField] public Transform[] _planets;
    [SerializeField] public Transform[] _signs;
    [SerializeField] public float _radius;
    [SerializeField] public GeoData _geoData;

    public bool timeRunning;

    private void Start()
    {
        _geoData = CreateCurrentTimeInBerlin();
        GeoData.ActiveData = _geoData;

        TimeManager.Instance.StartAnimation();

        //CalculatePlanets();
        //CalculateSigns();
        //CalculateForwardAndUp();
    }

    private void Update()
    {
        if(timeRunning)
        {
            CalculatePlanets();
            CalculateSigns();
            CalculateForwardAndUp();

            //timeRunning = false;
        }
        
    }

    void CalculateForwardAndUp()
    {
        // calculate Aries
        var xaz = new double[6];
        var xx = new double[6];

        SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, xx, xaz);
        _ariesReference.transform.position = PositionsFromSphericalCoordinates(xaz);


        xx[1] = 90;
        SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, xx, xaz);
        _target.position = PositionsFromSphericalCoordinates(xaz);

        transform.rotation = Quaternion.LookRotation(_ariesReference.position, _target.position.normalized);
    }

    void CalculatePlanets()
    {
        // calculate planetary positions
        for (var p = SwissEph.SE_SUN; p <= SwissEph.SE_MEAN_NODE; p++)
        {
            var planetsX2 = new double[6];

            if (_planets.Length < p) return;
            if (!_planets[p]) continue;

            //calculate ecliptic coordinates
            _iflgret = SwissEphemerisManager.swe.swe_calc_ut(GeoData.ActiveData.Tjd_ut, p, _iflag, planetsX2, ref _serr); 

            if (_iflgret < 0)
                Debug.Log("error: " + _serr);

            if (_iflgret < 0)
                Debug.Log("error: " + _serr);

            _planets[p].transform.localPosition = PositionsFromSphericalCoordinates(planetsX2);
        }
    }

    void CalculateSigns()
    {
        for(var i = 0; i < _signs.Length; i++)
        {
            double[] signsX2 = new double[6];

            signsX2[0] = 30 * i;
            _signs[i].transform.localPosition = PositionsFromSphericalCoordinates(signsX2);

        }

    }

    private Vector3 PositionsFromSphericalCoordinates(double[] coordinates)
    {
        var az = (coordinates[0] + 90) * Mathf.Deg2Rad;
        var angle = coordinates[1] * Mathf.Deg2Rad;
        var pos = new Vector3
        {
            x = _radius * Mathf.Cos((float)az) * Mathf.Cos((float)angle),
            y = _radius * Mathf.Sin((float)angle),
            z = _radius * Mathf.Sin((float)az) * Mathf.Cos((float)angle)
        };

        return pos;
    }

    // For initializing time
    // Starts geodata based in Berlin's current time
    GeoData CreateCurrentTimeInBerlin()
    {
        int BerlinCityId = 12638;
        char hSys = 'W'; // whole sign houses
        string timezone = "W. Europe Standard Time";

        // get local time and date
        DateTime moment = DateTime.Now;

        // get reference timezone (Berlin)
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone);

        // convert local time and date to berlin time
        // check for daylight savings
        DateTime dtInBerlin = TimeZoneInfo.ConvertTime(moment, timeZoneInfo);
        bool isDST = timeZoneInfo.IsDaylightSavingTime(dtInBerlin);

        GeoData gd = ScriptableObject.CreateInstance<GeoData>();
        gd.InitializeDataWithCityIdDateTime("", moment, BerlinCityId, hSys, isDST);

        return gd;
    }
}
