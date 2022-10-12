using UnityEngine;

public class SignDescriptionGetter : PlanetDataGetter
{
    [SerializeField] BoxDescriptionText boxDescriptionText;

    protected override void GetSignInfo(int signId, SignInfoBox box)
    {
        base.GetSignInfo(signId, box);

        textMesh.text = boxDescriptionText.signInformationTexts[signId].description.Replace("\r", "");
    }
}
