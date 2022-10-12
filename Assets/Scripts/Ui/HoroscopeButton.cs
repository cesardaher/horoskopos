using UnityEngine;
using UnityEngine.UI;

public class HoroscopeButton : MonoBehaviour
{
    public GameObject horoscopeBox;
    [SerializeField] Button button;


    private void Start()
    {
        horoscopeBox.SetActive(false);
    }

    public void OpenHoroscopeBox()
    {
        if (!horoscopeBox.activeSelf)
        {
            horoscopeBox.SetActive(true);
            button.interactable = false;


            horoscopeBox.transform.SetAsLastSibling();
            CheckForTutorialPrompt();
            return;
        }
    }

    public void CheckForTutorialPrompt()
    {
        if (TutorialManager.isInTutorial && TutorialManager.Instance.openChart)
        {
            EventManager.Instance.ContinueTutorial();
            TutorialManager.Instance.openChart = false;
        }
    }


    public void CloseBox()
    {
        button.interactable = true;
    }
}
