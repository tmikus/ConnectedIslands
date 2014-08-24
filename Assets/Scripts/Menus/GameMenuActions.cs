using System;
using UnityEngine;
using System.Collections;

public class GameMenuActions : MonoBehaviour
{
    /// <summary>
    /// Called when the script is created.
    /// </summary>
    void Awake()
    {
        Game.IsPaused = true;
        GameUiEventManager.TriggerGameMenuOpened();
    }

    /// <summary>
    /// Called when the script is destroyed.
    /// </summary>
    void OnDestroy()
    {
        GameUiEventManager.TriggerGameMenuClosed();
        Game.IsPaused = false;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Opens the main menu screen.
    /// </summary>
    public void OpenMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }

    /// <summary>
    /// Closes the game menu and resumes the gameplay.
    /// </summary>
    public void ResumeGame()
    {
        Destroy(gameObject);
    }
}
