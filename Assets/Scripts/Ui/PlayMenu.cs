using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button backButton;
    [SerializeField] Button accelerateButton;
    [SerializeField] Button deccelerateButton;
    [SerializeField] Button pauseButton;

    private void Start()
    {
        StopState();
    }

    public void PlayState()
    {
        playButton.interactable = false;

        backButton.interactable = true;
        accelerateButton.interactable = true;
        deccelerateButton.interactable = true;
        pauseButton.interactable = true;
    }

    public void StopState()
    {
        pauseButton.interactable = false;

        playButton.interactable = true;
        backButton.interactable = true;
        accelerateButton.interactable = false;
        deccelerateButton.interactable = false;
    }

    public void BackState()
    {
        backButton.interactable = false;

        pauseButton.interactable = true;
        playButton.interactable = true;
        accelerateButton.interactable = true;
        deccelerateButton.interactable = true;
    }
}
