using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropdownFieldTracker : MonoBehaviour
{
    public static bool usingDropdown;
    [SerializeField] List<TMP_Dropdown> _dropdownField;

    private void Start()
    {
        usingDropdown = false;
    }

    private void Update()
    {
        Debug.Log(usingDropdown);

        foreach (var dropdownField in _dropdownField)
        {
            if (dropdownField.IsExpanded)
            {
                usingDropdown = true;
                return;
            }
        }

        usingDropdown = false;


    }
}
