using SwissEphNet;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using AstroResources;

public class HousesDrawer : MonoBehaviour
{
    [SerializeField] CuspExtender houseCuspExtender;
    [SerializeField] GameObject cuspHolder;
    [SerializeField] GameObject midHouseModel;
    [SerializeField] HouseData houseData;

    [SerializeField] List<CuspExtender> houseCusps = new List<CuspExtender>();
    [SerializeField] List<Point3D> houseNumbers = new List<Point3D>();

    bool areCampanusHousesCalculated;
    bool areRegiomontanusHousesCalculated;
    char hSys;
    float houseMarkerRange = 80;
    float campanusHouseMarkerRange = 10;

    void Awake()
    {
        EventManager.Instance.OnRecalculationOfGeoData += DrawHouses;
        EventManager.Instance.OnHouseReassignment += DrawHouses;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnRecalculationOfGeoData -= DrawHouses;
        EventManager.Instance.OnHouseReassignment -= DrawHouses;
    }

    // for when updating data without changing house system
    public void DrawHouses()
    {
        hSys = GeoData.ActiveData.Hsys;
        DrawHouseCusps(hSys);
        DrawMidHouses(hSys);

        areCampanusHousesCalculated = hSys == 'C' ? true : false;
        areCampanusHousesCalculated = hSys == 'R' ? true : false;
    }

    // for when changing house system
    public void DrawHouses(char hSys)
    {
        DrawHouseCusps(hSys);
        DrawMidHouses(hSys);

        areCampanusHousesCalculated = hSys == 'C' ? true : false;
        areCampanusHousesCalculated = hSys == 'R' ? true : false;
    }

    void DrawHouseCusps(char hSys)
    {
        for (int i = 1; i < 7; i++) //iterate through 6 houses cusps
        {
            CuspExtender cusp;

            // if list is not filled, fill it with every remaining cusp
            if (houseCusps.Count < i)
            {
                cusp = Instantiate(houseCuspExtender.gameObject, cuspHolder.transform).GetComponent<CuspExtender>();
                cusp.houseId = i;
                cusp.gameObject.name = "House " + i.ToString() + " " + (i + 6).ToString();
                houseCusps.Add(cusp);
            }

            // get existing cusp
            else cusp = houseCusps[i - 1];

            if (hSys == 'C')
            {
                if (areCampanusHousesCalculated) break;
                    cusp.ExtendCuspCampanus();
            }
            else if(hSys == 'R')
            {
                if (areRegiomontanusHousesCalculated) break;
                    cusp.ExtendCuspRegiomontanus();
                
            }                
            else cusp.ExtendHouseCusp(GeoData.ActiveData.HouseCusps[i]);
        }
    }

    public void DrawMidHouses(char hSys)
    {
        for (int i = 1; i < 25; i++) //iterate through 12
        {
            Point3D houseMarker;

            // if list is not filled
            if (houseNumbers.Count < i)
            {
                houseMarker = Instantiate(midHouseModel, cuspHolder.transform).GetComponent<Point3D>();
                if (i > 12) houseMarker.name = "MidHouse " + (i - 12).ToString() + "-2";
                houseMarker.name = "MidHouse " + i.ToString();
                houseNumbers.Add(houseMarker);
                AssignTextField(houseMarker, i);
            }
            // get existing marker
            else
                houseMarker = houseNumbers[i - 1];


            if (hSys == 'C')
            {
                if (areCampanusHousesCalculated) break;
                AssignTextField(houseMarker, i);
                RotateCampanus(houseMarker, i);
            }
            else if (hSys == 'R')
            {
                if (areRegiomontanusHousesCalculated) break;
                AssignTextField(houseMarker, i);
                RotateRegiomontanus(houseMarker, i);
            }
            else
            {
                CalculateHouseAzAlt(houseMarker, i);
                AssignTextField(houseMarker, i);
            }
                
        }

        void RotateCampanus(Point3D houseMarker, int i)
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

        void RotateRegiomontanus(Point3D houseMarker, int i)
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

            // convert coordinates to a secondary spherical coordinate
            double[] horizontalSphCoordinates = AstroFunctions.CartesianToHorizonSpherical(cartesianCusp.x, cartesianCusp.y, cartesianCusp.z);
            
            // setup final rotation values
            float azimuth, xRotation;

            // azimuth rotation is the distance from north direction
            azimuth = campanusHouseMarkerRange;
            // xRotation is the rotation relative to a north/south axis 
            xRotation = -(float)horizontalSphCoordinates[1] * Mathf.Rad2Deg;

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

        void CalculateHouseAzAlt(Point3D houseMarker, int i)
        {

            double[] x2 = new double[6];
            double[] xaz = new double[3];
                
            if (i > 12)
            { 
                i -= 12;
                x2[1] = -houseMarkerRange;
            }
            else
            {
                x2[1] = houseMarkerRange;
            }

            x2[0] = houseData.houseDataList[i].midLongitude;


            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, x2, xaz);

            //reset rotation, then rotate
            houseMarker.transform.transform.localEulerAngles = Vector3.zero;
            houseMarker.transform.GetChild(0).position = AstroFunctions.HorizontalToCartesian(xaz[0], xaz[1]);
        }

        void AssignTextField(Point3D houseMarker, int i)
        {
            TextMeshPro textField = houseMarker.transform.GetChild(0).GetComponent<TextMeshPro>();
            if (i > 12) i -= 12;

            string houseName = i switch
            {
                1 => "I",
                4 => "IV",
                7 => "VII",
                10 => "X",
                _ => i.ToString(),
            };

            textField.text = houseName;
        }


    }
}
