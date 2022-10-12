using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AstroResources;

public class ConjunctionManager : MonoBehaviour
{
    static public ConjunctionManager Instance { get; private set; }

    [SerializeField] List<ChartRegion> _chartRegions = new List<ChartRegion>();
    public List<ChartRegion> ChartRegions { get { return _chartRegions; } private set { _chartRegions = value; } }

    [SerializeField] List<int> _conjunctPlanets = new List<int>();
    public List<int> ConjunctPlanets { get { return _conjunctPlanets; } private set { _conjunctPlanets = value; } }

    [SerializeField] List<int> _inconjunctPlanets = new List<int>();
    public List<int> InconjunctPlanets { get { return _inconjunctPlanets; } private set { _inconjunctPlanets = value; } }

    const double _minDistanceForConjunctions = 5;
    const double _minDistanceForReversing = 5;

    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else Debug.LogWarning("More than one ConjunctionManager. Delete this.");

        EventManager.Instance.OnMultiplePlanetsToggle += UpdateActivePlanets;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnMultiplePlanetsToggle -= UpdateActivePlanets;
    }

    public void ClearConjunctions()
    {
        _chartRegions.Clear();
        _conjunctPlanets.Clear();
    }

    public void FindCloseConjunctions()
    {
        _conjunctPlanets.Clear();
        _inconjunctPlanets.Clear();

        foreach (PlanetData firstPlanet in PlanetData.PlanetDataList)
        {
            foreach (PlanetData secPlanet in PlanetData.PlanetDataList)
            {
                if (firstPlanet.Longitude == 0 || secPlanet.Longitude == 0) continue;
                if (secPlanet == firstPlanet) continue;
                if (secPlanet.SignID != firstPlanet.SignID) continue;

                if (AstroFunctions.GetAbsolute360Distance(firstPlanet.Longitude, secPlanet.Longitude) <= _minDistanceForConjunctions)
                    AddConjunction(firstPlanet.planetID, secPlanet.planetID);
            }
        }

        foreach(PlanetData planet in PlanetData.PlanetDataList)
        {
            if (_conjunctPlanets.Contains(planet.planetID)) continue;

            AddInconjunctPlanet(planet.planetID);
        }
    }

    public Conjunction FindPlanet(int planetID)
    {
        foreach(Conjunction conjunction in _chartRegions)
            if (conjunction.Contains(planetID)) return conjunction;

        return null;
    }

    void AddConjunction(int firstPlanetID, int secondPlanetID)
    {
        bool appendedConjunction = false;

        foreach (Conjunction conjunction in _chartRegions)
        {
            if (conjunction.Contains(firstPlanetID) && conjunction.Contains(secondPlanetID))
            { 
                appendedConjunction = true;
                break;
            }
            
            if (conjunction.Contains(firstPlanetID))
            {
                conjunction.AddPlanet(secondPlanetID);
                _conjunctPlanets.Add(secondPlanetID);
                appendedConjunction = true;
                break;
            }
            if (conjunction.Contains(secondPlanetID))
            {
                conjunction.AddPlanet(firstPlanetID);
                _conjunctPlanets.Add(firstPlanetID);

                appendedConjunction = true;
                break;
            }
        }

        if(!appendedConjunction)
        {
            // create conjunction and add it to the list
            Conjunction newConjunction = new Conjunction(firstPlanetID, secondPlanetID);
            _chartRegions.Add(newConjunction);
            _conjunctPlanets.Add(firstPlanetID);
            _conjunctPlanets.Add(secondPlanetID);
        }

        ArrangeRegions();
    }

    void ArrangeRegions()
    {
        if (_chartRegions.Count == 1) return;

        _chartRegions = _chartRegions.OrderBy(o => o.Longitude).ToList();

        for (int i = 0; i < _chartRegions.Count; i++)
        {
            if (i == _chartRegions.Count - 1)
            {
                _chartRegions[i].NextRegion = _chartRegions[0];
                continue;
            }

            _chartRegions[i].NextRegion = _chartRegions[i + 1];
        }
    }

    void AddInconjunctPlanet(int planetID)
    {
        _chartRegions.Add(new InconjunctPlanet(planetID));

        ArrangeRegions();
    }

    void UpdateActivePlanets()
    {
        foreach(ChartRegion region in _chartRegions)
        {
            if(region is Conjunction)
            {
                var conjunction = (Conjunction)region;
                conjunction.FindActivePlanets();
            }
        }       
    }
}

[System.Serializable]
public abstract class ChartRegion
{
    protected double _longitude;
    public  double Longitude { get { return _longitude; } }

    protected ChartRegion nextRegion;
    public ChartRegion NextRegion { 
        get { return nextRegion; } 
        set { 
            nextRegion = value;

            if(nextRegion.PreviousRegion != this)
                nextRegion.PreviousRegion = this;
                
        } 
    }

    protected ChartRegion previousRegion;
    public ChartRegion PreviousRegion { 
        get { return previousRegion; } 
        set { 
            previousRegion = value;

            if (previousRegion.NextRegion != this)
                previousRegion.NextRegion = this;
        } 
    }

    public virtual double GetDistanceToNext()
    {
        return AstroFunctions.GetAbsolute360Distance(_longitude, nextRegion.Longitude);
    }

    public virtual double GetDistanceFromPrevious()
    {
        return AstroFunctions.GetAbsolute360Distance(_longitude, previousRegion.Longitude);
    }
    protected bool IsThereClosePlanet()
    {
        // in southern hemisphere, check for planets forward
        if (!GeoData.ActiveData._northernHemisphere)
        {
            if(GetDistanceToNext() < 13)
                return true;

            return false;
        }

        // in northern hemisphere, check for planets backward
        if (GetDistanceFromPrevious() < 13)
            return true;

        return false;
    }
}

[System.Serializable]
public class InconjunctPlanet : ChartRegion
{
    public InconjunctPlanet(int id)
    {
        planetID = id;
        planet = PlanetData.PlanetDataList[id];
        _longitude = planet.Longitude;
    }

    [SerializeField] int planetID;
    [SerializeField] PlanetData planet;

    public int PlanetID { get { return planetID; } }
    public PlanetData Planet { get { return planet; } }
}

[System.Serializable]

public class Conjunction : ChartRegion
{
    public Conjunction(int firstId, int secId)
    {
        planets = new List<PlanetData>();
        activePlanets = new List<PlanetData>();
        planetIDs = new List<int>();
        activePlanetIDs = new List<int>();

        AddPlanet(firstId);
        AddPlanet(secId);
    }

    [SerializeField] List<PlanetData> planets;
    [SerializeField] List<PlanetData> activePlanets;
    [SerializeField] List<int> planetIDs;
    [SerializeField] List<int> activePlanetIDs;
    [SerializeField] int minId;
    [SerializeField] int maxId;
    [SerializeField] double minLong;
    [SerializeField] double maxLong;
    [SerializeField] int activeMinId;
    [SerializeField] int activeMaxId;
    [SerializeField] double activeMidLong;
    [SerializeField] double activeMinLong;
    [SerializeField] double activeMaxLong;

    public int Size{ get { return planets.Count; } }
    public List<PlanetData> Planets { get { return planets; } }
    public List<int> PlanetIDs { get { return planetIDs; } }
    public int MinId { get { return minId; } }
    public int MaxId { get { return maxId; } }
    public double MinLong { get { return minLong; } }
    public double MaxLong { get { return maxLong; } }
    public int ActiveMinId { get { return activeMinId; } }
    public int ActiveMaxId { get { return activeMaxId; } }
    public double ActiveMinLong { get { return activeMinLong; } }
    public double ActiveMidLong { get { return activeMidLong; } }
    public double ActiveMaxLong { get { return activeMaxLong; } }
    public List<PlanetData> ActivePlanets { get { return activePlanets; } }
    public List<int> ActivePlanetIDs { get { return activePlanetIDs; } }
    public bool IsReversed { get { return IsThereClosePlanet(); } }

    public void AddPlanet(int id)
    {
        if (planetIDs.Contains(id)) return;

        PlanetData planet = PlanetData.PlanetDataList[id];

        planets.Add(planet);
        planetIDs.Add(id);
        if (planet.IsActive)
        {
            activePlanetIDs.Add(id);
            activePlanets.Add(planet);
        }

        RearrangeLists();

        ReassignMinMaxValues();
    }

    public void AddPlanet(PlanetData planet)
    {
        if (planetIDs.Contains(planet.planetID)) return;

        planets.Add(planet);
        planetIDs.Add(planet.planetID);
        if (planet.IsActive)
        {
            activePlanetIDs.Add(planet.planetID);
            activePlanets.Add(planet);
        }

        RearrangeLists();

        ReassignMinMaxValues();
    }

    public void RemovePlanet(int id)
    {
        PlanetData planet = PlanetData.PlanetDataList[id];
        if (!planets.Contains(planet))
        {
            Debug.LogError("Planet not contained within Conjunction.");
            return;
        }

        planets.Remove(planet);
        activePlanets.Remove(planet);
        planetIDs.Remove(id);
        activePlanetIDs.Remove(id);
    }

    public void RemovePlanet(PlanetData planet)
    {
        if (!planets.Contains(planet))
        {
            Debug.LogError("Planet not contained within Conjunction.");
            return;
        }


        planets.Remove(planet);
        activePlanets.Remove(planet);
        planetIDs.Remove(planet.planetID);
        activePlanetIDs.Remove(planet.planetID);
    }

    public void Clear()
    {
        planets.Clear();
        planetIDs.Clear();
        activePlanetIDs.Clear();
        activePlanets.Clear();

        minId = 0;
        maxId = 0;
        minLong = 0;
        _longitude = 0;
        maxLong = 0;
        activeMinId = 0;
        activeMaxId = 0;
        activeMinLong = 0;
        activeMidLong = 0;
        activeMaxLong = 0;
    }

    public bool Contains(int id)
    {
        if (planetIDs.Contains(id)) return true;

        return false;

    }

    public bool Contains(PlanetData planet)
    {
        if (planets.Contains(planet)) return true;

        return false;

    }

    public void FindActivePlanets()
    {
        activePlanetIDs.Clear();
        
        var tempPlanetIDList = planetIDs.ToList();
        var tempPlanetList = planets.ToList();

        foreach (int planetID in planetIDs)
        {
            if (!PlanetData.PlanetDataList[planetID].IsActive)
            {
                tempPlanetIDList.Remove(planetID);
                tempPlanetList.Remove(PlanetData.PlanetDataList[planetID]);
            }
        }

        activePlanetIDs = tempPlanetIDList;
        activePlanets = tempPlanetList;

        ReassignActiveValues();
    }

    void RearrangeLists()
    {
        planets = planets.OrderBy(o => o.Longitude).ToList();
        planetIDs = planetIDs.OrderBy(o => o).ToList();
    }

    void ReassignMinMaxValues()
    {
        minId = planetIDs[0];
        maxId = planetIDs[planets.Count - 1];
        minLong = planets[0].Longitude;
        maxLong = planets[planets.Count - 1].Longitude;
        _longitude = (minLong + maxLong) / 2;

        ReassignActiveValues();
    }

    void ReassignActiveValues()
    {
        if (activePlanetIDs.Count == 0) return;

        activeMinId = activePlanetIDs[0];
        activeMaxId = activePlanetIDs[activePlanetIDs.Count - 1];
        activeMinLong = activePlanets[0].Longitude;
        activeMaxLong = activePlanets[activePlanets.Count - 1].Longitude;
        activeMidLong = (activeMinLong + activeMaxLong) / 2;
    }

    public override double GetDistanceToNext()
    {
        return AstroFunctions.GetAbsolute360Distance(_longitude, nextRegion.Longitude);
    }

    public override double GetDistanceFromPrevious()
    {
        return AstroFunctions.GetAbsolute360Distance(_longitude, previousRegion.Longitude);
    }

}
