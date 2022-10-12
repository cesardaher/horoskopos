using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public string titleScene;
    public string gameScene;

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void ReturnToTitle()
    {
        EventManager.Instance.OpenTitleScreen();

        // reset GeoData
        GeoData.ActiveData = null;

        SceneManager.LoadScene(titleScene);
    }
}
