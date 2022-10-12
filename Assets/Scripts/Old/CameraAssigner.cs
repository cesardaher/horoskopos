using UnityEngine;

public class CameraAssigner : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    private void Start()
    {
        canvas.worldCamera = Camera.main;
    }
}
