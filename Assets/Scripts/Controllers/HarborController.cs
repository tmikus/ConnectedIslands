using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HarborController : MonoBehaviour
{
    /// <summary>
    /// Multiplier for the cloth price.
    /// </summary>
    public float m_clothPriceMultiplier = 1.0f;

    /// <summary>
    /// Multiplier for the gold price.
    /// </summary>
    public float m_goldPriceMultiplier = 1.0f;

    /// <summary>
    /// Name of the harbor.
    /// </summary>
    public string m_harborName = "";

    /// <summary>
    /// Multiplier for the metal price.
    /// </summary>
    public float m_metalPriceMultiplier = 1.0f;

    /// <summary>
    /// Multiplier for the repair price.
    /// </summary>
    public float m_repairMultiplier = 1.0f;

    /// <summary>
    /// Multiplier for the wood price.
    /// </summary>
    public float m_woodPriceMultiplier = 1.0f;

    /// <summary>
    /// Called when the harbor was created.
    /// </summary>
    private void Awake()
    {
        
    }

    /// <summary>
    /// Called when the player / enemy entered the trigger.
    /// </summary>
    /// <param name="collision">Collision that has occurred</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // TODO: Check if player is being chased or attacked.

            Player.m_currentHarbor = this;
            Application.LoadLevelAdditive("TradeWithHarbor");
        }
    }

    /// <summary>
    /// Called when the player / enemy left the trigger.
    /// </summary>
    /// <param name="collision">Collision that has occurred</param>
    private void OnTriggerexit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.m_currentHarbor = null;
        }
    }
}
