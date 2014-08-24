using System;
using UnityEngine;
using System.Collections;

public class EnemySpawnPointController : MonoBehaviour
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
    /// Color to use for enemy ship.
    /// </summary>
    public Color m_color;

    /// <summary>
    /// Cooldown before spawning new instance of the enemy.
    /// Time in seconds.
    /// </summary>
    public float m_spawnCooldown = 30.0f;

    /// <summary>
    /// Instance of game object used to instantiate enemy ship.
    /// </summary>
    public GameObject m_shipPrefab;

    /// <summary>
    /// Generates random loot for the ship.
    /// </summary>
    private void GenerateRandomLoot(ShipController shipController)
    {
        shipController.m_lootMoney = UnityEngine.Random.Range(0, 100);
        shipController.m_cargoBay.m_cloth = UnityEngine.Random.Range(0, 100);
        shipController.m_cargoBay.m_gold = UnityEngine.Random.Range(0, 100);
        shipController.m_cargoBay.m_metal = UnityEngine.Random.Range(0, 100);
        shipController.m_cargoBay.m_wood = UnityEngine.Random.Range(0, 100);
    }

    /// <summary>
    /// Monitors life of the enemy.
    /// If enemy was killed it waits some time before re-creating him.
    /// </summary>
    /// <returns>Enumerator for managing time of the coroutine.</returns>
    private IEnumerator MonitorEnemyLife()
    {
        do
        {
            yield return new WaitForSeconds(1.0f);
        }
        while (m_shipGameObject != null);

        yield return new WaitForSeconds(m_spawnCooldown);

        SpawnEnemy();
    }

    /// <summary>
    /// Spawns new enemy.
    /// </summary>
    private void SpawnEnemy()
    {
        m_shipGameObject = (GameObject)Instantiate(m_shipPrefab, transform.position, transform.rotation);
        m_shipGameObject.tag = "Enemy";

        m_shipController = m_shipGameObject.GetComponent<ShipController>();
        m_shipController.ShipColor = m_color;

        m_shipGameObject.AddComponent<EnemyShipAI>();

        GenerateRandomLoot(m_shipController);

        StartCoroutine(MonitorEnemyLife());
    }

    /// <summary>
    /// Called when the script was awaken.
    /// </summary>
    private void Start()
    {
       SpawnEnemy();
    }
}
