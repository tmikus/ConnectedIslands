using System;
using System.Collections;
using UnityEngine;

public class EnemyShipAI : MonoBehaviour
{
    /// <summary>
    /// Enumeration of enemy states.
    /// </summary>
    private enum EnemyState
    {
        /// <summary>
        /// Enemy is patrolling the souroundings in search of player.
        /// </summary>
        Patrolling,
        /// <summary>
        /// Enemy is alarmed.
        /// </summary>
        Alarmed,
        /// <summary>
        /// Enemy is atacking.
        /// </summary>
        Atacking,
        /// <summary>
        /// Enemy is running away from player.
        /// </summary>
        Fleeing,
        /// <summary>
        /// Enemy is chasing player.
        /// </summary>
        Chasing
    }

    /// <summary>
    /// Precision for finding direction.
    /// </summary>
    private const float DirectionPrecision = 0.05f;

    /// <summary>
    /// Distance in which the enemy shoots at player.
    /// </summary>
    private const float DistanceToShootAtPlayer = 10.0f;

    /// <summary>
    /// Distance in which this ship sees the player.
    /// </summary>
    private const float DistanceToSpotPlayer = 25.0f;

    /// <summary>
    /// Threshold for the enemy to start fleeing.
    /// </summary>
    private const int FleeHealthPointsThreshold = 20;

    /// <summary>
    /// Interval for the logic processing.
    /// Time is in seconds.
    /// </summary>
    private const float LogicProcessingInterval = 1.0f;

    /// <summary>
    /// Time required to start atacking player.
    /// </summary>
    private const float TimeToStartAtacking = 5.0f;

    /// <summary>
    /// Time required to start patrolling.
    /// </summary>
    private const float TimeToStartPatrolling = 10.0f;

    /// <summary>
    /// Time required to stop chasing player.
    /// </summary>
    private const float TimeToStopChasing = 10.0f;

    /// <summary>
    /// State of the enemy logic.
    /// </summary>
    private EnemyState m_state = EnemyState.Patrolling;

    /// <summary>
    /// Controller for the enemy ship.
    /// </summary>
    private ShipController m_enemyShipController;

    /// <summary>
    /// Expected dot product of the turning.
    /// </summary>
    private float m_expectedDotProduct;

    /// <summary>
    /// Has the enemy chosen turning direction?
    /// </summary>
    private bool m_hasSetTurningDirection;

    /// <summary>
    /// Is enemy trying to change direction?
    /// </summary>
    private bool m_isChangingDirection;

    /// <summary>
    /// Time of last direction update.
    /// </summary>
    private float m_lastDirectionUpdate;

    /// <summary>
    /// Time of next direction update.
    /// </summary>
    private float m_nextDirectionUpdate;

    /// <summary>
    /// Player's ship instance.
    /// </summary>
    private GameObject m_playerShip;

    /// <summary>
    /// Time of the last state change.
    /// </summary>
    private float m_stateChangeTime;

    /// <summary>
    /// Target direction of the player.
    /// </summary>
    private Vector2 m_targetDirection;

    /// <summary>
    /// Alarms the enemy that the player is in sight.
    /// </summary>
    private void Alarm()
    {
        SetState(EnemyState.Alarmed);
        gameObject.SendMessage("SetSailExpansionLevel", (ushort)1);
    }

    /// <summary>
    /// Starts atacking the player.
    /// </summary>
    private void Atack()
    {
        SetState(EnemyState.Atacking);
        gameObject.SendMessage("SetSailExpansionLevel", (ushort)1);
    }

    /// <summary>
    /// Called when the enemy AI was created.
    /// </summary>
    private void Awake()
    {
        m_enemyShipController = gameObject.GetComponent<ShipController>();
        Patrol();
        StartCoroutine(ProcessLogic());
    }

    /// <summary>
    /// Changes the direction of the enemy movement.
    /// </summary>
    /// <param name="targetDirection">Target direction of the enemy to achieve.</param>
    private void ChangeDirection(Vector2 targetDirection)
    {
        m_isChangingDirection = true;
        m_targetDirection = targetDirection;
        m_hasSetTurningDirection = false;
    }

    /// <summary>
    /// Starts chasing the player.
    /// </summary>
    private void Chase()
    {
        SetState(EnemyState.Chasing);
        gameObject.SendMessage("SetSailExpansionLevel", (ushort)2);
    }

    /// <summary>
    /// Checks if the enemy should atack the player.
    /// </summary>
    private void CheckIfAtacking()
    {
        if (Time.realtimeSinceStartup - m_stateChangeTime >= TimeToStartAtacking)
        {
            Atack();
        }
    }

    /// <summary>
    /// Checks if the enemy should stop being alarmed or fleeing
    /// </summary>
    private void CheckIfPatrolling()
    {
        if (Time.realtimeSinceStartup - m_stateChangeTime >= TimeToStartPatrolling)
        {
            Patrol();
        }
    }

    /// <summary>
    /// Checks if the enemy should stop chasing and go back to patroling.
    /// </summary>
    private void CheckIfStopChasing()
    {
        if (Time.realtimeSinceStartup - m_stateChangeTime >= TimeToStopChasing)
        {
            Alarm();
        }
    }

    /// <summary>
    /// Updates the enemy UI based on the state.
    /// </summary>
    private void FixedUpdate()
    {
        if (GetPlayerShip() == null)
        {
            Patrol();
        }

        switch (m_state)
        {
            case EnemyState.Patrolling:
            case EnemyState.Alarmed:
                if (Time.realtimeSinceStartup - m_nextDirectionUpdate >= 0)
                {
                    m_lastDirectionUpdate = Time.realtimeSinceStartup;
                    m_nextDirectionUpdate = m_lastDirectionUpdate + UnityEngine.Random.Range(0.5f, 5.0f);
                    ChangeDirection(new Vector2(0, 1).Rotate(UnityEngine.Random.Range(0, Mathf.PI * 2)));
                }
                break;
            case EnemyState.Chasing:
                if (Time.realtimeSinceStartup - m_nextDirectionUpdate >= 0)
                {
                    m_lastDirectionUpdate = Time.realtimeSinceStartup;
                    m_nextDirectionUpdate = m_lastDirectionUpdate + UnityEngine.Random.Range(0.3f, 1.0f);

                    FollowPlayer();
                }
                break;
            case EnemyState.Fleeing:
                if (Time.realtimeSinceStartup - m_nextDirectionUpdate >= 0)
                {
                    m_lastDirectionUpdate = Time.realtimeSinceStartup;
                    m_nextDirectionUpdate = m_lastDirectionUpdate + 1.0f;

                    m_expectedDotProduct = 1.0f;
                    var direction = transform.position - m_playerShip.transform.position;
                    direction.Normalize();
                    ChangeDirection(direction);
                }
                break;
            case EnemyState.Atacking:
                if (Time.realtimeSinceStartup - m_nextDirectionUpdate >= 0)
                {
                    m_lastDirectionUpdate = Time.realtimeSinceStartup;
                    m_nextDirectionUpdate = m_lastDirectionUpdate + UnityEngine.Random.Range(0.3f, 1.0f);

                    var direction = m_playerShip.transform.position - transform.position;
                    direction.Normalize();
                    ChangeDirection(direction);

                    if (m_playerShip.GetComponent<ShipController>().CanFireCanons() && IsInShootingRange())
                    {
                        if (IsInShootingPosition())
                        {
                            if (IsInShootingLeft())
                            {
                                gameObject.SendMessage("FireLeftCanons");
                            }
                            else
                            {
                                gameObject.SendMessage("FireRightCanons");
                            }
                        }
                        else
                        {
                            m_expectedDotProduct = 0.0f;
                        }
                    }
                    else
                    {
                        FollowPlayer();
                    }
                }
                break;
        }

        // Is enemy changing direction?
        if (m_isChangingDirection)
        {
            ProcessDirectionChange();
        }
    }

    /// <summary>
    /// Starts fleeing from the battle.
    /// </summary>
    private void Flee()
    {
        SetState(EnemyState.Fleeing);
        gameObject.SendMessage("SetSailExpansionLevel", (ushort)2);
    }

    /// <summary>
    /// Follow player's direction.
    /// </summary>
    private void FollowPlayer()
    {
        m_expectedDotProduct = 1.0f;

        var direction = m_playerShip.transform.position - transform.position;
        direction.Normalize();
        ChangeDirection(direction);
    }

    /// <summary>
    /// Gets the player ship.
    /// </summary>
    /// <returns>Instance of the player ship.</returns>
    private GameObject GetPlayerShip()
    {
        if (m_playerShip == null)
        {
            m_playerShip = GameObject.FindGameObjectWithTag("Player");
        }

        return m_playerShip;
    }

    /// <summary>
    /// Checks if enemy is in shooting left canons.
    /// </summary>
    /// <returns>True if he is; otherwise false.</returns>
    private bool IsInShootingLeft()
    {
        var crossProduct = Vector3.Cross(rigidbody2D.velocity.normalized, m_targetDirection);

        return crossProduct.z > 0;
    }

    /// <summary>
    /// Checks if enemy is in shooting position.
    /// </summary>
    /// <returns>True if he is; otherwise false.</returns>
    private bool IsInShootingPosition()
    {
        var dotProduct = Vector2.Dot(rigidbody2D.velocity.normalized, m_targetDirection);

        return Math.Abs(0.0f - dotProduct) < DirectionPrecision * 2;
    }

    /// <summary>
    /// Is the enemy in shooting range?
    /// </summary>
    /// <returns>True if he is; otherwise false.</returns>
    private bool IsInShootingRange()
    {
        var playerShip = GetPlayerShip();
        return playerShip != null && Vector2.Distance(transform.position, playerShip.transform.position) <= DistanceToShootAtPlayer;
    }

    /// <summary>
    /// Is the player in sight?
    /// </summary>
    /// <returns>True if he is; otherwise false.</returns>
    private bool IsPlayerInSight()
    {
        var playerShip = GetPlayerShip();
        return playerShip != null && Vector2.Distance(transform.position, playerShip.transform.position) <= DistanceToSpotPlayer;
    }

    /// <summary>
    /// Starts patrolling the suroundings.
    /// </summary>
    private void Patrol()
    {
        SetState(EnemyState.Patrolling);
        gameObject.SendMessage("SetSailExpansionLevel", (ushort)1);
    }

    /// <summary>
    /// Processes the direction change.
    /// </summary>
    private void ProcessDirectionChange()
    {
        var velocityNormalized = rigidbody2D.velocity.normalized;
        var dotProduct = Vector2.Dot(velocityNormalized, m_targetDirection);

        if (Math.Abs(m_expectedDotProduct - dotProduct) < DirectionPrecision)
        {
            m_isChangingDirection = false;
            StartCoroutine(StopTurning());
        }
        else if (!m_hasSetTurningDirection)
        {
            m_hasSetTurningDirection = true;

            var crossProduct = Vector3.Cross(velocityNormalized, m_targetDirection);

            if (crossProduct.z < 0)
            {
                gameObject.SendMessage("SetHorizontalInput", 1.0f);
            }
            else
            {
                gameObject.SendMessage("SetHorizontalInput", -1.0f);
            }
        }
    }

    /// <summary>
    /// Processes the ship's logic.
    /// </summary>
    /// <returns>Delay.</returns>
    private IEnumerator ProcessLogic()
    {
        while (gameObject)
        {
            if (GetPlayerShip() == null)
            {
                Patrol();
            }
            else if (IsPlayerInSight())
            {
                switch (m_state)
                {
                    case EnemyState.Patrolling:
                        Alarm();
                        break;
                    case EnemyState.Alarmed:
                        CheckIfAtacking();
                        break;
                    case EnemyState.Atacking:
                        if (m_enemyShipController.m_healthPoints < FleeHealthPointsThreshold)
                        {
                            Flee();
                        }
                        break;
                    case EnemyState.Chasing:
                        Atack();
                        break;
                }
            }
            else
            {
                switch (m_state)
                {
                    case EnemyState.Alarmed:
                        CheckIfPatrolling();
                        break;
                    case EnemyState.Atacking:
                        Chase();
                        break;
                    case EnemyState.Chasing:
                        CheckIfStopChasing();
                        break;
                    case EnemyState.Fleeing:
                        Patrol();
                        break;
                }
            }

            yield return new WaitForSeconds(LogicProcessingInterval);
        }
    }

    /// <summary>
    /// Sets the state of the enemy.
    /// </summary>
    /// <param name="state">State to set.</param>
    private void SetState(EnemyState state)
    {
        m_state = state;
        m_stateChangeTime = Time.realtimeSinceStartup;
    }

    /// <summary>
    /// Stops turning of the ship after specified some time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StopTurning()
    {
        yield return new WaitForSeconds(0.2f);

        gameObject.SendMessage("SetHorizontalInput", 0.0f);
    }
}
