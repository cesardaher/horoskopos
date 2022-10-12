using SwissEphNet;
using System.Collections.Generic;
using UnityEngine;
using AstroResources;

public class SeasonDrawer : MonoBehaviour
{
    [SerializeField] GameObject seasonLabelPrefab;
    List<MidSeason> seasonPoints = new List<MidSeason>();

    public void DrawSeasonsLabel()
    {

        for(int i = 0; i < 4; i++)
        {
            MidSeason seasonObj;
            // Create new object or get from pool
            // increment helps work with list, which doesn't include 0
            if (seasonPoints.Count <= i)
                seasonObj = CreateSeasonObj(i + 1);
            else
                seasonObj = seasonPoints[i];

            double longitude = (90 * i) + 45; // mid longitude of fixed signs

            // assign lat lon
            // change direction above ecliptic based on hemisphere
            double[] x2 = { longitude, 0, 0, 0, 0, 0 };
            if (GeoData.ActiveData.NorthernHemisphere) x2[1] = 10;
                else x2[1] = -10;

            double[] xaz = new double[6];

            // calculate horizontal positions
            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, x2, xaz);

            // rotate
            seasonObj.RotateAzimuth(xaz[0]);
            seasonObj.RotateAltitude(xaz[2]);

        }

        MidSeason CreateSeasonObj(int id)
        {
            MidSeason seasonObj;
            seasonObj = Instantiate(seasonLabelPrefab, transform).GetComponent<MidSeason>();
            seasonPoints.Add(seasonObj);
            
            seasonObj.seasonID = id;
            SEASON seasonName = (SEASON) id;
            seasonObj.seasonName = seasonName.ToString();
            seasonObj.AssignText(id);
            
            return seasonObj;
        }
    }

    public void RotateAzimuth(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;

        rotationVector.y += (float)rotation;

        transform.localRotation = Quaternion.Euler(rotationVector);
    }

    //rotates on the Z axis
    public void RotateAltitude(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;
        rotationVector.z += (float)rotation;
        transform.localRotation = Quaternion.Euler(rotationVector);
    }
}

