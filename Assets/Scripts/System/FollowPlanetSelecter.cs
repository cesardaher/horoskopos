using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowPlanetSelecter : MonoBehaviour
{
    public int PlanetId { get; set; }

    [SerializeField] Toggle toggle;

    public void SelectPlanet(bool val)
    {
        if(val) EventManager.Instance.SelectPlanetToFollow(PlanetId);
    }
}
