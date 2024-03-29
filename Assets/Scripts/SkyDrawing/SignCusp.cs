﻿using System.Collections.Generic;
using UnityEngine;
using AstroResources;

public class SignCusp : Point3D
{
    public string signName;
    public int signNumber;
    public static List<SignCusp> eclipticPlanets = new List<SignCusp>();
    [SerializeField] double azimuth;
    public double trAlt { get; set; }
    [SerializeField] double appAlt;
    public CuspExtender cuspExtender;

    public double Azimuth
    {
        get { return azimuth; }
        set
        {
            azimuth = value;
            RotateAzimuth(azimuth);

        }
    }

    public double AppAlt
    {
        get { return appAlt; }
        set
        {
            appAlt = value;
            RotateAltitude(appAlt);
            transform.GetChild(0).position = AstroFunctions.HorizontalToCartesian(azimuth, appAlt);

        }
    }

    public void Start()
    {
        eclipticPlanets.Add(this);

    }
}
