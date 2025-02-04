using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else Debug.LogWarning("More than one EventManager. Delete this.");
    }

    // MODES

    public event Action OnChartMode;
    public void ActivateChartMode()
    {
        OnChartMode?.Invoke();
    }

    public event Action OnSkyMode;
    public void ActivateSkyMode()
    {
        OnSkyMode?.Invoke();
    }

    // TUTORIAL 

    public event Action<bool> OnTutorialToggle;
    public void ToggleTutorial(bool val)
    {
        OnTutorialToggle?.Invoke(val);
    }

    public event Action OnTutorialContinue;
    public void ContinueTutorial()
    {
        OnTutorialContinue?.Invoke();
    }

    public event Action OnTutorialBacktrack;
    public void BacktrackTutorial()
    {
        OnTutorialBacktrack?.Invoke();
    }

    public event Action OnTutorialAllowContinue;
    public void AllowContinueTutorial()
    {
        OnTutorialAllowContinue?.Invoke();
    }

    // INFORMATION BOXES

    public event Action<PlanetInfoBox> OnPlanetInfoBoxInitialization;
    public void ConnectWithPlanetInfoBox(PlanetInfoBox infoBox)
    {
        OnPlanetInfoBoxInitialization?.Invoke(infoBox);
    }

    public event Action<int, Vector3> OnPlanetSelect;
    public void OpenPlanetBox(int id, Vector3 pos)
    {
        OnPlanetSelect?.Invoke(id, pos);

    }

    public event Action<int, Vector3> OnSignSelect;
    public void OpenSignBox(int id, Vector3 pos)
    {
        OnSignSelect?.Invoke(id, pos);
        
    }

    public event Action<int, PlanetInfoBox> OnPlanetBoxSelect;
    public void ApplyPlanetIdentity(int id, PlanetInfoBox infoBox)
    {
        OnPlanetBoxSelect?.Invoke(id, infoBox);
    }

    public event Action<int> OnPlanetBoxClose;
    public void ClosePlanetBox(int boxID)
    {
            OnPlanetBoxClose?.Invoke(boxID);
    }

    public event Action<int, SignInfoBox> OnSignBoxSelect;
    public void ApplySignIdentity(int id, SignInfoBox box)
    {
            OnSignBoxSelect?.Invoke(id, box);
    }

    public event Action<int> OnSignBoxClose;
    public void CloseSignBox(int boxID)
    {
        if (OnSignBoxClose != null)
            OnSignBoxClose?.Invoke(boxID);
    }

    public event Action OnMultiplePlanetsToggle;
    public void ToggleMultiplePlanets()
    {
        OnMultiplePlanetsToggle?.Invoke();
    }

    public event Action<char> OnHouseReassignment;
    public void RecalculateHouses(char hSys)
    {
        OnHouseReassignment?.Invoke(hSys);
    }

    // CHART CALCULATIONS

    public event Action OnRecalculationOfGeoData;
    public void GeoDataRecalculateEvent()
    {
        OnRecalculationOfGeoData?.Invoke();
    }

    public event Action<int> OnRotatedPlanet;
    public void ApplyPlanetPosition(int id)
    {
        OnRotatedPlanet?.Invoke(id);
    }

    public event Action<int> OnCalculatedPlanet;
    public void ApplyPlanetInfo(int id)
    {
        OnCalculatedPlanet?.Invoke(id);
    }

    // 2D CHART INTERACTIONS

    public event Action<int> On2DPlanetClicked;
    public void InteractWith2DPlanet(int id)
    {
        On2DPlanetClicked?.Invoke(id);

    }

    public event Action<int> On2DSignClicked;
    public void InteractWith2DSign(int id)
    {
        On2DSignClicked?.Invoke(id);

    }

    // SKYBOX

    public event Action<bool> OnUseConstellations;
    public void UseConstellations(bool val)
    {
        OnUseConstellations?.Invoke(val);
    }

    // ECLIPTIC FUNCTIONS

    public event Action<bool> OnToggleEclipticGlow;
    public void ToggleEclipticGlow(bool val)
    {
        OnToggleEclipticGlow?.Invoke(val);

    }

    public event Action<bool> OnToggleSignSymbolsGlow;
    public void ToggleSignSymbolsGlow(bool val)
    {
        OnToggleSignSymbolsGlow?.Invoke(val);
    }

    public event Action OnZodiacSeasonSeparation;
    public void DivideZodiacBySeason()
    {
        OnZodiacSeasonSeparation?.Invoke();
    }

    public event Action OnZodiacElementSeparation;
    public void DivideZodiacByElement()
    {
        OnZodiacElementSeparation?.Invoke();
    }

    public event Action OnZodiacRevertColor;
    public void RevertZodiacColor()
    {
        OnZodiacRevertColor?.Invoke();
    }

    // ANIMATION

    public event Action<int> OnSelectFollowedPlanet;

    public void SelectPlanetToFollow(int val)
    {
        OnSelectFollowedPlanet?.Invoke(val);
    }
    
    public event Action<bool> OnFollowPlanet;
    public void ToggleFollowPlanet(bool val)
    {
        OnFollowPlanet?.Invoke(val);
    }

    public event Action OnAnimationStart;
    public void StartAnimation()
    {
        OnAnimationStart?.Invoke();
    }

    public event Action OnAnimationEnd;
    public void StopAnimation()
    {
        OnAnimationEnd?.Invoke();
    }

    // OPEN / LOAD SCENES

    public event Action OnTitleScreenReturn;
    public void OpenTitleScreen()
    {
        OnTitleScreenReturn?.Invoke();
    }
}
