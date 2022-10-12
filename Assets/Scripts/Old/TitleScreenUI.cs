using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenUI : MonoBehaviour
{
    public GameObject dataCreator;
    public GameObject rightNowCreator;
    public Button startGameButton;
    public List<GeoData> geoList = new List<GeoData>();

    private void Awake()
    {
        LoadGeoData();
        dataCreator.SetActive(false);
        rightNowCreator.SetActive(false);
    }

    void LoadGeoData()
    {
        //geoList = Resources.LoadAll<GeoData>("GeoData");
        geoList = SaveManager.LoadFiles();
    }

    public void EnableNewData()
    {
        // enable data creator panel
        dataCreator.SetActive(true);
        //disable right now
        rightNowCreator.SetActive(false);
        // disable start
        startGameButton.interactable = false;

    }

    public void EnableRightNow()
    {
        // enable right now input
        rightNowCreator.SetActive(true);
        //disable data creation
        dataCreator.SetActive(false);
        // enable start
        startGameButton.interactable = true;
    }

    public void DisableNewData()
    {
        dataCreator.SetActive(false);
        rightNowCreator.SetActive(false);
        startGameButton.interactable = true;
    }

    public void CreateNewGeoData()
    {
        // TODO: ADD STUFF FOR NEW GEODATA
        // TODO: select new data on dropdown

        DisableNewData();
    }
}
