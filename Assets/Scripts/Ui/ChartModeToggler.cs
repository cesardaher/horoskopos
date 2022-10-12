using UnityEngine;

public class ChartModeToggler : MonoBehaviour
{
    [SerializeField] GameObject planet3dHolder;
    [SerializeField] GameObject planetChart3dHolder;
    [SerializeField] GameObject symbolHolder;


    public void ToggleChartMode(bool value)
    {
        if (value)
        {
            planet3dHolder.SetActive(false);
            planetChart3dHolder.SetActive(true);
            symbolHolder.SetActive(true);
        }
        else
        {
            planet3dHolder.SetActive(true);
            planetChart3dHolder.SetActive(false);
            symbolHolder.SetActive(false);
        }
    }

}
