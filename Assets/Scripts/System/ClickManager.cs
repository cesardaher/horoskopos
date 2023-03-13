using UnityEngine.EventSystems;
using UnityTemplateProjects;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    /* This class manages the mouse clicks, in order to communicate either camera movement or interaction */

    bool skyClick = false;
    float time;

    private void Update()
    {
        // activates camera click when clicking outside UI elements
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                skyClick = true;
        }

        // when click is held, move camera according to cursor movement
        if (Input.GetMouseButton(0))
        {
            if (skyClick)
            {
                // activate time counter to cancel click interaction
                time += Time.deltaTime;
                EventManager.Instance.HoldClickOnScreen(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }
        }

        // when secondary click is held, move camera
        if (Input.GetMouseButton(1))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                EventManager.Instance.HoldClickOnScreen(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
            
        // activate interaction with objects when clicking, not holding
        if (Input.GetMouseButtonUp(0))
        {
            // prevent clicking when cursor is over UI object
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (time < 0.25)
            {
                // check if cursor is over sky object
                Vector3 pos = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(pos);

                // if object found is clickable, start interaction
                if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
                {
                    var clickable = hitInfo.collider.GetComponent<IClickable>();
                    if (clickable != null)
                    {
                        clickable.Interact(pos);
                    }
                        
                }
            }

            // reset after click/held
            skyClick = false;
            time = 0;
        }
    }
}
