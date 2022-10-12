using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonLabelBillboard : MonoBehaviour
{
    public Camera cam;
    public EclipticPoles eclipticPoles;

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        eclipticPoles = Camera.main.GetComponentInParent<UnityTemplateProjects.CameraController>().eclipticPoles;
    }

    void Update()
    {
        transform.LookAt(cam.transform.position, cam.transform.up);
    }
}
