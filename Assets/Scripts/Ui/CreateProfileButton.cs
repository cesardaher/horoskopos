using UnityEngine;

public class CreateProfileButton : MonoBehaviour
{
    public GeoDataHandler dataManager;

    public void CreateProfile()
    {
        dataManager.CreateNewData();
    }
}
