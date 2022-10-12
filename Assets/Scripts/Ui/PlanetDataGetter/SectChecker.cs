using UnityEngine;

public class SectChecker : PlanetDataGetter
{
    protected override void GetPlanetInfo(int planetId, PlanetInfoBox box)
    {
        base.GetPlanetInfo(planetId, box);

        // Mercury's special case
        if (planetId == 2)
        {
            if(IsMercuryMorningStar()) // diurnal Mercury
            {
                if (PlanetData.PlanetDataList[0].realPlanet.TrAlt > 0)
                {
                    textMesh.text = "In sect";
                }
                else
                    textMesh.text = "Out of sect";
            }
            else // nocturnal Mercury
            {
                if (PlanetData.PlanetDataList[0].realPlanet.TrAlt > 0)
                {
                    textMesh.text = "Out of sect";
                }
                else
                    textMesh.text = "In sect";
            }

            return;
        }

        if (astroIdentity.listOfPlanets[planetId].sect == "")
        {
            textMesh.text = "--";
            return;
        }

        if(astroIdentity.listOfPlanets[planetId].sect == "Diurnal")
        {
            if(PlanetData.PlanetDataList[0].realPlanet.TrAlt > 0)
            {
                textMesh.text = "In sect";
            }
            else
                textMesh.text = "Out of sect";
        }

        if (astroIdentity.listOfPlanets[planetId].sect == "Nocturnal")
        {
            if (PlanetData.PlanetDataList[0].realPlanet.TrAlt > 0)
            {
                textMesh.text = "Out of sect";
            }
            else
                textMesh.text = "In sect";
        }

        bool IsMercuryMorningStar()
        {
            double mercuryLongitude = PlanetData.PlanetDataList[2].Longitude;
            double sunLongitude = PlanetData.PlanetDataList[0].Longitude;

            if(Get360Distance(mercuryLongitude, sunLongitude) < 0)
            {
                return true;
            }

            return false;

            float Get360Distance(double x, double y)
            {
                double diff = x - y;

                if (Mathf.Abs((float)diff) > 180)
                {
                    diff = diff - 360;
                }
                return (float)diff;
            }
        }

    }
}
