using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EclipticToggler : MonoBehaviour
{
    [SerializeField] LineRenderer[] eclipticRenderers;
    [SerializeField] GameObject eclipticHolder;

    public void ToggleEcliptic(bool val)
    {
        eclipticHolder.SetActive(val);
    }
}
