using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetToggle : MonoBehaviour
{
    public int PlanetId { get; set; }


    public void SelectPlanet(bool val)
    {
        if(val) EventManager.Instance.SelectPlanetToFollow(PlanetId);
    }
}
