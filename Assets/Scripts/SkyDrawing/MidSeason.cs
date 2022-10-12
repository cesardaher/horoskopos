using TMPro;
using UnityEngine;

public class MidSeason : Point3D
{
    [SerializeField] AstrologicalIdentity astrologicalIdentity;
    public string seasonName;
    public int seasonID;
    [SerializeField] TextMeshPro symbolText;

    public void Start()
    {
        // scale x is reversed on transform prefab to make the text look correct
        transform.GetChild(0).LookAt(Camera.main.transform.position, Camera.main.GetComponentInParent<UnityTemplateProjects.CameraController>().eclipticPoles.northPolePosition);
    }

    //assign sprite from spritelist using indices
    public void AssignText(int i)
    {
        symbolText.text = astrologicalIdentity.listOfSeasons[i].name.ToLower();
        symbolText.color = astrologicalIdentity.listOfSeasons[i].color;
    }
}
