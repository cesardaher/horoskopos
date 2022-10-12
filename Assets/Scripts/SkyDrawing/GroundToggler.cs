using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundToggler : MonoBehaviour
{
    [SerializeField] Material material;
    float _groundWidth = 0.002f;
    bool _groundActive;

    private void Start()
    {
        EventManager.Instance.OnChartMode += DectivateGroundOnChartMode;
        EventManager.Instance.OnSkyMode += ActivateGroundOnSkyMode;
        ToggleGround(true);
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnChartMode -= DectivateGroundOnChartMode;
        EventManager.Instance.OnSkyMode -= ActivateGroundOnSkyMode;
    }

    public void ToggleGround(bool val)
    {
        _groundActive = val;

        if (val)
        {

            if (!ModesMenu.chartModeOn) material.SetFloat("_UseGround", 1f);
            material.SetFloat("_LineThickness", _groundWidth);
            return;
        }

        material.SetFloat("_UseGround", 0);
        material.SetFloat("_LineThickness", 0);
    }

    public void ActivateGroundOnSkyMode()
    {
        if (!_groundActive) return;

        material.SetFloat("_UseGround", 1f);
    }

    public void DectivateGroundOnChartMode()
    {
        if (!_groundActive) return;

        material.SetFloat("_UseGround", 0);
    }
}
