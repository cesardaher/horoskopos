using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonBillboard : MonoBehaviour
{
    [SerializeField] SkyRotator skyRotator;
    Camera cam;

    private void Awake()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }
    void Update()
    {
        DirectMoon();
    }

    void DirectMoon()
    {
        transform.LookAt(cam.transform.position, skyRotator.northPolePosition);
        if (transform.localScale.x >= 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
