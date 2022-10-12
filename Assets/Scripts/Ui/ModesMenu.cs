using UnityEngine;
using UnityEngine.UI;

public class ModesMenu : MonoBehaviour
{
    public static bool chartModeOn;

    [Header("Buttons")]
    [SerializeField] Button skyModeButton;
    [SerializeField] Button chartModeButton;
    [SerializeField] Toggle groundToggler;

    [Header("Controllers")]
    [SerializeField] SkyboxController skyboxController;
    [SerializeField] VolumeController volumeController;
    [SerializeField] AtmosphereToggler atmosphereToggler;


    [Header("Planets and Symbols")]
    [SerializeField] GameObject planet3dHolder;
    [SerializeField] GameObject planetChart3dHolder;
    [SerializeField] GameObject symbolHolder;

    private void Start()
    {
        SkyModeButton();
    }

    public void ChartModeButton()
    {
        ToggleChartMode(true);
        skyModeButton.interactable = true;
        chartModeButton.interactable = false;

        EventManager.Instance.ActivateChartMode();

        if(TutorialManager.isInTutorial && TutorialManager.Instance.viewChartMode)
        {
            EventManager.Instance.AllowContinueTutorial();
            TutorialManager.Instance.viewChartMode = false;
        }
    }

    public void SkyModeButton()
    {
        ToggleChartMode(false);
        skyModeButton.interactable = false;
        chartModeButton.interactable = true;

        EventManager.Instance.ActivateSkyMode();

        if (TutorialManager.isInTutorial && TutorialManager.Instance.viewSky)
        {
            EventManager.Instance.AllowContinueTutorial();
            TutorialManager.Instance.viewSky = false;
        }
    }

    void ToggleChartMode(bool value)
    {
        if (value)
        {
            // set static mode
            chartModeOn = true;

            // planets and symbols
            planet3dHolder.SetActive(false);
            planetChart3dHolder.SetActive(true);
            symbolHolder.SetActive(true);

            // chart mode and PP
            volumeController.ToggleVolumeState(false);
            atmosphereToggler.ToggleChartModeColors(true);

            // atmosphere
            atmosphereToggler.SetSunsetIntensity(false);

            // ground
            atmosphereToggler.SetGround(false);
        }
        else
        {
            // set static mode
            chartModeOn = false;

            // planets and symbols
            planet3dHolder.SetActive(true);
            planetChart3dHolder.SetActive(false);
            symbolHolder.SetActive(false);

            // sky mode and PP
            atmosphereToggler.ToggleChartModeColors(false);
            volumeController.ToggleVolumeState(true);

            // atmosphere
            if (atmosphereToggler.isAtmosphereOn)
                atmosphereToggler.SetSunsetIntensity(true);
            else
                atmosphereToggler.SetSunsetIntensity(false);

            // ground
            if(groundToggler.isOn)
                atmosphereToggler.SetGround(true);
        }
    }
}
