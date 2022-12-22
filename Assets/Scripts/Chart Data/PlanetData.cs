using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AstroResources;

public class PlanetData : PointData
{

    [SerializeField] List<PlanetData> planetList;
    static List<PlanetData> _planetDataList = new List<PlanetData>();
    public static List<PlanetData> PlanetDataList
    {
        get { return _planetDataList; }

        private set { 
            _planetDataList = value;

#if UNITY_EDITOR
            foreach(PlanetData planet in _planetDataList)
            {
                planet.planetList = _planetDataList;
            }
#endif
        }
    }

    protected bool _isActive;
    public virtual bool IsActive
    {
        get { return _isActive; }
        set
        {
            _isActive = value;

            ToggleState(_isActive);
        }
    }

    [Header("Planet Objects")]
    [SerializeField] Planet2D _planet2D;
    [SerializeField] Planet3D _planet3D;
    [SerializeField] Planet3D _planetChart3D;
    [SerializeField] PositionChartModeSymbols _clampSymbol;
    public SpacePoint realPlanet { get; private set; }
    public SpacePoint chartPlanet { get; private set; }

    [Header("Chart Properties")]
    public string astroName;
    double chartRotation;
    public int planetID;
    public bool Retrograde
    {
        get
        {
            if (speedLong < 0) return true;
            return false;
        }
    }

    [Header("Positional Data")]
    [SerializeField] double distance;
    [SerializeField] double speedLat;
    [SerializeField] double speedLong;
    [SerializeField] double speedDistance;
    [SerializeField] double speedAverage;
    public double conjunctionCenter;

    public int[] speedLongMinSec = new int[3];
    public int[] speedLatMinSec = new int[3];
    public int[] speedAverageLonMinSec = new int[3];

    [Header("Phenomenal Data")]
    public double phaseAngle;
    public double phase;
    public double appDiameter;
    public int PhaseState
    {
        get { return AssignPhaseState(); }
    }

    double[] attr = new double[20];
    [SerializeField] double[] _chartAz = new double[6];

    public override double[] X2
    {
        get { return _x2; }
        set
        {
            _x2 = value;
            Longitude = _x2[0];
            Latitude = _x2[1];

            ChartRotation = _x2[0];
            distance = _x2[2];
            SpeedLong = _x2[3];
            SpeedLat = _x2[4];
            SpeedDist = _x2[5];
        }
    }
    public override double[] Xaz
    {
        get { return _xaz; }
        set
        {
            _xaz = value;
            realPlanet.Azimuth = _xaz[0];
            realPlanet.TrAlt = _xaz[1];
            realPlanet.AppAlt = _xaz[2];

            // REVERT
            realPlanet.planet.transform.GetChild(0).position = AstroFunctions.HorizontalToCartesian(realPlanet.Azimuth, realPlanet.AppAlt);

            EventManager.Instance.ApplyPlanetPosition(planetID);
        }
    }
    public double ChartRotation
    {
        get { return chartRotation; }
        set
        {
            //rotate planet as soon as assigned
            chartRotation = value;
        }
    }
    public double SpeedLat
    {
        get { return speedLat; }
        set
        {
            speedLat = value;
            AstroFunctions.DecimalToMinSec(speedLat, out speedLatMinSec);
        }
    }
    public double SpeedLong
    {
        get { return speedLong; }
        set
        {
            speedLong = value;
            AstroFunctions.DecimalToMinSec(speedLong, out speedLongMinSec);
        }
    }
    public double SpeedDist
    {
        get { return speedDistance; }
        set
        {
            speedDistance = value;
        }
    }
    public double SpeedAverage
    {
        get { return speedAverage; }
        set
        {
            speedAverage = value;
            AstroFunctions.DecimalToMinSec(speedAverage, out speedAverageLonMinSec);
        }
    }
    public double[] ChartAz
    {
        get { return _chartAz; }
        set
        {
            _chartAz = value;
            chartPlanet.Azimuth = _chartAz[0];
            chartPlanet.TrAlt = _chartAz[1];
            chartPlanet.AppAlt = _chartAz[2];
        }
    }
    public double[] Attr
    {
        get { return attr; }

        set
        {
            attr = value;
            phaseAngle = attr[0];
            phase = attr[1];
            appDiameter = attr[3];
        }
    }

    private void Awake()
    {
        IsActive = true;
        realPlanet = new SpacePoint(this, _planet3D);
        chartPlanet = new SpacePoint(this, _planetChart3D);
    }

    private void Start()
    {
        PlanetDataList.Add(this);
        _planet2D.ParentPlanet = this;
        PlanetDataList = PlanetDataList.OrderBy(x => x.planetID).ToList();
        EventManager.Instance.OnCalculatedPlanet += AssignPlanet;

    }

    private void OnValidate()
    {
        SpeedAverage = speedAverage;
    }

    void OnDestroy()
    {
        EventManager.Instance.OnCalculatedPlanet -= AssignPlanet;
        PlanetDataList.Remove(this);
    }

    public void PlaceSymbols()
    {
        _clampSymbol.FindLabelsPositions(_longitude, Retrograde);
    }

    public void PlaceSymbols2D()
    {
        _planet2D.PlaceSymbol(_longitude);
    }

    public void PlaceConjunctSymbols(int i, Conjunction conjunction, bool isReverse)
    {

        if (_clampSymbol != null && ModesMenu.chartModeOn)
        {
            conjunctionCenter = conjunction.ActiveMidLong;
            _clampSymbol.PositionConjunctLabels(conjunctionCenter, i, Retrograde, isReverse);
        }
    }

    public void PlaceConjunctSymbols2D(int index, Conjunction conjunction)
    {

        conjunctionCenter = conjunction.ActiveMidLong;
        int conjunctionSize = conjunction.ActivePlanets.Count;
        _planet2D.PlaceSymbolConjunct(conjunctionCenter, index, conjunctionSize);
    }

    public static void RessignAllPlanets()
    {
        foreach (PlanetData planet in PlanetDataList)
        {
            planet.AssignPlanet(planet.planetID);
        }
    }

    void AssignPlanet(int id)
    {
        if (id == planetID)
        {

            X2 = _x2;
            Xaz = _xaz;
            ChartAz = _chartAz;
        }
    }

    int AssignPhaseState()
    {
        if (PlanetDataList[0] == this) return 0;

        if (AstroFunctions.Get360Distance(_longitude, PlanetDataList[0]._longitude) >= 0)
            return 1;

        return -1;

    }

    void ToggleState(bool val)
    {
        if (val)
        {
            if (planetID == 0)
                _planet3D.spriteRenderer.enabled = true;
            else
                _planet3D.gameObject.SetActive(true);

            _planet2D.gameObject.SetActive(true);
            _planetChart3D.gameObject.SetActive(true);
            _clampSymbol.gameObject.SetActive(true);
        }
        else
        {
            if (planetID == 0)
                _planet3D.spriteRenderer.enabled = false;
            else
                _planet3D.gameObject.SetActive(false);

            _planet2D.gameObject.SetActive(false);
            _planetChart3D.gameObject.SetActive(false);
            _clampSymbol.gameObject.SetActive(false);
        }
    }
}