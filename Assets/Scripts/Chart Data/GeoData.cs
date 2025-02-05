using SwissEphNet;
using System;
using UnityEngine;
using CosineKitty;

[CreateAssetMenu] [Serializable]
public class GeoData : ScriptableObject
{
    [SerializeField] CsvReader cityDatabase;
    [SerializeField] TimezoneReader timezoneDb;
    static GeoData activeData;
    public static GeoData ActiveData
    {
        get { return activeData; }
        set { 
            activeData = value;

            if (activeData == null) return;

            EventManager.Instance.GeoDataRecalculateEvent();
        }
    }

    [Header("Profile Name")]
    [SerializeField] string _dataName;

    [Header("Date and Time")]
    [SerializeField] int _iday;
    [SerializeField] int _imon;
    [SerializeField] int _iyear;
    [SerializeField] int _ihour;
    [SerializeField] int _imin;
    [SerializeField] double _d_timezone;
    [SerializeField] double _dsec;
    [SerializeField] double _tjd_et, _tjd_ut;
    string serr = "";
    public DateTime DateTime { get; private set; }

    [SerializeField] char _hsys = 'P';

    // final date
    [SerializeField] int _fday;
    [SerializeField] int _fmon;
    [SerializeField] int _fyear;
    [SerializeField] int _fhour;
    [SerializeField] int _fmin;
    [SerializeField] double _fsec;
    [SerializeField] double _ftimezone;

    [Header("Geolocation")]
    [SerializeField] string _city;
    [SerializeField] int _cityId;
    [SerializeField] double _geolon;
    [SerializeField] double _geolat;
    [SerializeField] double _height; // meters
    [SerializeField] bool _daylightSavings;
    public bool _northernHemisphere;

    double[] _geopos = new double[3];

    [Header("ChartData")]
    double[] _houseCusps = new double[13];
    double[] _ascmc = new double[10];
    double _armc;
    double _asc;
    double _mc;

    public string DataName { get { return _dataName; } private set { _dataName = value; } }
    public int Iday { get { return _iday; } private set { _iday = value; } }
    public int Imon { get { return _imon; } private set { _imon = value; } }
    public int Iyear { get { return _iyear; } private set { _iyear = value; } }
    public int Ihour { get { return _ihour; } private set { _ihour = value; } }
    public int Imin { get { return _imin; } private set { _imin = value; } }
    public double Dsec { get { return _dsec; } private set { _dsec = value; } }
    public double D_timezone { get { return _d_timezone; } private set { _d_timezone = value; } }
    public double F_timezone { get { return _ftimezone; } private set { _ftimezone = value; } }
    public bool DaylightSavings { get { return _daylightSavings; } private set { _daylightSavings = value; } }
    public bool NorthernHemisphere { get { return _northernHemisphere; } private set { _northernHemisphere = value; } }
    public double Tjd_et { get { return _tjd_et; } private set { _tjd_et = value; } }
    public double Tjd_ut { get { return _tjd_ut; } private set { _tjd_ut = value; } }
    public double Geolat { get { return _geolat; } private set { _geolat = value; } }
    public double Geolon { get { return _geolon; } private set { _geolon = value; } }
    public double Height { get { return _height; } private set { _height = value; } }
    public char Hsys { get { return _hsys; } private set { _hsys = value; } }
    public string City { get { return _city; } private set { _city = value; } }
    public int CityId { get { return _cityId; }
        set {
            _cityId = value;

            // assign city data
            if (_cityId != 0)
                GetCoordinatesFromCity(_cityId);
        }
    }

    public double[] Geopos { get { return _geopos; } private set { _geopos = value; } }
    public double[] HouseCusps { get { return _houseCusps; } private set { _houseCusps = value; } }
    public double[] Ascmc { get { return _ascmc; } private set { _ascmc = value; } }
    public double Armc { get { return _armc; } private set { _armc = value; } }
    public double Asc { get { return _asc; } private set { _asc = value; } }
    public double Mc { get { return _mc; } private set { _mc = value; } }

    public Observer observer = new Observer();
    public AstroTime astroTime;

    private void InitializeObserver()
    {
        observer = new Observer(Geolat, Geolon, Height);
    }

    private void InitializeAstroTime(DateTime dt)
    {
        astroTime = new AstroTime(dt);
    }

    private void InitializeAstroTime()
    {
        astroTime = new AstroTime(Iyear, Imon, Iday, Ihour, Imin, Dsec);
    }

    private void OnValidate()
    {
        if (cityDatabase is null) cityDatabase = Resources.Load<CsvReader>("CityDatabase/CityDatabase");
        if (timezoneDb is null) timezoneDb = Resources.Load<TimezoneReader>("CityDatabase/timezones/TimezoneDatabase");
        //ResetCalculationsOnValidate();
    }

    public void InitializeData(string name, int iday, int imon, int iyear, int ihour, int imin, double dsec, double d_timezone, double lat, double lon, double height, char houseSys, bool daylight)
    {
        if (cityDatabase is null) cityDatabase = Resources.Load<CsvReader>("CityDatabase/CityDatabase");
        if (timezoneDb is null) timezoneDb = Resources.Load<TimezoneReader>("CityDatabase/timezones/TimezoneDatabase");

        _dataName = name;
        Iday = iday;
        Imon = imon;
        Iyear = iyear;
        Ihour = ihour;
        Imin = imin;
        Dsec = dsec;
        D_timezone = d_timezone;
        Geolat = lat;
        Geolon = lon;
        Height = height;
        Hsys = houseSys;
        DaylightSavings = daylight;
        CheckHemisphere();
        InitializeObserver();
        InitializeAstroTime();

        ResetCalculations();

        // broadcast event if this is the active GeoData
        if (ActiveData == this)
            EventManager.Instance.GeoDataRecalculateEvent();
    }

    public void InitializeDataDateTime(string name, DateTime dateTime, double d_timezone, double lat, double lon, double height, char houseSys, bool daylight)
    {
        if (cityDatabase is null) cityDatabase = Resources.Load<CsvReader>("CityDatabase/CityDatabase");
        if (timezoneDb is null) timezoneDb = Resources.Load<TimezoneReader>("CityDatabase/timezones/TimezoneDatabase");

        DateTime = dateTime;

        _dataName = name;
        Iday = dateTime.Day;
        Imon = dateTime.Month;
        Iyear = dateTime.Year;
        Ihour = dateTime.Hour;
        Imin = dateTime.Minute;
        Dsec = dateTime.Second + (double)dateTime.Millisecond / 1000;
        D_timezone = d_timezone;
        Geolat = lat;
        Geolon = lon;
        Height = height;
        Hsys = houseSys;
        DaylightSavings = daylight;
        CheckHemisphere();
        InitializeObserver();
        InitializeAstroTime(dateTime);

        ResetCalculationsWithoutDateTime();

        // broadcast event if this is the active GeoData
        if (ActiveData == this)
            EventManager.Instance.GeoDataRecalculateEvent();
    }

    // GeoLon/GeoLat are set through CityId property
    public void InitializeDataWithCityIdDateTime(string name, DateTime dateTime, int cityId, char houseSys, bool daylight)
    {
        if (cityDatabase is null) cityDatabase = Resources.Load<CsvReader>("CityDatabase/CityDatabase");
        if (timezoneDb is null) timezoneDb = Resources.Load<TimezoneReader>("CityDatabase/timezones/TimezoneDatabase");


        DateTime = dateTime;

        _dataName = name;
        Iday = dateTime.Day;
        Imon = dateTime.Month;
        Iyear = dateTime.Year;
        Ihour = dateTime.Hour;
        Imin = dateTime.Minute;
        Dsec = dateTime.Second + dateTime.Millisecond/1000;
        CityId = cityId;
        Hsys = houseSys;
        DaylightSavings = daylight;

        InitializeAstroTime(dateTime);

        CheckHemisphere();
        CheckTimezone();

        ResetCalculationsWithoutDateTime();

        // broadcast event if this is the active GeoData
        if (ActiveData == this)
            EventManager.Instance.GeoDataRecalculateEvent();
    }


    // REMOVE THIS
    public void InitializeDataWithCityId(string name, int iday, int imon, int iyear, int ihour, int imin, double dsec, int cityId, char houseSys, bool daylight)
    {
        if (cityDatabase is null) cityDatabase = Resources.Load<CsvReader>("CityDatabase/CityDatabase");
        if (timezoneDb is null) timezoneDb = Resources.Load<TimezoneReader>("CityDatabase/timezones/TimezoneDatabase");

        _dataName = name;
        Iday = iday;
        Imon = imon;
        Iyear = iyear;
        Ihour = ihour;
        Imin = imin;
        Dsec = dsec;
        CityId = cityId;
        Hsys = houseSys;
        DaylightSavings = daylight;
        CheckHemisphere();
        CheckTimezone();

        ResetCalculations();

        // broadcast event if this is the active GeoData
        if (ActiveData == this)
            EventManager.Instance.GeoDataRecalculateEvent();
    }

    // REMOVE THIS
    public void InitializeDataWithCityIdAndTimeOffset(string name, int iday, int imon, int iyear, int ihour, int imin, double dsec, int cityId, char houseSys, double offset)
    {
        if (cityDatabase is null) cityDatabase = Resources.Load<CsvReader>("CityDatabase/CityDatabase");
        if (timezoneDb is null) timezoneDb = Resources.Load<TimezoneReader>("CityDatabase/timezones/TimezoneDatabase");

        _dataName = name;
        Iday = iday;
        Imon = imon;
        Iyear = iyear;
        Ihour = ihour;
        Imin = imin;
        Dsec = dsec;
        CityId = cityId;
        Hsys = houseSys;
        D_timezone = offset;
        DaylightSavings = false;
        CheckHemisphere();
        CheckTimezone();

        ResetCalculations();

        // broadcast event if this is the active GeoData
        if (ActiveData == this)
            EventManager.Instance.GeoDataRecalculateEvent();
    }

    public void CalculateTime()
    {
        Int32 retval;

        Int32 gregflag = SwissEph.SE_GREG_CAL;

        /* time zone = Indian Standard Time; NOTE: east is positive */

        double[] dret = new double[2];

        SwissEphemerisManager.swe.swe_utc_time_zone(_iyear, _imon, _iday, _ihour, _imin, _dsec, _ftimezone, ref _fyear, ref _fmon, ref _fday, ref _fhour, ref _fmin, ref _dsec);

        retval = SwissEphemerisManager.swe.swe_utc_to_jd(_fyear, _fmon, _fday, _fhour, _fmin, _fsec, gregflag, dret, ref serr);
        if (retval == SwissEph.ERR)
            Debug.LogWarning("Error: "+ serr);

        _tjd_et = dret[0]; // this is ET (TT)
        _tjd_ut = dret[1]; // this is UT (UT1)     
    }

    public static void RecalculateHouses()
    {
        ActiveData.ComputeHouses();
    }

    public static void GetHouseSystemFromEvent(char hSys) => ActiveData.Hsys = hSys;

    void ComputeHouses()
    {
        SwissEphemerisManager.swe.swe_houses(Tjd_ut, Geolat, Geolon, _hsys, _houseCusps, _ascmc);

        AssignHouseMeridian(_ascmc);
    }

    void AssignHouseMeridian(double[] ascmc)
    {
        _armc = ascmc[2];
        _mc = ascmc[1];
        _asc = ascmc[0];
    }

    void AssignGeoPos()
    {
        _geopos[0] = Geolon;
        _geopos[1] = Geolat;
        _geopos[2] = Height;

        InitializeObserver();
    }

    void GetCoordinatesFromCity(int id)
    {
        _city = cityDatabase.listOfCityNames[id];
        _geolon = cityDatabase.listOfCities.cities[id].longitude;
        _geolat = cityDatabase.listOfCities.cities[id].latitude;
        _height = cityDatabase.listOfCities.cities[id].elevation;
        _d_timezone = FindTimezone(cityDatabase.listOfCities.cities[id].timezone);

        AssignGeoPos();


        float FindTimezone(string val)
        {
            int index = timezoneDb.listOfTimezoneNames.FindIndex(s => s == val);
            float timezoneOffset;

            timezoneOffset = timezoneDb.listOfTimezones.timezones[index].offset;

            return timezoneOffset;
        }
    }

    void CheckHemisphere()
    {
        if (Geolat >= 0)
        {
            NorthernHemisphere = true;
            return;
        }

        NorthernHemisphere = false;
    }

    public void CheckTimezone()
    {
        if(DaylightSavings)
        {
            F_timezone = D_timezone + 1;
            return;
        }

        F_timezone = D_timezone;
    }

    void InitializeArrays()
    {
        _houseCusps = new double[13];
        _ascmc = new double[10];
    }

    void GenerateDateTime()
    {
        DateTime = new DateTime(Iyear, Imon, Iday, Ihour, Imin, 0);
        DateTime = DateTime.AddSeconds(Dsec);
    }

    private void ResetCalculationsWithoutDateTime()
    {
        CheckHemisphere();
        CheckTimezone();
        InitializeArrays();
        AssignGeoPos();
        CalculateTime();
        ComputeHouses();
    }

    private void ResetCalculations()
    {
        CheckHemisphere();
        CheckTimezone();
        GenerateDateTime();
        InitializeArrays();
        AssignGeoPos();
        CalculateTime();
        ComputeHouses();
    }

    private void ResetCalculationsOnValidate()
    {
        InitializeArrays();
        AssignGeoPos();
        CheckTimezone();
        CalculateTime();
        ComputeHouses();
    }
}
