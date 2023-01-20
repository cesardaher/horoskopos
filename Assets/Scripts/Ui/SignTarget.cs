using UnityEngine;
using UnityEngine.EventSystems;

public class SignTarget : MonoBehaviour, IClickable
{
    public static bool inTutorial;

    Collider coll;
    public SignInfoBox infoBox;
    [SerializeField] MidSign sign;

    private void OnEnable()
    {
        infoBox = FindObjectOfType<SignInfoBox>();
        coll = GetComponent<Collider>();
    }

    void ToggleInfoBox(bool value, Vector3 pos)
    {
        if (value)
        {
            //set up planet info
            EventManager.Instance.OpenSignBox(sign.signID, pos);
            return;
        }

        if (!value && infoBox.gameObject.activeSelf)
        {
            // turn off box if on
            infoBox.gameObject.SetActive(false);
            return;
        }
    }

    public void Interact()
    {
        // cancel interaction when something is on top
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // get ray and check for collider
        // open info box
        {
            Vector3 pos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
            {
                if (hitInfo.collider == coll)
                    ToggleInfoBox(true, pos);

            }
        }
    }
}
