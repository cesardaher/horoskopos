using System;
using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    public DateTime ActiveDateTime { get; private set; }
    GeoData _tempGeoData;
    [SerializeField] int _speed;
    readonly int _maxSpeed = 86400000 * 3;
    readonly int _minSpeed = 1000;

    [SerializeField] PlayMenu _playMenu;

    static public bool IsTimeRunning
    {
        get; private set;
    }

    [Header("Active Data")]
    [SerializeField] double Tjd_ut;
    [SerializeField] [Range(1, 31)] int day;
    [SerializeField] [Range(1, 12)] int month;
    [SerializeField] [Range(0, 2100)] int year;
    [SerializeField] [Range(0, 23)] int hour;
    [SerializeField] [Range(0, 59)] int min;
    [SerializeField] [Range(0, 59)] double sec;

    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else Debug.LogWarning("More than one TimeManager. Delete this.");

        EventManager.Instance.OnRecalculationOfGeoData += CollectTimeFromGeoData;
        EventManager.Instance.OnTitleScreenReturn += StopCoroutines;
    }

    private void OnDestroy()
    {
        Instance = null;

        EventManager.Instance.OnRecalculationOfGeoData -= CollectTimeFromGeoData;
        EventManager.Instance.OnTitleScreenReturn -= StopCoroutines;
    }

    private void Start()
    {
        CollectTimeFromGeoData();
    }

    void CollectTimeFromGeoData()
    {
        ActiveDateTime = GeoData.ActiveData.DateTime;

        Tjd_ut = GeoData.ActiveData.Tjd_ut;
        day = GeoData.ActiveData.Iday;
        month = GeoData.ActiveData.Imon;
        year = GeoData.ActiveData.Iyear;
        hour = GeoData.ActiveData.Ihour;
        min = GeoData.ActiveData.Imin;
        sec = GeoData.ActiveData.Dsec;
    }

    public void BackwardsAnimation()
    {
       
        if (IsTimeRunning)
            StopAnimation();

        if (_speed > 0)
            _speed = -_minSpeed;

        _playMenu.BackState();
        StartCoroutine(nameof(RunTimeAtSetSpeed), _speed);
    }

    public void StartAnimation()
    {

        if (IsTimeRunning)
            StopAnimation();

        if (_speed < 0)
            _speed = _minSpeed;

        _playMenu.PlayState();
        StartCoroutine(nameof(RunTimeAtSetSpeed), _speed);
    }

    public void StopAnimation()
    {
        if (IsTimeRunning)
        {
            _playMenu.StopState();
            StopAllCoroutines();
            EventManager.Instance.StopAnimation();
            // do one last calculation to show better definition on lines
            ReassignGeoData();
            IsTimeRunning = false;
        }
    }

    void BreakAnimation()
    {
        StopAllCoroutines();
    }


    IEnumerator RunTimeAtSetSpeed(int secRate)
    {
        IsTimeRunning = true;
        EventManager.Instance.StartAnimation();

        while (IsTimeRunning)
        {
            if(secRate == _minSpeed || secRate == -_minSpeed)
            {
                // this calls for changing vertices on line renderers
                TimeStep(secRate);

                yield return new WaitForSecondsRealtime(1);
            }

            TimeStep(secRate / 60);
            yield return new WaitForFixedUpdate();
        }

        // this calls for changing vertices on line renderers
        EventManager.Instance.StopAnimation();
        ReassignGeoData();


        void TimeStep(int secRate)
        {
            if(secRate == _minSpeed) ActiveDateTime = ActiveDateTime.AddSeconds(1);
            else if(secRate == -_minSpeed) ActiveDateTime = ActiveDateTime.AddSeconds(-1);
            else ActiveDateTime = ActiveDateTime.AddMilliseconds(secRate);

            day = ActiveDateTime.Day;
            month = ActiveDateTime.Month;
            year = ActiveDateTime.Year;
            hour = ActiveDateTime.Hour;
            min = ActiveDateTime.Minute;
            sec = ActiveDateTime.Second + ((double)ActiveDateTime.Millisecond/1000);

            ReassignGeoData();
        }

    }

    public void IncreaseSpeed()
    {
        if (IsTimeRunning)
        {
            int newSpeed;
            // for positive speeds double them
            if (_speed > 0)
            {
                newSpeed = _speed * 3;
                if (newSpeed > _maxSpeed) return;
            }
            else // for negative speeds, halve them
            {
                newSpeed = _speed / 3;
                if (newSpeed > -_minSpeed) newSpeed = _minSpeed;
            }

            BreakAnimation();
            _speed = newSpeed;
            StartCoroutine(nameof(RunTimeAtSetSpeed), _speed);
        }

    }

    public void DecreaseSpeed()
    {

        if (IsTimeRunning)
        {
            int newSpeed;
            // for positive speeds halve them
            if (_speed > 0)
            {
                newSpeed = _speed / 3;
                if (newSpeed < _minSpeed) newSpeed = -_minSpeed;
            }
            else // for negative speeds, multiply
            {
                newSpeed = _speed * 3;
                if (newSpeed < -_maxSpeed) return;
            }

            BreakAnimation();
            _speed = newSpeed;
            StartCoroutine(nameof(RunTimeAtSetSpeed), _speed);
        }
    }

    void ReassignGeoData()
    {
        GeoData.ActiveData.InitializeDataDateTime(GeoData.ActiveData.DataName, ActiveDateTime, GeoData.ActiveData.D_timezone, GeoData.ActiveData.Geolat, GeoData.ActiveData.Geolon, GeoData.ActiveData.Height, GeoData.ActiveData.Hsys, GeoData.ActiveData.DaylightSavings);
        //GeoData.ActiveData.InitializeData(GeoData.ActiveData.DataName, ActiveDateTime.Day, ActiveDateTime.Month, ActiveDateTime.Year, ActiveDateTime.Hour, ActiveDateTime.Minute, ActiveDateTime.Second + ((double)ActiveDateTime.Millisecond / 1000), GeoData.ActiveData.D_timezone, GeoData.ActiveData.Geolat, GeoData.ActiveData.Geolon, GeoData.ActiveData.Height, GeoData.ActiveData.Hsys, GeoData.ActiveData.DaylightSavings);
    }

    public void StopCoroutines()
    {
        StopAllCoroutines();
    }
}
