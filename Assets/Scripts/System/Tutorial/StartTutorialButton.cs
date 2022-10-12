using UnityEngine;

public class StartTutorialButton : MonoBehaviour
{
    public void StartTutorial()
    {
        TutorialManager.isInTutorial = true;
    }
}
