using UnityEngine;
using System.Collections;

public class GameUiEventManager : MonoBehaviour
{
    /// <summary>
    /// Delegate for the event of closing game menu.
    /// </summary>
    public delegate void GameMenuClosed();

    /// <summary>
    /// Delegate for the event of opening game menu.
    /// </summary>
    public delegate void GameMenuOpened();

    /// <summary>
    /// Event called when the game menu was closed.
    /// </summary>
    public static event GameMenuClosed OnGameMenuClosed;

    /// <summary>
    /// Event called when the game menu has been opened.
    /// </summary>
    public static event GameMenuOpened OnGameMenuOpened;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Trigger the event of game menu being closed.
    /// </summary>
    public static void TriggerGameMenuClosed()
    {
        if (OnGameMenuClosed != null)
        {
            OnGameMenuClosed();
        }
    }

    /// <summary>
    /// Trigger the event of game menu being opened.
    /// </summary>
    public static void TriggerGameMenuOpened()
    {
        if (OnGameMenuOpened != null)
        {
            OnGameMenuOpened();
        }
    }
}
