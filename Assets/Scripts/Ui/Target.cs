using UnityEngine;
using UnityEngine.EventSystems;
using UnityTemplateProjects;

public class Target : MonoBehaviour, IClickable
{
    public static bool inTutorial;

    Collider coll;
    public PlanetInfoBox infoBox;
    [SerializeField] PlanetData planetData;

    void Start()
    {
        coll = GetComponent<Collider>();
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

    void ToggleInfoBox(bool value, Vector3 pos)
    {
        if (value)
        {
            //set up planet info
            EventManager.Instance.OpenPlanetBox(planetData.planetID, pos);

            return;
        }

        if (!value && infoBox.gameObject.activeSelf)
        {
            // turn off box if on
            infoBox.gameObject.SetActive(false);
            return;
        }
    }

}
