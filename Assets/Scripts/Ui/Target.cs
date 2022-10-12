using UnityEngine;
using UnityEngine.EventSystems;
using UnityTemplateProjects;

public class Target : MonoBehaviour
{
    public static bool inTutorial;

    Collider coll;
    public PlanetInfoBox infoBox;
    [SerializeField] PlanetData planetData;

    void Start()
    {
        EventManager.Instance.OnTutorialToggle += ToggleTutorialNonStatic;
        coll = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
                    {
                        //cancel interaction on tutorial
                        if (inTutorial)
                        {
                            FindSunOnTutorial();
                            return;
                        }

                        ToggleInfoBox(true, pos);
                        TargetWithCamera();

                    }

                }
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

    void TargetWithCamera()
    {

    }

    void ToggleTutorialNonStatic(bool val)
    {
        if (inTutorial != val) ToggleTutorial(val);
    }

    static void ToggleTutorial(bool val)
    {
        inTutorial = val;
    }

    void FindSunOnTutorial()
    {
        if(TutorialManager.Instance.findSun && planetData.planetID == 0)
        {
            EventManager.Instance.AllowContinueTutorial();
            TutorialManager.Instance.findSun = false;
        }
    }

    void FindMoonOnTutorial()
    {
        if (TutorialManager.Instance.findMoon && planetData.planetID == 1)
        {
            EventManager.Instance.AllowContinueTutorial();
            TutorialManager.Instance.findMoon = false;
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnTutorialToggle -= ToggleTutorialNonStatic;
    }
}
