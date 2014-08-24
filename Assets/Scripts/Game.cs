using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Game
{
    /// <summary>
    /// Is game paused?
    /// </summary>
    private static bool m_isPaused;

    /// <summary>
    /// Time scale from before pausing.
    /// </summary>
    private static float m_originalTimeScale;

    /// <summary>
    /// Delegate for game paused event.
    /// </summary>
    public delegate void GamePaused();

    /// <summary>
    /// Delegate for game resumed event.
    /// </summary>
    public delegate void GameResumed();

    /// <summary>
    /// Delegate for game started event.
    /// </summary>
    public delegate void GameStarted();

    /// <summary>
    /// Gets or sets if game is paused.
    /// </summary>
    public static bool IsPaused
    {
        get { return m_isPaused; }
        set
        {
            if (m_isPaused == value)
                return;

            m_isPaused = value;

            if (value)
            {
                TriggerGamePaused();
            }
            else
            {
                TriggerGameResumed();
            }
        }
    }

    /// <summary>
    /// Gets or sets if the game is running.
    /// </summary>
    public static bool IsRunning
    {
        get { return !IsPaused; }
        set { IsPaused = !value; }
    }

    /// <summary>
    /// Called when the game was paused.
    /// </summary>
    public static event GamePaused OnGamePaused;

    /// <summary>
    /// Called when the game was resumed.
    /// </summary>
    public static event GameResumed OnGameResumed;

    /// <summary>
    /// Called when the new game was started.
    /// </summary>
    public static event GameStarted OnGameStarted;

    /// <summary>
    /// Triggers the game paused event.
    /// </summary>
    private static void TriggerGamePaused()
    {
        m_originalTimeScale = Time.timeScale;
        Time.timeScale = 0;

        if (OnGamePaused != null)
        {
            OnGamePaused();
        }
    }

    /// <summary>
    /// Triggers the game resumed event.
    /// </summary>
    private static void TriggerGameResumed()
    {
        Time.timeScale = m_originalTimeScale;

        if (OnGameResumed != null)
        {
            OnGameResumed();
        }
    }
    
    /// <summary>
    /// Starts new game.
    /// </summary>
    public static void StartNewGame()
    {
        if (OnGameStarted != null)
        {
            OnGameStarted();
        }
    }
}
