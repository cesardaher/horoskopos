using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardinalsToggler : MonoBehaviour
{
    [SerializeField] GameObject cardinalDirections;

    public void ToggleCardinalDirectionsVisibility(bool val)
    {
        cardinalDirections.SetActive(val);
    }
}
