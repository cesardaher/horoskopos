using UnityEngine;

public class LightCubemap : MonoBehaviour
{
    public Material skybox;
    string direction = "_SunDirection";

    private void Awake()
    {
        // connect to event manager
        EventManager.Instance.OnRotatedPlanet += UpdateSunPos;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnRotatedPlanet -= UpdateSunPos;
    }

    // calculates Sun Position for id 0
    void UpdateSunPos(int id)
    {
        if (id != 0) return;
        skybox.SetVector(direction, transform.position.normalized);
        
    }

}
