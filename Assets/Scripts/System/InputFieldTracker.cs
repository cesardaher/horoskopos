using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputFieldTracker : MonoBehaviour
{
    public static bool usingInput;
    [SerializeField] List<TMP_InputField> _inputFields;
    [SerializeField] AutoCompleteDropdownHandler _autoComplete;

    private void Start()
    {
        usingInput = false;
    }

    private void Update()
    {
        foreach(var inputField in _inputFields)
        {
            if(inputField.isFocused)
            {
                usingInput = true;
                return;
            }
        }

        if(_autoComplete.IsDropdownActive)
        {
            usingInput = true;
            return;
        }

        usingInput = false;
    }
}
