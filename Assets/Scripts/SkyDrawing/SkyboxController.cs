using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public Material skybox;

    bool useAtmosphere;
    public bool UseAtmosphere
    {
        set {
            if (value)
                skybox.SetFloat("_UseAtmosphere", 1f);
            else
                skybox.SetFloat("_UseAtmosphere", 0f);

            useAtmosphere = value;
        }
        get { return useAtmosphere; }
    }

    bool useConstellations;
    public bool UseConstellations
    {
        set
        {
            if (value)
                skybox.SetFloat("_UseConstellations", 1f);
            else
                skybox.SetFloat("_UseConstellations", 0f);

            useConstellations = value;
        }
        get { return useConstellations; }
    }

    public void ToggleGround(bool val)
    {
        if (val)
        {
            skybox.SetFloat("_UseGround", 1f);
            return;
        }

        skybox.SetFloat("_UseGround", 0);
    }
    private void Awake()
    {
        EventManager.Instance.OnUseConstellations += ToggleConstellation;
        ToggleConstellation(false);
    }

    private void Start()
    {

    }

    private void OnDestroy()
    {
        EventManager.Instance.OnUseConstellations -= ToggleConstellation;
    }


    void ToggleConstellation(bool val)
    {
        UseConstellations = val;
    }
}
