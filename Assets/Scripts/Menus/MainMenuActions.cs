using UnityEngine;
using System.Collections;

public class MainMenuActions : MonoBehaviour
{
	/// <summary>
    /// Use this for initialization.
	/// </summary>
	void Start ()
    {
	
	}
	
	/// <summary>
    /// Update is called once per frame.
	/// </summary>
	void Update ()
    {
	
	}

    /// <summary>
    /// Closes the game without asking for confirmation.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Opens the new game.
    /// </summary>
    public void NewGame()
    {
        Application.LoadLevel("Game");
    }
}
