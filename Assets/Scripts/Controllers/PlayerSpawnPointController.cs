using System;
using UnityEngine;
using System.Collections;

public class PlayerSpawnPointController : MonoBehaviour
{
    /// <summary>
    /// Controller used for controlling the ship.
    /// </summary>
    private ShipController m_shipController;

    /// <summary>
    /// Instance of game object.
    /// </summary>
    private GameObject m_shipGameObject;

    /// <summary>
    /// Color to use for player ship.
    /// </summary>
    public Color m_color;

    /// <summary>
    /// Instance of game object used to instantiate player ship.
    /// </summary>
    public GameObject m_shipPrefab;

    /// <summary>
    /// Cooldown before spawning new instance of the enemy.
    /// Time in seconds.
    /// </summary>
    public float m_spawnCooldown = 5.0f;

    /// <summary>
    /// Monitors life of the player.
    /// If player was killed it waits some time before re-creating him.
    /// </summary>
    /// <returns>Enumerator for managing time of the coroutine.</returns>
    private IEnumerator MonitorPlayerLife()
    {
        do
        {
            yield return new WaitForSeconds(1.0f);
        }
        while (m_shipGameObject != null);

        yield return new WaitForSeconds(m_spawnCooldown);

        SpawnPlayer();
    }

    /// <summary>
    /// Spawns new enemy.
    /// </summary>
    private void SpawnPlayer()
    {
        m_shipGameObject = (GameObject)Instantiate(m_shipPrefab, transform.position, transform.rotation);
        m_shipGameObject.tag = "Player";

        m_shipController = m_shipGameObject.GetComponent<ShipController>();
        m_shipController.ShipColor = m_color;

        m_shipGameObject.AddComponent<PlayerShipController>();

        var playerCamera = GameObject.Find("/root/playerCamera");
        playerCamera.GetComponent<SmoothFollow2D>().m_target = m_shipGameObject.transform;

        Player.TriggerPlayerResourcesChanged(m_shipController.m_cargoBay);

        StartCoroutine(MonitorPlayerLife());
    }

    /// <summary>
    /// Called when the script was awaken.
    /// </summary>
    private void Start()
    {
        SpawnPlayer();
    }
}
