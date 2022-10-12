using UnityEngine;

public class CurrentTimeButton : MonoBehaviour
{
    public GeoDataHandler dataManager;
    public void GetCurrentTime()
    {
        dataManager.GetCurrentTime();
    }
}
