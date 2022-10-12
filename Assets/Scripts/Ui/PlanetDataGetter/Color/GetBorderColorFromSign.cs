using UnityEngine.UI;

public class GetBorderColorFromSign : GetColor
{
    Outline _outline;

    new private void Awake()
    {
        base.Awake();
        _outline = GetComponent<Outline>();
    }

    protected override void GetGraphicColor(int signId, SignInfoBox box)
    {
        base.GetGraphicColor(signId, box);
        _outline.effectColor = _astroIdentities.listOfSigns[signId].secondaryColor;
    }
}
