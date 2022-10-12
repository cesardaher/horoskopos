using UnityEngine;

public class TutorialContinueButton : MonoBehaviour
{
    public void ContinueTutorial()
    {
         EventManager.Instance.ContinueTutorial();
    }

    public void BacktrackTutorial()
    {
        EventManager.Instance.BacktrackTutorial();
    }
}
