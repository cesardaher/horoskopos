using UnityEngine;

public class UpdateButton : MonoBehaviour
{
    public GeoDataHandler dataManager;

    public void UpdateData()
    {
        dataManager.UpdateGeoData();
    }
}
