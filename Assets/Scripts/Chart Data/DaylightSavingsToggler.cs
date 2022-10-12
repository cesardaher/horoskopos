using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DaylightSavingsToggler : MonoBehaviour
{
    [SerializeField] Toggle unknownTime;

    public void ToggleDLS(bool val)
    {
        unknownTime.isOn = val;
        TutorialDataManager.isUsingDaylightSavings = true;
    }
}
