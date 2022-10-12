using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeridianToggler : MonoBehaviour
{
    [SerializeField] GameObject meridian;
    [SerializeField] Angle3D midheaven;

    public void ToggleMeridianVisibility(bool val)
    {
        meridian.SetActive(val);
        midheaven.gameObject.SetActive(val);
    }
}
