using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using AstroResources;

public class AngleData : PointData
{
    [SerializeField] int angleID;
    static List<AngleData> _angleDataList = new List<AngleData>();
    public static List<AngleData> AngleDataList { 
        get { return _angleDataList; } 
    }

    [SerializeField] Angle3D _angle3D;
    public SpacePoint Angle3D { get; private set; }

    public override double[] X2
    {
        get { return _x2; }
        set
        {
            _x2 = value;
            Longitude = _x2[0];
            Latitude = _x2[1];
        }
    }

    public override double[] Xaz
    {
        get { return _xaz; }
        set { 
            _xaz = value;
            Angle3D.Azimuth = _xaz[0];
            Angle3D.TrAlt = _xaz[1];
            Angle3D.AppAlt = _xaz[2];

            Angle3D.planet.transform.position = AstroFunctions.HorizontalToCartesian(Angle3D.Azimuth, Angle3D.TrAlt);
        }
    }

    private void Awake()
    {
        _angleDataList.Add(this);
        _angleDataList = _angleDataList.OrderBy(x => x.angleID).ToList();
        Angle3D = new SpacePoint(this, _angle3D);
    }

    private void OnDestroy()
    {
        _angleDataList.Remove(this);
    }
}
