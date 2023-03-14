using SwissEphNet;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using AstroResources;

public class HousesDrawer : MonoBehaviour
{

    /* This class is responsible for drawing the house cusps in 3D, as well as placing the markers that indicate each of them. */

    [SerializeField] CuspExtender houseCuspExtender;
    [SerializeField] GameObject cuspHolder;
    [SerializeField] GameObject midHouseModel;
    [SerializeField] HouseData houseData;

    [SerializeField] List<CuspExtender> houseCusps = new List<CuspExtender>();
    [SerializeField] List<Point3D> houseNumbers = new List<Point3D>();

    // active cusp for calculations
    CuspExtender _activeCusp;
    Point3D _activeHouseMarker;

    // flags for house systems that only need to be calculated once
    bool areCampanusHousesCalculated;
    bool areRegiomontanusHousesCalculated;

    // current house system used
    char hSys;

    // constants for the placement of markers in 3D
    const float houseMarkerRange = 80;
    const float campanusHouseMarkerRange = 10;

    void Awake()
    {
        // add events to Event Manager
        EventManager.Instance.OnRecalculationOfGeoData += DrawHouses;
        EventManager.Instance.OnHouseReassignment += DrawHouses;
    }

    private void OnDestroy()
    {
        // remove events from EventManager
        EventManager.Instance.OnRecalculationOfGeoData -= DrawHouses;
        EventManager.Instance.OnHouseReassignment -= DrawHouses;
    }

    // Calls the drawing of cusps and markers in 3D space
    // This function can be called differently, depending on whether a new system is called,
    // or the cusps are simply being updated to a new date/time

    // When updating data without changing house system
    public void DrawHouses()
    {
        // get house system from active data
        hSys = GeoData.ActiveData.Hsys;

        // draw 3D objects
        DrawHouseCusps(hSys);
        PlaceHouseMarkers(hSys);

        // flag house systems that only need one calculation
        areCampanusHousesCalculated = hSys == 'C' ? true : false;
        areRegiomontanusHousesCalculated = hSys == 'R' ? true : false;
    }

    // When changing house system
    public void DrawHouses(char hSys)
    {
        // draw 3D objects
        DrawHouseCusps(hSys);
        PlaceHouseMarkers(hSys);

        // flag house systems that only need one calculation
        areCampanusHousesCalculated = hSys == 'C' ? true : false;
        areRegiomontanusHousesCalculated = hSys == 'R' ? true : false;
    }

    // Draws house cusp lines in 3D
    // House cusps are projected onto the ecliptic poles by default
    // Current exceptions are: Campanus and Regiomontanus
    void DrawHouseCusps(char hSys)
    {
        // iterate through 6 houses cusps
        // from houses 1-6, the other ones are simply mirrored
        for (int i = 1; i < 7; i++) 
        {
            // fill pool of cusp objects that will be reused
            // name and complete with useful information
            if (houseCusps.Count < i)
            {
                _activeCusp = Instantiate(houseCuspExtender.gameObject, cuspHolder.transform).GetComponent<CuspExtender>();
                _activeCusp.houseId = i;
                _activeCusp.gameObject.name = "House " + i.ToString() + " " + (i + 6).ToString();
                houseCusps.Add(_activeCusp);
            }
            // get existing cusp if list is already filled
            else _activeCusp = houseCusps[i - 1];

            // special calculation for Campanus houses
            if (hSys == 'C')
            {
                if (areCampanusHousesCalculated) break;
                _activeCusp.ExtendCuspCampanus();
            }
            // special calculation for Regiomontanus houses
            else if (hSys == 'R')
            {
                if (areRegiomontanusHousesCalculated) break;
                _activeCusp.ExtendCuspRegiomontanus();
                
            }                
            // standard calculation for other types
            else _activeCusp.ExtendHouseCusp(GeoData.ActiveData.HouseCusps[i]);
        }

        // empty active cusp
        _activeCusp = null;
    }

    // Places house markers in 3D
    // The markers are placed at the midpoint between its cusp and the next
    // Houses are projected onto the ecliptic poles by default
    // Current exceptions are: Campanus and Regiomontanus
    public void PlaceHouseMarkers(char hSys)
    {
        // iterate through 24 houses marker
        // primary markers: 1-12
        // secondary markers: 13-24, mirrored on the other side of the sphere
        for (int i = 1; i < 25; i++)
        {
            // fill pool of cusp objects that will be reused
            // name and complete with useful information
            if (houseNumbers.Count < i)
            {
                _activeHouseMarker = Instantiate(midHouseModel, cuspHolder.transform).GetComponent<Point3D>();
                if (i > 12) _activeHouseMarker.name = "MidHouse " + (i - 12).ToString() + "-2";
                _activeHouseMarker.name = "MidHouse " + i.ToString();
                houseNumbers.Add(_activeHouseMarker);
                NameMarker(_activeHouseMarker, i);
            }
            // get existing marker if already filled
            else _activeHouseMarker = houseNumbers[i - 1];

            // special calculation for Campanus houses
            if (hSys == 'C')
            {
                if (areCampanusHousesCalculated) break;
                PlaceMarkerCampanus(_activeHouseMarker, i);
            }
            // special calculation for Regiomontanus houses
            else if (hSys == 'R')
            {
                if (areRegiomontanusHousesCalculated) break;
                PlaceMarkerRegiomontanus(_activeHouseMarker, i);
            }
            // standard calculation for other types
            else
            {
                CalculateMarker(_activeHouseMarker, i);
            }

            // clear marker
            _activeHouseMarker = null;
        }


        // Places the marker for Campanus houses
        // this function finds the positions of markers,
        // based on a sphere with poles in North/South directions
        // they are placed are regular 30 degree intervals
        void PlaceMarkerCampanus(Point3D houseMarker, int i)
        {
            // define default values
            int defaultRange = 30; // default distance between markers
            int midRange = defaultRange/2; // midpoint between signs
            int increment = -(defaultRange * (i));

            // azimuth rotation is the distance from north direction
            float azimuth = campanusHouseMarkerRange;
            // xRotation is the rotation relative to a north/south axis 
            float xRotation = -midRange - increment;

            // project opposite rotations for the secondary marker
            if (i > 12)
            {
                azimuth += 180;
                xRotation += 180;
            }

            // reset markers
            houseMarker.transform.localEulerAngles = Vector3.zero;
            houseMarker.transform.GetChild(0).position = new Vector3(10000,0,0);

            // rotate for final position
            houseMarker.RotateAzimuth(azimuth);
            houseMarker.RotateWorldX(xRotation);
        }

        // Places the marker for Regiomontanus houses
        // this function finds the positions of markers,
        // based on a sphere with poles in North/South directions
        // the positions are projected from the horizontal coordinates
        // of each house cusp
        void PlaceMarkerRegiomontanus(Point3D houseMarker, int i)
        {
            // normalize to 1 - 12 indexes
            int index = i > 12 ? i - 12 : i;

            // setup calculation variables
            double[] cuspPos = new double[6];
            double[] cuspPosHor = new double[6];
            cuspPos[0] = HouseData.instance.houseDataList[index].midLongitude;

            // get horizontal coordinates of the cusp
            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, cuspPos, cuspPosHor);

            // convert horizontal coordinates to cartesian
            Vector3 cartesianCusp = AstroFunctions.HorizontalToCartesian(cuspPosHor[0], cuspPosHor[1]);

            // convert coordinates to a secondary spherical coordinate (north/south directional poles)
            double[] horizontalSphCoordinates = AstroFunctions.CartesianToHorizonSpherical(cartesianCusp.x, cartesianCusp.y, cartesianCusp.z);
            
            // setup final rotation values
            float azimuth, xRotation;

            // azimuth rotation is the distance from north direction
            azimuth = campanusHouseMarkerRange;
            // xRotation is the rotation relative to a north/south axis 
            xRotation = -(float)horizontalSphCoordinates[1];

            // project opposite rotations for the secondary marker
            if (i > 12)
            {
                azimuth += 180;
                xRotation += 180;
            }

            // reset markers
            houseMarker.transform.localEulerAngles = Vector3.zero;
            houseMarker.transform.GetChild(0).position = new Vector3(10000, 0, 0);

            // rotate for final position
            houseMarker.RotateAzimuth(azimuth);
            houseMarker.RotateWorldX(xRotation);
        }

        // Places marker for all other house systems
        // these are simply placed at the midpoint between two cusps
        // at a certain distance from the ecliptic poles
        void CalculateMarker(Point3D houseMarker, int i)
        {
            // prepare data for calculation using swiss ephemeris
            double[] x2 = new double[6];
            double[] xaz = new double[3];
                
            // normalize secondary markers to 1-12
            // setup altitude
            if (i > 12)
            { 
                i -= 12;
                x2[1] = -houseMarkerRange;
            }
            else
                x2[1] = houseMarkerRange;

            // setup azimuth
            x2[0] = houseData.houseDataList[i].midLongitude;

            // calculate horizontal coordinates
            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, x2, xaz);

            // reset rotation, then rotate to final position
            houseMarker.transform.transform.localEulerAngles = Vector3.zero;
            houseMarker.transform.GetChild(0).position = AstroFunctions.HorizontalToCartesian(xaz[0], xaz[1]);
        }

        // Give the appropriate name to each marker
        void NameMarker(Point3D houseMarker, int i)
        {
            // get text field
            TextMeshPro textField = houseMarker.transform.GetChild(0).GetComponent<TextMeshPro>();
            
            // normalize to 1-12
            if (i > 12) i -= 12;

            // angular houses are given roman numerals
            string houseName = i switch
            {
                1 => "I",
                4 => "IV",
                7 => "VII",
                10 => "X",
                _ => i.ToString(),
            };

            // name markers
            textField.text = houseName;
        }


    }
}
