using System;
using UnityEngine;

public class CanonBallController : MonoBehaviour
{
    /// <summary>
    /// Life time of the canon ball.
    /// After that time ball desapears.
    /// </summary>
    public const float CanonBallLifeTime = 2.0f;

    /// <summary>
    /// Damage that the ball deals.
    /// </summary>
    private int m_damage = 10;

    /// <summary>
    /// Time of spawning of the canon ball.
    /// </summary>
    private float m_spawnTime;

    /// <summary>
    /// Called when the canon ball was created.
    /// </summary>
    private void Awake()
    {
        m_spawnTime = Time.realtimeSinceStartup;
    }

    /// <summary>
    /// Causes the canon ball to explode.
    /// </summary>
    private void ExplodeCanonball()
    {
        // TODO: Implement exploding.
        Destroy(gameObject);
    }

    /// <summary>
    /// Called when the collision has occurred.
    /// </summary>
    /// <param name="collision">Collision that has occurred</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy"
            || collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("DoDamage", m_damage);
        }
        
        ExplodeCanonball();
    }

    /// <summary>
    /// Called each frame to update the logic of the canon ball.
    /// </summary>
    private void Update()
    {
        // If time to live of the canon ball has ended - kill it.
        if (Time.realtimeSinceStartup - m_spawnTime > CanonBallLifeTime)
        {
            Destroy(gameObject);
        }
    }
}
