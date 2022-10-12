using System;
using AstroResources;

public class ElementGetter : PlanetDataGetter
{
    protected override void GetSignInfo(int signId, SignInfoBox box)
    {
        base.GetSignInfo(signId, box);
        int elementId = AstrologicalDatabase.signsByElement[signId];
        textMesh.text = AstrologicalDatabase.elementID[elementId];
    }
}
