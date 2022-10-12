using UnityEngine;

public class PanelToggler : MonoBehaviour
{
    private void Start()
    {
        EventManager.Instance.OnTutorialToggle += FollowTutorial;
    }

    void FollowTutorial(bool val)
    {
        // open when not on tutorial
        if (val) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnTutorialToggle -= FollowTutorial;
    }
}
