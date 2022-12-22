using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstroResources;

public abstract class PointData : MonoBehaviour
{
    [SerializeField] string _name;
    [SerializeField] protected double[] _x2 = new double[6];
    [SerializeField] protected double[] _xaz = new double[6];
    [SerializeField] protected string _sign;
    [SerializeField] protected int _signID;
    [SerializeField] protected double _longitude;
    [SerializeField] protected double _latitude;
    [SerializeField] protected double _degrees;
    [SerializeField] protected int _house;

    [SerializeField] protected int[] _longMinSec = new int[3];
    [SerializeField] protected int[] _latMinSec = new int[3];

    public string Sign
    {
        get { return _sign; }
        private set { _sign = value; }
    }

    public int SignID
    {
        get { return _signID; }
        private set { _signID = value; }
    }

    public double Longitude
    {
        get { return _longitude; }
        set
        {
            _longitude = value;

            //rotate planet as soon as assigned
            AssignSignData(_longitude);
            House = FindHouse(_longitude);
            AstroFunctions.DecimalToMinSec(_longitude, out _longMinSec);

        }
    }

    public double Latitude
    {
        get { return _latitude; }
        set
        {
            _latitude = value;

            AstroFunctions.DecimalToMinSec(_latitude, out _latMinSec);
        }
    }

    public int House
    {
        get { return _house; }
        private set { _house = value; }
    }

    public int[] LongMinSec
    {
        get { return _longMinSec; }
        private set { _longMinSec = value; }
    }

    public int[] LatMinSec
    {
        get { return _latMinSec; }
        private set { _latMinSec = value; }
    }

    public virtual double[] X2
    {
        get { return _x2; }
        set
        {
            _x2 = value;
            Longitude = _x2[0];
            Latitude = _x2[1];
        }
    }

    public virtual double[] Xaz 
    {   
        get { return _xaz; }
        set { _xaz = value; }
    }

    void AssignSignData(double value)
    {
        // assign sign, ID and degrees
        SignID = (int)Math.Abs(value / 30) + 1;
        Sign = ((SIGN)SignID).ToString();
        _degrees = Math.Abs(value % 30);
    }

    int FindHouse(double lon)
    {
        // iterate through all the house cusps
        for (int i = 1; i < GeoData.ActiveData.HouseCusps.Length; i++)
        {
            // prepare house cusps
            double currentHouseCusp = GeoData.ActiveData.HouseCusps[i];
            double nextHouseCusp;

            // prevent index overflow
            if (i == 12)
                nextHouseCusp = GeoData.ActiveData.HouseCusps[1];
            else nextHouseCusp = GeoData.ActiveData.HouseCusps[i + 1];


            // correct overflow in cyclic 360° values
            if (nextHouseCusp < currentHouseCusp)
            {
                // if lon is closer to the end of the zodiac, get 360 values
                if (lon >= 180)
                {
                    nextHouseCusp += 360;
                }
                else // if lon is closer to the beginning of the zodiac, get 0 values
                {
                    currentHouseCusp -= 360;
                }

            }

            if (lon >= currentHouseCusp && lon < nextHouseCusp)
            {
                return i;
            }
        }

        return 0;
    }
}

[System.Serializable]
public class SpacePoint
{
    readonly PointData parent;
    [SerializeField] public Point3D planet;
    [SerializeField] double azimuth;
    [SerializeField] double trAlt;
    [SerializeField] double appAlt;

    public double Azimuth
    {
        get { return azimuth; }
        set
        {
            // REVERT
            azimuth = value;
            //planet.RotateAzimuth(azimuth);
        }
    }

    public double TrAlt
    {
        get { return trAlt; }
        set
        {
            trAlt = value;
            //planet.RotateAltitude(trAlt);
        }
    }

    public double AppAlt
    {
        get { return appAlt; }
        set
        {
            appAlt = value;

            if (planet.gameObject.activeSelf)
            {
                // REVERT
                //planet.RotateAltitude(appAlt);
            }
                

        }
    }

    public SpacePoint(PointData parent, Point3D planet)
    {
        this.parent = parent;
        this.planet = planet;
    }
}
