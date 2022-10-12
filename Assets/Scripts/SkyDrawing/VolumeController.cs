using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeController : MonoBehaviour
{
    [SerializeField] Volume volume;
    Bloom bloom;
    Tonemapping tonemapping;

    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out tonemapping);
    }

    public void ToggleVolumeState(bool val)
    {
        if (val)
        {
            tonemapping.active = true;
            //volume.weight = 1;
            return;
        }

        tonemapping.active = false;
        //volume.weight = 0;
    }
}
