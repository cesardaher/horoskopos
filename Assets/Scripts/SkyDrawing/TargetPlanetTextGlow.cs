using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class TargetPlanetTextGlow : TargetSignTextGlow
{
    [SerializeField] TargetGlow planetMarker;

    protected override void Start()
    {
        base.Start();

        // connect with marker
        if (planetMarker != null)
        {
            planetMarker.targetTextGlow = this;
        }
    }
    

    protected override void OnMouseEnter()
    {
        // prevent interaction when over UI
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (planetMarker != null) planetMarker.material.SetFloat("_GlowIntensity", planetMarker.defaultGlow + planetMarker.glowIncrement);
        base.OnMouseEnter();
    }
    protected override void OnMouseExit()
    {
        if (planetMarker != null) planetMarker.material.SetFloat("_GlowIntensity", planetMarker.defaultGlow);
        base.OnMouseExit();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        // prevent interaction when over UI
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (planetMarker != null) planetMarker.material.SetFloat("_GlowIntensity", planetMarker.defaultGlow + planetMarker.glowIncrement);
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (planetMarker != null) planetMarker.material.SetFloat("_GlowIntensity", planetMarker.defaultGlow);
        base.OnPointerExit(eventData);
    }
}
