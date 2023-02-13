using UnityEngine;
using UnityEngine.UI;

public class ClickTracker : MonoBehaviour
{
    [SerializeField] Canvas myCanvas;
    Image image;
    bool clicked = false;

    private void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }

    private void Update()
    {
        GetMousePosition();

        if (Input.GetMouseButtonDown(0))
        {
            image.enabled = true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            image.enabled = false;
        }
        
    }

    void GetMousePosition()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        transform.position = myCanvas.transform.TransformPoint(pos);
    }
}
