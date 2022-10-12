using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool isPaused;

    [SerializeField] GameObject _menu;

    private void Start()
    {
        _menu.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (!isPaused) PauseApplication();
            else PlayApplication();
        }
    }

    void PauseApplication()
    {
        _menu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void PlayApplication()
    {
        _menu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }
}
