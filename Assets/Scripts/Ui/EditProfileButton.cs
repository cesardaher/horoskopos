using UnityEngine;

public class EditProfileButton : MonoBehaviour
{
    public GeoDataHandler dataManager;

    public void EditProfile()
    {
        dataManager.OverwriteProfile();

    }
}
