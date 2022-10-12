using UnityEngine.UI;

public class GetColorFromSign : GetColor
{
    MaskableGraphic _graphic;

    new private void Awake()
    {
        base.Awake();
        _graphic = GetComponent<MaskableGraphic>();
    }

    protected override void GetGraphicColor(int signId, SignInfoBox box)
    {
        base.GetGraphicColor(signId, box);
        _graphic.color = _astroIdentities.listOfSigns[signId].secondaryColor;
    }
}
