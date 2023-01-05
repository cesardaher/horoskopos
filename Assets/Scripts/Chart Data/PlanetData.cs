using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AstroResources;

public class PlanetData : PointData
{
    /* This class stores the data about each planet or planet-like object.
     * It is responsible for managing data such as position, speed, etc.
     * It is also responsible for managing the 3D (chart/sky objects) and 
     * 2D objects, changing their positions or disabling when necessary
     * 
     * Child of PointData, which manages points in 3D space and stores positional data.
    */
    
    // List of planets in program (visible in inspector)
    [SerializeField] List<PlanetData> planetList;
    
    // Static list of planets for global access
    static List<PlanetData> _planetDataList = new List<PlanetData>();
    
    // Property for planet list
    public static List<PlanetData> PlanetDataList
    {
        get { return _planetDataList; }

        private set { 
            _planetDataList = value;

#if UNITY_EDITOR
            // assigns list to each planet for visibility in inspector
            // only in editor
            foreach(PlanetData planet in _planetDataList)
            {
                planet.planetList = _planetDataList;
            }
#endif
        }
    }

    // State of planet, for toggling visibility
    protected bool _isActive;
    public virtual bool IsActive
    {
        get { return _isActive; }
        set
        {
            _isActive = value;

            // communicates states to all relevant objects
            ToggleState(_isActive);
        }
    }

    // State of animation
    // used for toggling between using true or apparent positions
    bool isAnimating = false;

    // Planetary objects that appear on screen
    [Header("Planet Objects")]
    // Chart 2D planet
    [SerializeField] Planet2D _planet2D;
    // Realistic sky planet
    [SerializeField] Planet3D _planet3D;
    // Chart 3D planet
    [SerializeField] Planet3D _planetChart3D;
    // Planetary chart symbol
    [SerializeField] PositionChartModeSymbols _clampSymbol;

    // Spacial data for 3D planetary objects
    public SpacePoint realPlanet { get; private set; }
    public SpacePoint chartPlanet { get; private set; }

    // Data about planet
    [Header("Chart Properties")]
    // Name of planet
    public string astroName;
    // ID of planet
    public int planetID;
    // Indicates direction of planet based on speed
    public bool Retrograde
    {
        get
        {
            if (speedLong < 0) return true;
            return false;
        }
    }

    // Position darta for planet
    [Header("Positional Data")]
    // Rotation in 2D chart
    double chartRotation;
    // Distance from Earth (unused)
    [SerializeField] double distance;
    // Speed in ecliptic latitude
    [SerializeField] double speedLat;
    // Speed in ecliptic longitude
    [SerializeField] double speedLong;
    // Speed in distance
    [SerializeField] double speedDistance;
    // Average ecliptic longitude speed
    [SerializeField] double speedAverage;
    // Longitude of conjunction
    // used when planet is conjunct for placing planet symbol
    public double conjunctionCenter;
    // Ecliptic longitude speed in degrees, minutes and seconds
    public int[] speedLongMinSec = new int[3];
    // Ecliptic latitude speed in degrees, minutes and seconds
    public int[] speedLatMinSec = new int[3];
    // Average longitude speed in degrees, minutes and seconds
    public int[] speedAverageLonMinSec = new int[3];

    // Phenomenonal data about planet (visibility)
    [Header("Phenomenonal Data")]
    // Angle distance relative to Sun
    public double phaseAngle;
    // Phase (full, quarter, etc.)
    public double phase;
    // Apparent angular diameter
    public double appDiameter;
    // Phase state property for Moon
    public int PhaseState
    {
        get { return AssignPhaseState(); }
    }

    // Auxiliary variables
    // For calculating phenomenonal data
    double[] _attr = new double[20];
    // For calculating azimuth and altitude position
    [SerializeField] double[] _chartAz = new double[6];

    // Properties for assigning positional data
    // these are based on variables in PointData.cs
    // Property for ecliptic positions, assigns values to all relevant fields
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
    // Property for horizontal positions
    // gets horizontal positions and puts planet in space
    public override double[] Xaz
    {
        get { return _xaz; }
        set
        {
            _xaz = value;
            realPlanet.Azimuth = _xaz[0];
            realPlanet.TrAlt = _xaz[1];
            realPlanet.AppAlt = _xaz[2];

            // use true positions for animation and apparent for static
            // this avoids stuttering on animation
            if (isAnimating)
                chartPlanet.planet.transform.position = AstroFunctions.HorizontalToCartesian(chartPlanet.Azimuth, chartPlanet.AppAlt);
            else
                chartPlanet.planet.transform.position = AstroFunctions.HorizontalToCartesian(chartPlanet.Azimuth, chartPlanet.TrAlt);

            // communicates with LightCubemap.cs
            // changes lighting when Sun's position changes
            EventManager.Instance.ApplyPlanetPosition(planetID);
        }
    }
    // Property for rotation of 2D planet in chart
    public double ChartRotation
    {
        get { return chartRotation; }
        set
        {
            chartRotation = value;
        }
    }
    // Property for ecliptic latitude speed
    public double SpeedLat
    {
        get { return speedLat; }
        set
        {
            speedLat = value;
            AstroFunctions.DecimalToMinSec(speedLat, out speedLatMinSec);
        }
    }
    // Property for ecliptic longitude speed
    public double SpeedLong
    {
        get { return speedLong; }
        set
        {
            speedLong = value;
            AstroFunctions.DecimalToMinSec(speedLong, out speedLongMinSec);
        }
    }
    // Property for distance speed
    public double SpeedDist
    {
        get { return speedDistance; }
        set
        {
            speedDistance = value;
        }
    }
    // Property for average longitude speed
    public double SpeedAverage
    {
        get { return speedAverage; }
        set
        {
            speedAverage = value;
            AstroFunctions.DecimalToMinSec(speedAverage, out speedAverageLonMinSec);
        }
    }
    // Property for horizontal positions (azimuth, altitude) in chart mode
    public double[] ChartAz
    {
        get { return _chartAz; }
        set
        {
            _chartAz = value;
            chartPlanet.Azimuth = _chartAz[0];
            chartPlanet.TrAlt = _chartAz[1];
            chartPlanet.AppAlt = _chartAz[2];

            // positions planets in space
            realPlanet.planet.transform.position = AstroFunctions.HorizontalToCartesian(realPlanet.Azimuth, realPlanet.TrAlt);
        }
    }
    // Property for assinging phenomenonal data
    public double[] Attr
    {
        get { return _attr; }

        set
        {
            _attr = value;
            phaseAngle = _attr[0];
            phase = _attr[1];
            appDiameter = _attr[3];
        }
    }

    // AWAKE
    // Sets up both realistic and chart planet
    private void Awake()
    {
        IsActive = true;
        realPlanet = new SpacePoint(this, _planet3D);
        chartPlanet = new SpacePoint(this, _planetChart3D);
    }

    // START
    // Sets up list of planets and events
    private void Start()
    {
        PlanetDataList.Add(this);
        _planet2D.ParentPlanet = this;
        PlanetDataList = PlanetDataList.OrderBy(x => x.planetID).ToList();

        EventManager.Instance.OnCalculatedPlanet += AssignPlanet;
        EventManager.Instance.OnAnimationStart += StartAnimationState;
        EventManager.Instance.OnAnimationEnd += StopAnimationState;

    }


    // Assigns average speed in inspector
    private void OnValidate()
    {
        SpeedAverage = speedAverage;
    }

    // ONDESTROY
    // Dettaches events and empties planet list
    void OnDestroy()
    {
        EventManager.Instance.OnCalculatedPlanet -= AssignPlanet;
        EventManager.Instance.OnAnimationStart -= StartAnimationState;
        EventManager.Instance.OnAnimationEnd -= StopAnimationState;
        PlanetDataList.Remove(this);
    }

    // Places planetary symbols in chart mode
    public void PlaceSymbols()
    {
        _clampSymbol.FindLabelsPositions(_longitude, Retrograde);
    }

    // Places 2D planetary symbols (rotate)
    public void PlaceSymbols2D()
    {
        _planet2D.PlaceSymbol(_longitude);
    }

    // Places planetary symbols in chart mode in conjunction
    // planets in conjunction accomodate for each other due to proximity
    public void PlaceConjunctSymbols(int i, Conjunction conjunction, bool isReverse)
    {

        if (_clampSymbol != null && ModesMenu.chartModeOn)
        {
            conjunctionCenter = conjunction.ActiveMidLong;
            _clampSymbol.PositionConjunctLabels(conjunctionCenter, i, Retrograde, isReverse);
        }
    }

    // Places 2D planetary symbols in conjunction
    // planets in conjunction accomodate for each other due to proximity
    public void PlaceConjunctSymbols2D(int index, Conjunction conjunction)
    {

        conjunctionCenter = conjunction.ActiveMidLong;
        int conjunctionSize = conjunction.ActivePlanets.Count;
        _planet2D.PlaceSymbolConjunct(conjunctionCenter, index, conjunctionSize);
    }

    // Static: Reassigns positional data for all planets
    public static void RessignAllPlanets()
    {
        foreach (PlanetData planet in PlanetDataList)
        {
            planet.AssignPlanet(planet.planetID);
        }
    }

    // Assigns positional data for single planet
    void AssignPlanet(int id)
    {
        if (id == planetID)
        {
            X2 = _x2;
            Xaz = _xaz;
            ChartAz = _chartAz;
        }
    }

    // Assigns a phase for the planet (waning, waxing)
    int AssignPhaseState()
    {
        // ignores Sun, as planetary phases are defined by proximity to it
        if (PlanetDataList[0] == this) return 0;

        // if positive value, waxing
        if (AstroFunctions.Get360Distance(_longitude, PlanetDataList[0]._longitude) >= 0)
            return 1;

        // negative, waning
        return -1;

    }

    // Enable/disable planetary objects when active/inactive
    // for the Sun (ID = 0), change only visibility
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

    // Changes animation state to use either apparent or true positions in 3D
    void StartAnimationState()
    {
        isAnimating = true;
    }
    void StopAnimationState()
    {
        isAnimating = false;

        // revert position to apparent
        chartPlanet.planet.transform.position = AstroFunctions.HorizontalToCartesian(chartPlanet.Azimuth, chartPlanet.AppAlt);
    }
}