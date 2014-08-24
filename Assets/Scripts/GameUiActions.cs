using System;
using System.Globalization;
using System.Net.Mime;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUiActions : MonoBehaviour
{
    /// <summary>
    /// Is the game menu open?
    /// </summary>
    private bool m_isGameMenuOpen;

    /// <summary>
    /// Text field containing amount of cloth owned by player.
    /// </summary>
    private Text m_clothText;

    /// <summary>
    /// Text field containing amount of gold owned by player.
    /// </summary>
    private Text m_goldText;

    /// <summary>
    /// Text for the help button.
    /// </summary>
    private Text m_helpButtonText;

    /// <summary>
    /// Help game object.
    /// </summary>
    private GameObject m_helpGameObject;

    /// <summary>
    /// Text field containing amount of metal owned by player.
    /// </summary>
    private Text m_metalText;

    /// <summary>
    /// Text field containing amount of money owned by player.
    /// </summary>
    private Text m_moneyText;

    /// <summary>
    /// Text field containing amount of wood owned by player.
    /// </summary>
    private Text m_woodText;

    /// <summary>
    /// Called when the game UI has been created.
    /// </summary>
    private void Awake()
    {
        m_helpButtonText = transform.Find("ui_canvas/ui_bHelp/Text").GetComponent<Text>();
        m_helpGameObject = transform.Find("ui_canvas/ui_tHelp").gameObject;
    }

	// Use this for initialization
	private void Start ()
	{
	    m_isGameMenuOpen = false;

        Game.StartNewGame();
        GameUiEventManager.OnGameMenuClosed += OnGameMenuClosed;
        Player.OnPlayerMoneyChanged += OnPlayerMoneyChanged;
        Player.OnPlayerResourcesChanged += OnPlayerResourcesChanged;

        var resourcesPanel = GameObject.Find("ui_pResources").transform;
	    m_clothText = resourcesPanel.FindChild("ui_tClothAmount").GetComponent<Text>();
	    m_goldText = resourcesPanel.FindChild("ui_tGoldAmount").GetComponent<Text>();
	    m_metalText = resourcesPanel.FindChild("ui_tMetalAmount").GetComponent<Text>();
	    m_moneyText = resourcesPanel.FindChild("ui_tMoneyAmount").GetComponent<Text>();
	    m_woodText = resourcesPanel.FindChild("ui_tWoodAmount").GetComponent<Text>();

	    m_moneyText.text = Player.Money.ToString();
	}
    
    /// <summary>
    /// Called when the game menu was closed.
    /// </summary>
    private void OnGameMenuClosed()
    {
        m_isGameMenuOpen = false;
    }

    /// <summary>
    /// Called when the player's money has changed.
    /// Updates the UI to reflect those changes.
    /// </summary>
    /// <param name="money">Amount of money that player has.</param>
    private void OnPlayerMoneyChanged(int money)
    {
        m_moneyText.text = money.ToString();
    }

    /// <summary>
    /// Updates the UI to reflect player's resources.
    /// </summary>
    /// <param name="playerCargoBay">Instance of the player's cargo bay.</param>
    private void OnPlayerResourcesChanged(CargoBay playerCargoBay)
    {
        m_clothText.text = playerCargoBay.m_cloth.ToString();
        m_goldText.text = playerCargoBay.m_gold.ToString();
        m_metalText.text = playerCargoBay.m_metal.ToString();
        m_woodText.text = playerCargoBay.m_wood.ToString();
    }

    /// <summary>
    /// Opens the game menu screen.
    /// </summary>
	public void OpenGameMenu()
	{
	    if (m_isGameMenuOpen)
            return;

        m_isGameMenuOpen = true;
		Application.LoadLevelAdditive("GameMenu");
	}

    /// <summary>
    /// Toggle show help.
    /// </summary>
    public void ToggleShowHelp()
    {
        if (m_helpGameObject.activeSelf)
        {
            m_helpButtonText.text = "Show Help";
            m_helpGameObject.SetActive(false);
        }
        else
        {
            m_helpButtonText.text = "Hide Help";
            m_helpGameObject.SetActive(true);
        }
    }
}
