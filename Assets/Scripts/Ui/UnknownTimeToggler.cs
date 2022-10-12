using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnknownTimeToggler : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField] TMP_Dropdown hourInput;
    [SerializeField] TMP_Dropdown minInput;
    [SerializeField] Toggle daylightSavings;
    [SerializeField] Toggle unknownTime;

    private void Start()
    {
        unknownTime.isOn = false;
        TutorialDataManager.isBirthTimeUnknown = false;
    }

    public void ToggleTime(bool val)
    {
        if(!val)
        {
            TutorialDataManager.isBirthTimeUnknown = false;
            hourInput.interactable = true;
            minInput.interactable = true;
            daylightSavings.interactable = true;

            hourInput.captionText.text = hourInput.options[hourInput.value].text;
            minInput.captionText.text = minInput.options[minInput.value].text;

            TutorialDataManager.isUsingDaylightSavings = daylightSavings.isOn;
            return;
        }

        if(val)
        {
            TutorialDataManager.isBirthTimeUnknown = true;
            hourInput.interactable = false;
            minInput.interactable = false;
            daylightSavings.interactable = false;

            hourInput.captionText.text = "";
            minInput.captionText.text = "";

            TutorialDataManager.isUsingDaylightSavings = true;
            return;
        }
    }
}
