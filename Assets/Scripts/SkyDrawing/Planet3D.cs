using UnityEngine;
using UnityEngine.UI;

public class Planet3D : Point3D
{
    [SerializeField] PlanetData parentPlanet;
    public SpriteRenderer spriteRenderer;
    static AstrologicalIdentity astrologicalIdentity;

    private void Start()
    {
        if(astrologicalIdentity == null) astrologicalIdentity = Resources.Load<AstrologicalIdentity>("AstroIdentities");

        // set the Sun's color manually, as the real and chart objects have different colors
        if (spriteRenderer != null && parentPlanet.planetID != 0)
        {
            spriteRenderer.color = astrologicalIdentity.listOfPlanets[parentPlanet.planetID].color;
        }
        
    }
}
