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

    }

    // for when changing house system
    public void DrawHouses(char hSys)
    {
        DrawHouseCusps(hSys);
        DrawMidHouses(hSys);

        areCampanusHousesCalculated = hSys == 'C' ? true : false;
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
            else
            {
                CalculateHouseAzAlt(houseMarker, i);
                AssignTextField(houseMarker, i);
            }
                
        }

        void RotateCampanus(Point3D houseMarker, int i)
        {
            int signRange = 30;
            int midSignRange = signRange/2;
            int signIncrement = -(signRange * (i));

            // place at correct distance from pole
            float azimuth = campanusHouseMarkerRange;
            float xRotation = -midSignRange - signIncrement;
            
            if (i > 12)
            {
                azimuth += 180;
                xRotation += 180;
            }

            houseMarker.transform.localEulerAngles = Vector3.zero;
            houseMarker.transform.GetChild(0).position = new Vector3(10000,0,0);
            houseMarker.RotateAzimuth(azimuth);
            houseMarker.RotateWorldX(xRotation);
        }

        void RotateRegiomontanus(Point3D houseMarker, int i)
        {

            double[] cuspPos = new double[6];
            cuspPos[0] = HouseData.instance.houseDataList[i].midLongitude;
            double[] cuspPosHor = new double[6];

            // HORIZONTAL COORDINATE FROM ECLIPTIC CUSP
            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, cuspPos, cuspPosHor);

            // CUSP TO CARTESIAN 
            Vector3 cartesianCusp = AstroFunctions.HorizontalToCartesian(cuspPosHor[0], cuspPosHor[1]);

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
