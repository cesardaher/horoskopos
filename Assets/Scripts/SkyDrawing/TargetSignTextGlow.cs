using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetSignTextGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text textMesh;
    public Material material;
    protected Color color;

    public float defaultGlow;
    public float glowIncrement = 0.15f;

    protected virtual void Start()
    {
        // create an instance of shared material and apply it to object
        material = new Material(textMesh.fontSharedMaterial);
        textMesh.fontMaterial = material;

        // get color from text mesh instance
        color = textMesh.color;

        // set up proper color to material glow
        material.SetColor("_GlowColor", color);
        defaultGlow = material.GetFloat("_GlowPower");
    }

    protected virtual void OnMouseEnter()
    {
        material.SetFloat("_GlowPower", defaultGlow + glowIncrement);
    }
    protected virtual void OnMouseExit()
    {
        material.SetFloat("_GlowPower", defaultGlow);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        material.SetFloat("_GlowPower", defaultGlow + glowIncrement);
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        material.SetFloat("_GlowPower", defaultGlow);
    }
}
