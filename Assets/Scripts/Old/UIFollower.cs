using TMPro;
using UnityEngine;

public class UIFollower : MonoBehaviour
{
    public TextMeshProUGUI ugui;
    [SerializeField] public Transform lookAt;
    [SerializeField] public Vector3 offset;

    [Header("Logic")] public Camera cam;

    void Start()
    {
        cam = Camera.main;
        ugui = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // follow object's pos
        Vector3 pos = cam.WorldToScreenPoint(lookAt.position + offset);

        // only move object when needed
        if(transform.position != pos)
        {
            // if object is in the correct direction
            if(pos.z >= 0)
            {
                // enable ugui if off
                if (!ugui.enabled) ugui.enabled = true;

                pos.z = 0;
                transform.position = pos;
                return;
            }                

            // if not, enable
            if(pos.z < 0)
            {
                ugui.enabled = false;
            }

        }
            
        
    }
}
