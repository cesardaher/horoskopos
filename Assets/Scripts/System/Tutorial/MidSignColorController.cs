using System;
using TMPro;
using UnityEngine;
using AstroResources;

public class MidSignColorController : MonoBehaviour
{
    [SerializeField] AstrologicalIdentity astrologicalIdentity;
    [SerializeField] MidSign midSignParent;    
    [SerializeField] TextMeshPro textMesh;
    [SerializeField] Color skyViewColor;
    [SerializeField] Color chartViewColor;

    Color controlColor;

    private void OnEnable()
    {
        controlColor = chartViewColor;
        ToggleGlow(false);

        EventManager.Instance.OnSkyMode += SkyViewColor;
        EventManager.Instance.OnChartMode += ChartViewColor;
        EventManager.Instance.OnZodiacElementSeparation += SetElementColor;
        EventManager.Instance.OnZodiacSeasonSeparation += SetSeasonColor;
        EventManager.Instance.OnZodiacRevertColor += RevertColor;
    }

    void SkyViewColor()
    {
        textMesh.color = skyViewColor;
    }
    void ChartViewColor()
    {
        textMesh.color = chartViewColor;
    }

    void ToggleGlow(bool val)
    {
        
        Color materialColor = textMesh.fontSharedMaterial.GetColor("_GlowColor");
        materialColor.a = 1;
        Color invisColor = new Color(materialColor.r, materialColor.g, materialColor.b, 0);
        
        if(val)
        {
            textMesh.fontSharedMaterial.SetFloat("_GlowPower", 0.5f);
            //textMesh.fontSharedMaterial.SetColor("_GlowColor", materialColor);
            return;
        }
            textMesh.fontSharedMaterial.SetFloat("_GlowPower", 0);
        //textMesh.fontSharedMaterial.SetColor("_GlowColor", invisColor);
    }

    void SetElementColor()
    {
        int elementId = AstrologicalDatabase.signsByElement[midSignParent.signID];
        textMesh.color = astrologicalIdentity.listOfElements[elementId].color;
    }

    void SetSeasonColor()
    {
        int seasonId = AstrologicalDatabase.signsBySeason[midSignParent.signID];
        textMesh.color = astrologicalIdentity.listOfSeasons[seasonId].color;
    }

    void RevertColor()
    {
        if(ModesMenu.chartModeOn)
        {
            textMesh.color = chartViewColor;
            return;
        }

        textMesh.color = skyViewColor;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnSkyMode -= SkyViewColor;
        EventManager.Instance.OnChartMode -= ChartViewColor;
        EventManager.Instance.OnZodiacElementSeparation -= SetElementColor;
        EventManager.Instance.OnZodiacSeasonSeparation -= SetSeasonColor;
        EventManager.Instance.OnZodiacRevertColor -= RevertColor;
    }
}
