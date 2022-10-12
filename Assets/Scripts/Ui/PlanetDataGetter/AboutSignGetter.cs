public class AboutSignGetter : PlanetDataGetter
{
    string defaultText;
    new private void Awake()
    {
        base.Awake();
        defaultText = textMesh.text;
    }

    protected override void GetSignInfo(int signId, SignInfoBox box)
    {
        base.GetSignInfo(signId, box);
        textMesh.text = string.Format(defaultText, astroIdentity.listOfSigns[signId].name);
        
    }
}
