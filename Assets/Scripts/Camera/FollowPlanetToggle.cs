using UnityEngine;

public class FollowPlanetToggle : MonoBehaviour
{

    public void ToggleFollowCamera(bool val)
    {
        EventManager.Instance.ToggleFollowPlanet(val);
    }
}
