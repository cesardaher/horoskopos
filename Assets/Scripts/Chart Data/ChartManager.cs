using System.Linq;
using System.Collections.Generic;
using SwissEphNet;
using System;
using UnityEngine;
using AstroResources;

public class ChartManager : MonoBehaviour
{
    //flag to track whether game was started
    bool _wasInitiated = false;

    public GeoData _geodata;
    [SerializeField] Zodiac _zodiac;
    [SerializeField] ChartElement _meridian;
    [SerializeField] ControlRecorder _controlRecorder;
    [SerializeField] AngleData _ascendant;
    [SerializeField] AngleData _midheaven;

    [SerializeField] bool _northernHemisphere;
    readonly int _iflag = SwissEph.SEFLG_SPEED;
    int _iflgret;

    string _serr = "";

    double[] _x2 = new double[6];
    double[] _tempX2 = new double[6];
    double[] _xaz = new double[6];
    double[] _secAz = new double[6];
    double[] _attr = new double[20];

    [SerializeField] double[] _houseCusps = new double[13];
    [SerializeField] double[] _ascmc = new double[10];
    [SerializeField] double _armc;
    [SerializeField] double _asc;
    [SerializeField] double _mc;

    [Header("Active Data")]
    [SerializeField] double _Tjd_ut;
    [SerializeField] [Range(1, 31)] int _day;
    [SerializeField] [Range(1, 12)] int _month;
    [SerializeField] [Range(0, 2100)] int _year;
    [SerializeField] [Range(0, 23)] int _hour;
    [SerializeField] [Range(0, 59)] int _min;
    [SerializeField] [Range(0, 59)] double _sec;
    [SerializeField] [Range(-12, 12)] double _timezone;
    [SerializeField] [Range(-90, 90)] double _lat;
    [SerializeField] [Range(-180, 180)] double _lon;
    [SerializeField] char _hsys;
    [SerializeField] double _height;

    private void Awake()
    {
        // add event to Event Manager
        EventManager.Instance.OnRecalculationOfGeoData += ReCalculateChart;
        EventManager.Instance.OnChartMode += PlacePlanetSymbols;
        EventManager.Instance.OnMultiplePlanetsToggle += PlanetData.RessignAllPlanets;
    }

    void OnDestroy()
    {
        EventManager.Instance.OnRecalculationOfGeoData -= ReCalculateChart;
        EventManager.Instance.OnChartMode -= PlacePlanetSymbols;
        EventManager.Instance.OnMultiplePlanetsToggle -= PlacePlanetSymbols;
        EventManager.Instance.OnMultiplePlanetsToggle -= PlanetData.RessignAllPlanets;
    }

    void Start()
    {
        // this is put on start so that it doesn't come before ConjunctionManager's action
        EventManager.Instance.OnMultiplePlanetsToggle += PlacePlanetSymbols;

        // if scene start on exploration mode, get current time
        if (GeoData.ActiveData is null)
            _geodata = CreateCurrentTimeInBerlin();
        // else get data from tutorial
        else
            _geodata = GeoData.ActiveData;

        // start chart calculations
        GeoData.ActiveData = _geodata;

        // flags if game was played
        // mainly on OnValidate
        _wasInitiated = true;
    }

    private void OnValidate()
    {
        // change data when game is playing
        // for changing the chart from inspector
        ReassignGeoData();
    }

    void ReCalculateChart()
    {
        CollectGeoData();
        CalculateChart();
    }

    void CollectGeoData()
    {
        _Tjd_ut = _geodata.Tjd_ut;
        _day = _geodata.Iday;
        _month = _geodata.Imon;
        _year = _geodata.Iyear;
        _hour = _geodata.Ihour;
        _min = _geodata.Imin;
        _sec = _geodata.Dsec;
        _timezone = _geodata.F_timezone;
        _lat = _geodata.Geolat;
        _lon = _geodata.Geolon;
        _northernHemisphere = _geodata.NorthernHemisphere;
        _height = _geodata.Height;
        _hsys = _geodata.Hsys;
    }

    void ReassignGeoData()
    {
        if (_wasInitiated)
            GeoData.ActiveData.InitializeData(_geodata.DataName, _day, _month, _year, _hour, _min, _sec, _geodata.D_timezone, _lat, _lon, _height, _hsys, _geodata.DaylightSavings);
    }

    GeoData CreateCurrentTimeInBerlin()
    {
        int BerlinCityId = 12638;
        char hSys = 'W'; // whole sign houses

        // get local time and date
        DateTime moment = DateTime.Now;

        // get reference timezone (Berlin)
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

        // convert local time and date to berlin time
        // check for daylight savings
        DateTime dtInBerlin = TimeZoneInfo.ConvertTime(moment, timeZoneInfo);
        bool isDST = timeZoneInfo.IsDaylightSavingTime(dtInBerlin);

        GeoData gd = ScriptableObject.CreateInstance<GeoData>();
        gd.InitializeDataWithCityIdDateTime("", moment, BerlinCityId, hSys, isDST);

        return gd;
    }

    void CalculateChart()
    {
        ConjunctionManager.Instance.ClearConjunctions();

        // assign 2D chart elements
        DrawChart2D();
        CalculatePlanets();
        AssignAsc();
        AssignMC();
        ConjunctionManager.Instance.FindCloseConjunctions();
        PlacePlanetSymbols();
        return;

        void CalculatePlanets()
        {
            // calculate planetary positions
            for (var p = SwissEph.SE_SUN; p <= SwissEph.SE_MEAN_NODE; p++)
            {
                //calculate ecliptic coordinates
                _iflgret = SwissEphemerisManager.swe.swe_calc_ut(GeoData.ActiveData.Tjd_ut, p, _iflag, _x2, ref _serr);

                if (_iflgret < 0)
                    Debug.Log("error: " + _serr);

                // calculate azimuth and altitude
                SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, _x2, _xaz);

                if (_iflgret < 0)
                    Debug.Log("error: " + _serr);

                // populate tempX2 for latitude 0
                for (int i = 0; i < _tempX2.Length; i++)
                    _tempX2[i] = _x2[i];


                _tempX2[1] = 0;
                _tempX2[2] = 0;

                // calculate azimuth and altitude for latitude 0 (chart mode)
                SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, _tempX2, _secAz);

                // calculate phenomenal data
                _iflgret = SwissEphemerisManager.swe.swe_pheno_ut(GeoData.ActiveData.Tjd_ut, p, SwissEph.SEFLG_SWIEPH, _attr, ref _serr);

                if (_iflgret < 0)
                    Debug.Log("error: " + _serr);

                PlanetData.PlanetDataList[p].Attr = _attr;

                // assign positions to planets
                AssignPlanets(p);

            }
        }

        void AssignPlanets(int planetId)
        {
            PlanetData planet = PlanetData.PlanetDataList[planetId];
            double[] tempX2 = { _x2[0], _x2[1], _x2[2], _x2[3], _x2[4], _x2[5] };
            double[] tempXaz = { _xaz[0], _xaz[1], _xaz[2], _xaz[3], _xaz[4], _xaz[5] };
            double[] tempSecXaz = { _secAz[0], _secAz[1], _secAz[2], _secAz[3], _secAz[4], _secAz[5] };
            planet.X2 = tempX2;
            planet.Xaz = tempXaz;
            planet.ChartAz = tempSecXaz;

            EventManager.Instance.ApplyPlanetInfo(planetId);

            if (planet is NorthNodeData)
                CalculateSouthNode(planet);
        }

        void CalculateSouthNode(PlanetData node)
        {
            NorthNodeData northNode = node as NorthNodeData;
            PlanetData southNode = northNode.southNodeData;

            // get opposite position for the node
            _x2[0] = AstroFunctions.ConvertDegreesTo360(_x2[0] + 180);
            southNode.X2 = _x2;

            // calculate azimuth and altitude
            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, _x2, _xaz);
            southNode.Xaz = _xaz;

            // populate tempX2 for latitude 0
            for (int i = 0; i < _tempX2.Length; i++)
                _tempX2[i] = _x2[i];


            _tempX2[1] = 0;
            _tempX2[2] = 0;

            // calculate azimuth and altitude for latitude 0 (chart mode)
            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, _tempX2, _secAz);
            southNode.ChartAz = _secAz;

            EventManager.Instance.ApplyPlanetInfo(southNode.planetID);
        }

        void AssignMC()
        {
            double[] mcX2 = { _mc, 0, 0, 0, 0, 0 };
            double[] az = new double[6];

            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, mcX2, az);

            _midheaven.X2 = mcX2;
            _midheaven.Xaz = az;

        }

        void AssignAsc()
        {
            double[] ascX2 = { _asc, 0, 0, 0, 0, 0 };
            double[] az = new double[6];

            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, ascX2, az);

            _ascendant.X2 = ascX2;
            _ascendant.Xaz = az;
        }

    }

    public void PlacePlanetSymbols()
    {
        var conjunctionList = ConjunctionManager.Instance.ChartRegions.ToList();

        foreach(ChartRegion region in conjunctionList)
        {
            // first deal with conjunctions
            if(region is Conjunction)
            {
                var conjunction = (Conjunction)region;
                var activePlanetIDs = conjunction.ActivePlanetIDs;

                for (int j = 0; j < activePlanetIDs.Count; j++)
                {
                    PlanetData planet = PlanetData.PlanetDataList[activePlanetIDs[j]];

                    if (activePlanetIDs.Count == 1)
                    {
                        planet.PlaceSymbols();
                        planet.PlaceSymbols2D();
                    }
                    else
                    {
                        planet.PlaceConjunctSymbols(j, conjunction, conjunction.IsReversed);
                        planet.PlaceConjunctSymbols2D(j, conjunction);
                    } 
                }
                continue;
            }


            // deal with single planets
            var inconjunctPlanet = (InconjunctPlanet)region;
            inconjunctPlanet.Planet.PlaceSymbols();
            inconjunctPlanet.Planet.PlaceSymbols2D();
        }

    }

    void DrawChart2D()
    {
        _houseCusps = GeoData.ActiveData.HouseCusps;
        _ascmc = GeoData.ActiveData.Ascmc;

        _armc = GeoData.ActiveData.Armc;
        _mc = GeoData.ActiveData.Mc;
        _asc = GeoData.ActiveData.Asc;

        // rotate whole zodiac
        _zodiac.Rotation = -_asc;

        // rotate meridian
        // zodiac is meridian's parent
        _meridian.Rotation = _mc;
    }

}