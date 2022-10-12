using AstroResources;

public class ModalityGetter : PlanetDataGetter
{
    protected override void GetSignInfo(int signId, SignInfoBox box)
    {
        base.GetSignInfo(signId, box);
        int modalityId = AstrologicalDatabase.signsByModality[signId];
        textMesh.text = AstrologicalDatabase.modalityID[modalityId];
    }
}
