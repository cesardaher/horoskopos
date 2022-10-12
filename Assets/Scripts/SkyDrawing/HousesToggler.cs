using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousesToggler : MonoBehaviour
{
    [SerializeField] GameObject houseObjectsHolder;

    private void Start()
    {
        ToggleHousesVisibility(false);
    }

    public void ToggleHousesVisibility(bool val)
    {
        houseObjectsHolder.SetActive(val);
    }
}
