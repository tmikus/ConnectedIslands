using UnityEngine;

public static class Player
{
    /// <summary>
    /// Money owned by player.
    /// </summary>
    private static int m_money;

    /// <summary>
    /// Instance of harbor in which player is docked.
    /// </summary>
    public static HarborController m_currentHarbor;

    /// <summary>
    /// Gets or sets player money.
    /// </summary>
    public static int Money
    {
        get { return m_money; }
        set
        {
            if (m_money == value)
                return;

            m_money = value;

            if (OnPlayerMoneyChanged != null)
            {
                OnPlayerMoneyChanged(value);
            }
        }
    }

    /// <summary>
    /// Delegate for event of changing player money.
    /// </summary>
    /// <param name="money">Value to which money has changed.</param>
    public delegate void PlayerMoneyChanged(int money);

    /// <summary>
    /// Delegate for the event of changing player resources.
    /// </summary>
    /// <param name="playerCargoBay">Current player's cargo bay.</param>
    public delegate void PlayerResourcesChanged(CargoBay playerCargoBay);

    /// <summary>
    /// Event called when the player money has changed.
    /// </summary>
    public static event PlayerMoneyChanged OnPlayerMoneyChanged;

    /// <summary>
    /// Called when the player's resources has changed.
    /// </summary>
    public static event PlayerResourcesChanged OnPlayerResourcesChanged;

    /// <summary>
    /// Initializes instance of the player.
    /// </summary>
    static Player()
    {
        Money = 100;
        Game.OnGameStarted += OnGameStarted;
    }

    /// <summary>
    /// Called when the game has started.
    /// Initializes player money amount.
    /// </summary>
    private static void OnGameStarted()
    {
        Money = 100;
    }

    /// <summary>
    /// Triggers the "OnPlayerResourcesChanged" event.
    /// </summary>
    /// <param name="playerCargoBay">Instance of player's cargo bay.</param>
    public static void TriggerPlayerResourcesChanged(CargoBay playerCargoBay)
    {
        if (OnPlayerResourcesChanged != null)
        {
            OnPlayerResourcesChanged(playerCargoBay);
        }
    }
}
