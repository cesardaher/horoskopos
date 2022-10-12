public class NorthNodeData : PlanetData
{
    public PlanetData southNodeData;

    public override bool IsActive
    {
        get { return _isActive; }
        set
        {
            base.IsActive = value;
            southNodeData.IsActive = value;
        }
    }
}
