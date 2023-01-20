using UnityEngine.EventSystems;
using UnityTemplateProjects;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    bool clicked = false;
    float time;

    [SerializeField] CameraController cameraController;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            time += Time.deltaTime;
            EventManager.Instance.HoldClickOnScreen(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        if(Input.GetMouseButton(1))
            EventManager.Instance.HoldClickOnScreen(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (Input.GetMouseButtonUp(0))
        {
            if (time < 0.25)
            {
                Vector3 pos = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(pos);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
                {
                    var clickable = hitInfo.collider.GetComponent<IClickable>();
                    if (clickable != null) clickable.Interact();
                }
            }

            time = 0;


        }
    }
}
