using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquatorToggler : MonoBehaviour
{
    [SerializeField] GameObject equatorObject;

    public void ToggleEquatorVisibility(bool val)
    {
        equatorObject.SetActive(val);
    }
}
