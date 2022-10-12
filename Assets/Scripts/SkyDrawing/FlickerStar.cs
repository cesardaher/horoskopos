using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlickerStar : MonoBehaviour
{
    [SerializeField] bool isFlickering;
    [SerializeField] Material material;
    float glow;
    public float Glow
    {
        get { return glow; }
        set { 
            glow = value;

            StopCoroutine(FlickerTarget());
            StartCoroutine(FlickerTarget());
        }
    }
    [SerializeField][Range(-1, 1)]
    float flickerIncrement;

    // Start is called before the first frame update
    void Start()
    {
        //set up material and properties
        material = GetComponent<SpriteRenderer>().material;
        glow = material.GetFloat("_GlowIntensity");

        if(isFlickering)
        {
            StartCoroutine(FlickerTarget());
        }
    }

    private void OnValidate()
    {
        if (material == null) return;

        StopCoroutine(FlickerTarget());
        StartCoroutine(FlickerTarget());
    }

    IEnumerator FlickerTarget()
    {
        float newGlow;
        while (isFlickering)
        {
            newGlow = Random.Range(glow - flickerIncrement, glow + flickerIncrement);
            material.SetFloat("_GlowIntensity", newGlow);
            yield return new WaitForSeconds(0.1f);
        }

    }
}
