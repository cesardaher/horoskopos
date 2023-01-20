using UnityEngine.EventSystems;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    bool clicked = false;
    float time;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            clicked = true;

        if (clicked)
            time += Time.deltaTime;

        if (Input.GetMouseButtonUp(0))
        {
            if (!clicked) return;

            clicked = false;

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
