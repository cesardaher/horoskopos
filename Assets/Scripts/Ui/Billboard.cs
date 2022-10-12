using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera cam;
    bool followTarget;

    private void Awake()
    {
        EventManager.Instance.OnFollowPlanet += ToggleFollowPlanet;

        if(cam == null)
        {
            cam = Camera.main;
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnFollowPlanet -= ToggleFollowPlanet;
    }

    void Update()
    {
        if(followTarget)
        {
            transform.LookAt(cam.transform.position, cam.transform.up);
            return;
        }

        transform.forward = cam.transform.forward;
        transform.forward = new Vector3(cam.transform.forward.x, transform.forward.y, cam.transform.forward.z);
    }

    void ToggleFollowPlanet(bool val)
    {
        followTarget = val;
        
        if(val)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
            return;
        }

        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}
