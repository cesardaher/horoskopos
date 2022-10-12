using UnityEngine;

public class ClampLabel : MonoBehaviour
{
    public GameObject target;
    public Canvas canvas;

    // Update is called once per frame
    void Update()
    {
        // Offset position above object bbox (in world space)
        float offsetPosY = target.transform.position.y + 1000f;

        // Final position of marker above GO in world space
        Vector3 offsetPos = new Vector3(target.transform.position.x, offsetPosY, target.transform.position.z);

        // Calculate *screen* position (note, not a canvas/recttransform position)
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, null, out Vector2 canvasPos);

        // Set
        transform.localPosition = canvasPos;

    }
}
