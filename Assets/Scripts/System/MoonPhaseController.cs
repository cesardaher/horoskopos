using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonPhaseController : MonoBehaviour
{
    [SerializeField] PlanetData moonData;
    [SerializeField] MoonPhaseData moonPhaseData;
    [SerializeField] Material material;
    [SerializeField] SpriteRenderer spriteRenderer;

    void Start()
    {
        //set up material and properties
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;

        ChangePhaseTexture(moonData.planetID);

        EventManager.Instance.OnCalculatedPlanet += ChangePhaseTexture;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnCalculatedPlanet -= ChangePhaseTexture;
    }

    void ChangePhaseTexture(int id)
    {
        if (id != moonData.planetID) return;

        bool IsWaxing;

        if (moonData.PhaseState == 1) IsWaxing = true;
        else if (moonData.PhaseState == -1) IsWaxing = false;
        else
        {
            Debug.LogError("Invalid Phase State");
            return;
        }

        int[] phaseIndex = moonPhaseData.SearchRange(moonData.phase);

        int realIndex = IsWaxing ? phaseIndex[0] : phaseIndex[1];

        spriteRenderer.sprite = moonPhaseData.listOfMoonPhases[realIndex].Sprite;
        material.SetTexture("_EmissionTex", moonPhaseData.listOfMoonPhases[realIndex].Texture);
    }
}
