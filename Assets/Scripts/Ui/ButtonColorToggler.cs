using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorToggler : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] List<MaskableGraphic> _maskableObjects = new List<MaskableGraphic>();

    float _opacity = 0.3f;
    bool _buttonInteractable;

    private void Start()
    {
        _buttonInteractable = _button.interactable;
        ToggleColor(_buttonInteractable);
    }

    private void Update()
    {
        if(_buttonInteractable != _button.interactable)
        {
            
            _buttonInteractable = _button.interactable;
            ToggleColor(_buttonInteractable);
        }

    }

    void ToggleColor(bool val)
    {
        if(val)
        {
            foreach (MaskableGraphic graphic in _maskableObjects)
            {
                var color = graphic.color;
                color.a = _opacity;
                graphic.color = color;
            }

            return;
        }


        foreach (MaskableGraphic graphic in _maskableObjects)
        {
            var color = graphic.color;
            color.a = 1;
            graphic.color = color;
        }
    }
}
