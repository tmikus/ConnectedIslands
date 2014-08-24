using System;
using UnityEngine;
using System.Collections;

public class PlayerShipController : MonoBehaviour
{
    /// <summary>
    /// Player's camera.
    /// </summary>
    private GameObject m_camera;

    /// <summary>
    /// Has the sail expansion been processed?
    /// </summary>
    private bool m_processedSailExpansion;

    /// <summary>
    /// Has the sail retration been processed?
    /// </summary>
    private bool m_processedSailRetraction;

    /// <summary>
    /// Called when the player ship controller has been awaken.
    /// </summary>
    private void Awake()
    {
        m_camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    /// <summary>
    /// Updates the logic of player ship movement.
    /// </summary>
    private void Update()
    {
        // Expanding or retracting sails.
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!m_processedSailExpansion)
            {
                m_processedSailExpansion = true;

                gameObject.SendMessage("ExpandSails");
                m_camera.SendMessage("ZoomOut");
            }
        }
        else
        {
            m_processedSailExpansion = false;

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (!m_processedSailRetraction)
                {
                    m_processedSailRetraction = true;

                    gameObject.SendMessage("RetractSails");
                    m_camera.SendMessage("ZoomIn");
                }
            }
            else
            {
                m_processedSailRetraction = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            gameObject.SendMessage("FireLeftCanons");
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            gameObject.SendMessage("FireRightCanons");
        }

        gameObject.SendMessage("SetHorizontalInput", Input.GetAxis("Horizontal"));
    }
}
