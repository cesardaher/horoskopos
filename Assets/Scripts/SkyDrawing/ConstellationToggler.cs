using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationToggler : MonoBehaviour
{

    public void ToggleConstellations(bool val)
    {
        EventManager.Instance.UseConstellations(val);
    }
}
