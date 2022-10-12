using TMPro;
using UnityEngine;

public class CurrentDateConfirmation : MonoBehaviour
{
    public TextMeshProUGUI textArea;
    public TutorialDataManager tutDataManager;

    string ConstructString()
    {
        string confirmation;
        confirmation = string.Format("It is currently {0}:{1} on {2}.{3}.{4} in the city of {5}.",
            GeoData.ActiveData.Ihour.ToString(), //0
            GeoData.ActiveData.Imin.ToString(), //1
            GeoData.ActiveData.Iday.ToString(), //2
            GeoData.ActiveData.Imon.ToString(), //3
            GeoData.ActiveData.Iyear.ToString(), //4
            GeoData.ActiveData.City //5
            );

        return confirmation;
    }

    // Activated through Button
    public void UpdateText()
    {
        textArea.text = ConstructString();
    }
}
