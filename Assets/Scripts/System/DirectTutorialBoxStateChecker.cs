using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectTutorialBoxStateChecker : MonoBehaviour
{

    private void Start()
    {
        if (TutorialManager.isInTutorial) gameObject.SetActive(false);
    }
}
