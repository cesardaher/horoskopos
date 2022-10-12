using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMarkerToggler : MonoBehaviour
{
    [SerializeField] MoonOrbitDrawer moonOrbitDrawer;

    public void OutsideStart()
    {
        EventManager.Instance.OnChartMode += ActivateMoonOrbitMarker;
        EventManager.Instance.OnSkyMode += DeactivateMoonOrbitMarker;
        EventManager.Instance.OnPlanetSelect += ActivateMoonOrbitMarker;
        EventManager.Instance.OnPlanetBoxClose += DeactivateMoonOrbitMarker;

        if (!ModesMenu.chartModeOn) DeactivateMoonOrbitMarker();
    }

    void ActivateMoonOrbitMarker() => gameObject.SetActive(true);
    void DeactivateMoonOrbitMarker() => gameObject.SetActive(false);    

    void ActivateMoonOrbitMarker(int id, Vector3 pos)
    {

        if (gameObject.activeSelf) return; // redundancy check
        if (id != 1 && id != 10 && id != 11) return; // irrelevant planets
        if (!ModesMenu.chartModeOn) return; // only chart mode

        gameObject.SetActive(true);
        
    }

    void DeactivateMoonOrbitMarker(int id)
    {
        if (!gameObject.activeSelf) return; // redundancy check
        if (id != 1 && id != 10 && id != 11) return; // irrelevant planets
        if (ModesMenu.chartModeOn) return; // only chart mode

        gameObject.SetActive(false);

    }

    private void OnDestroy()
    {
        EventManager.Instance.OnChartMode -= ActivateMoonOrbitMarker;
        EventManager.Instance.OnSkyMode -= DeactivateMoonOrbitMarker;
        EventManager.Instance.OnPlanetSelect -= ActivateMoonOrbitMarker;
        EventManager.Instance.OnPlanetBoxClose -= DeactivateMoonOrbitMarker;
    }
}
