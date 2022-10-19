using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedSignal : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _speedText;
    [SerializeField] string pausedText;
    [SerializeField] string speedText;

    void Start()
    {
        EventManager.Instance.OnAnimationStart += SetSpeedText;
        EventManager.Instance.OnAnimationEnd += SetPauseText;

        SetPauseText();
    }


    void SetPauseText()
    {
        _speedText.text = pausedText;   
    }

    void SetSpeedText()
    {
        _speedText.text = ConvertSpeedString();
    }

    string ConvertSpeedString()
    {
        int formatSpeed = Mathf.Abs(TimeManager.Instance.Speed)/1000;
        string finalSpeed = formatSpeed.ToString() + speedText;

        if (TimeManager.Instance.Speed < 0) finalSpeed = "-" + finalSpeed;
        return finalSpeed;
    }

    string ConvertSpeedPerSec()
    {
        int formatSpeed = Mathf.Abs(TimeManager.Instance.Speed) / 1000;
        string finalSpeed = "";

        if(formatSpeed < 60)
        {
            finalSpeed = formatSpeed.ToString() + "s /s";
        }
        return finalSpeed;
    }

}
