using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TargetGlow : MonoBehaviour
{
    public bool isRealPlanet;
    bool isFlickering;
    public Material material;
    public float defaultGlow;
    public float glowIncrement = 0.6f;
    public TargetPlanetTextGlow targetTextGlow;
    [SerializeField] FlickerStar flickerStar;

    private void Start()
    {
        //set up material and properties
        if(GetComponent<MaskableGraphic>() != null)
            material = GetComponent<MaskableGraphic>().material;
        else
            material = GetComponent<SpriteRenderer>().material;

        flickerStar = GetComponent<FlickerStar>();

        // set empty glow if it's a chart dot
        // else get glow from real planet
        if (!isRealPlanet)
        {
            defaultGlow = 0;
            material.SetFloat("_GlowIntensity", defaultGlow);
        }
        else
        {
            defaultGlow = material.GetFloat("_GlowIntensity");
        }
    }

    IEnumerator FlickerTarget()
    {
        Debug.Log("clicked");
        while(isFlickering)
        {
            float glow = Random.Range(0.2f, 0.4f);
            material.SetFloat("_GlowIntensity", glow);
            yield return new WaitForSeconds(0.1f);
        }

    }

    public void OnMouseEnter()
    {
        if (targetTextGlow != null) targetTextGlow.material.SetFloat("_GlowPower", targetTextGlow.defaultGlow + targetTextGlow.glowIncrement);
        if (flickerStar != null) flickerStar.Glow = defaultGlow + glowIncrement;
        material.SetFloat("_GlowIntensity", defaultGlow + glowIncrement);
    }
    public void OnMouseExit()
    {
        if (targetTextGlow != null) targetTextGlow.material.SetFloat("_GlowPower", targetTextGlow.defaultGlow);
        if(flickerStar != null) flickerStar.Glow = defaultGlow;
        material.SetFloat("_GlowIntensity", defaultGlow);
    }
}
