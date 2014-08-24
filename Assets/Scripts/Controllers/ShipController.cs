using System;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    /// <summary>
    /// Cooldown for firing canons.
    /// In seconds.
    /// </summary>
    private const float CanonFireCooldown = 5.0f;

    /// <summary>
    /// Maximum turning angle per second.
    /// The full turn should happen in 4 seconds.
    /// </summary>
    private const float MaximumTurningAngle = (Mathf.PI * 2) / 4;

    /// <summary>
    /// Maximum available sails level.
    /// </summary>
    private const int MaximumSailsLevel = 2;

    /// <summary>
    /// Minimum available sails level.
    /// </summary>
    private const int MinimumSailsLevel = 0;

    /// <summary>
    /// Velocoty gained by each of the levels of sails.
    /// </summary>
    private const int SailLevelVelocity = 2;

    /// <summary>
    /// Health points at the beginning of the game.
    /// </summary>
    public const int MaximumHealthPoints = 100;

    /// <summary>
    /// Instance of the health bar of the ship.
    /// </summary>
    private GameObject m_healthBar;

    /// <summary>
    /// Instance of health indicator.
    /// </summary>
    private GameObject m_healthIndicator;

    /// <summary>
    /// Horizontal input for the ship.
    /// This means the angle for turning of ship.
    /// </summary>
    private float m_horizontalInput;

    /// <summary>
    /// Time when canons were fired.
    /// </summary>
    private float m_lastFireCanonsTime;

    /// <summary>
    /// Vector for the ship movement.
    /// This vector should always be normalised!
    /// </summary>
    private Vector2 m_movementVector;

    /// <summary>
    /// Level of sail expansion.
    /// 0 - no sails
    /// 1 - half of sails expanded.
    /// 2 - full sails
    /// </summary>
    private ushort m_sailExpansionLevel;
    
    /// <summary>
    /// Color to use for rendering ship.
    /// </summary>
    private Color m_shipColor = Color.white;

    /// <summary>
    /// Rotation of the ship rigid body.
    /// </summary>
    private float m_shipRotation;
    
    /// <summary>
    /// Prefab used for rendering canon ball.
    /// </summary>
    public GameObject m_canonBallPrefab;

    /// <summary>
    /// Instance of the ship's cargo bay.
    /// </summary>
    public CargoBay m_cargoBay;

    /// <summary>
    /// Number of health points the ship has.
    /// </summary>
    public int m_healthPoints = MaximumHealthPoints;

    /// <summary>
    /// Amount of money to win when killing this opponent.
    /// </summary>
    public int m_lootMoney;

    /// <summary>
    /// Colour of the ship.
    /// </summary>
    public Color ShipColor
    {
        get { return m_shipColor; }
        set
        {
            m_shipColor = value;
            transform.FindChild("hull").gameObject.GetComponent<SpriteRenderer>().color = value;
        }
    }

    /// <summary>
    /// Called when the scene is started.
    /// </summary>
    private void Awake()
    {
        m_cargoBay = new CargoBay();
        m_healthBar = transform.FindChild("healthBar").gameObject;
        m_healthIndicator = m_healthBar.transform.FindChild("bar").gameObject;
        m_movementVector = new Vector2(0, 1);
        m_shipRotation = 0.0f;
    }

    /// <summary>
    /// Fires canons.
    /// </summary>
    /// <param name="left">Should fire left side? Otherwise right side.</param>
    /// <returns>Has firing been successful?</returns>
    private bool FireCanons(bool left)
    {
        if (!CanFireCanons())
        {
            return false;
        }

        GameObject canonsSide;
        Vector2 forceDirection;
        m_lastFireCanonsTime = Time.realtimeSinceStartup;

        if (left)
        {
            canonsSide = transform.FindChild("leftSideCanons").gameObject;
            forceDirection = new Vector2(-1, 0);
        }
        else
        {
            canonsSide = transform.FindChild("rightSideCanons").gameObject;
            forceDirection = new Vector2(1, 0);
        }

        var forceVector3 = transform.rotation * (forceDirection * 10);
        var forceVector2 = new Vector2(forceVector3.x, forceVector3.y);

        // For each canon in the side
        var canonsCount = canonsSide.transform.childCount;
        for (var canonIndex = 0; canonIndex < canonsCount; ++canonIndex)
        {
            var canonBallSpawnPoint = canonsSide.transform.GetChild(canonIndex).GetChild(0);
            var canonBall = (GameObject)Instantiate(m_canonBallPrefab, canonBallSpawnPoint.position, canonBallSpawnPoint.rotation);
            canonBall.GetComponent<Rigidbody2D>().velocity = forceVector2;
        }

        return true;
    }

    /// <summary>
    /// Updates the logic of player ship movement.
    /// </summary>
    private void FixedUpdate()
    {
        if (m_healthPoints <= 0)
        {
            SinkShip();
            return;
        }

        // Do the movement only if sails are expanded
        if (m_sailExpansionLevel > 0)
        {
            // Calculate turning angle of the ship.
            var turningAngle = -m_horizontalInput * MaximumTurningAngle * Time.deltaTime;

            // Store the rotation of the ship
            m_shipRotation += turningAngle * Mathf.Rad2Deg;

            // Rotate the ship in that direction.
            m_movementVector = m_movementVector.Rotate(turningAngle);
        }

        // Rotat the ship to follow the direction of velocity.
        rigidbody2D.rotation = m_shipRotation;

        // Calculating ship velocity
        rigidbody2D.velocity = m_movementVector * (m_sailExpansionLevel * SailLevelVelocity);

        // Keep health bar in the same orientation.
        m_healthBar.transform.localRotation = Quaternion.Inverse(transform.localRotation);
    }
    
    /// <summary>
    /// Can player fire canons?
    /// </summary>
    /// <returns>True if he can; otherwise false.</returns>
    public bool CanFireCanons()
    {
        return Time.realtimeSinceStartup - m_lastFireCanonsTime > CanonFireCooldown;
    }

    /// <summary>
    /// Does the damage to the ship.
    /// </summary>
    /// <param name="damage">Damage to deal to the ship.</param>
    public void DoDamage(int damage)
    {
        m_healthPoints -= damage;
        UpdateHealthBar();
    }

    /// <summary>
    /// Expands sails of the ship.
    /// </summary>
    public void ExpandSails()
    {
        if (m_sailExpansionLevel < MaximumSailsLevel)
        {
            ++m_sailExpansionLevel;
        }
    }

    /// <summary>
    /// Fires the left side canons.
    /// </summary>
    /// <returns>Has firing been successful?</returns>
    public bool FireLeftCanons()
    {
        return FireCanons(true);
    }

    /// <summary>
    /// Fires the right side canons.
    /// </summary>
    /// <returns>Has firing been successful?</returns>
    public bool FireRightCanons()
    {
        return FireCanons(false);
    }

    /// <summary>
    /// Loots the resource.
    /// </summary>
    /// <param name="loot">Resource to loot.</param>
    public void LootResource(Loot loot)
    {
        switch (loot.m_type)
        {
            case Loot.LootType.Cloth:
                m_cargoBay.m_cloth += loot.m_amount;
                break;
            case Loot.LootType.Gold:
                m_cargoBay.m_gold += loot.m_amount;
                break;
            case Loot.LootType.Metal:
                m_cargoBay.m_metal += loot.m_amount;
                break;
            case Loot.LootType.Money:
                if (tag == "Player")
                {
                    Player.Money += loot.m_amount;
                }
                else
                {
                    m_lootMoney += loot.m_amount;
                }
                break;
            case Loot.LootType.Wood:
                m_cargoBay.m_wood += loot.m_amount;
                break;
        }

        if (tag == "Player")
        {
            Player.TriggerPlayerResourcesChanged(m_cargoBay);
        }
    }

    /// <summary>
    /// Retracts the sails of the ship.
    /// </summary>
    public void RetractSails()
    {
        if (m_sailExpansionLevel > MinimumSailsLevel)
        {
            --m_sailExpansionLevel;
        }
    }

    /// <summary>
    /// Sets the horizontal input for the ship.
    /// </summary>
    /// <param name="input">Input to set.</param>
    public void SetHorizontalInput(float input)
    {
        m_horizontalInput = input;
    }

    /// <summary>
    /// Sets sails expansion level to specified value.
    /// </summary>
    /// <param name="level">Level to set.</param>
    public void SetSailExpansionLevel(ushort level)
    {
        if (level > MaximumSailsLevel)
        {
            throw new IndexOutOfRangeException("Cannot set expansion level greater than Maximum Sails Level!");
        }

        m_sailExpansionLevel = level;
    }

    /// <summary>
    /// Sinks the ship.
    /// </summary>
    public void SinkShip()
    {
        if (tag == "Enemy")
        {
            var playerShip = GameObject.FindGameObjectWithTag("Player");
            var shipController = GetComponent<ShipController>();
            var cargo = shipController.m_cargoBay;

            if (shipController.m_lootMoney > 0)
            {
                playerShip.SendMessage("LootResource", new Loot(Loot.LootType.Money, shipController.m_lootMoney));
            }

            if (cargo.m_cloth > 0)
            {
                playerShip.SendMessage("LootResource", new Loot(Loot.LootType.Cloth, cargo.m_cloth));
            }

            if (cargo.m_gold > 0)
            {
                playerShip.SendMessage("LootResource", new Loot(Loot.LootType.Gold, cargo.m_gold));
            }

            if (cargo.m_metal > 0)
            {
                playerShip.SendMessage("LootResource", new Loot(Loot.LootType.Metal, cargo.m_metal));
            }

            if (cargo.m_wood > 0)
            {
                playerShip.SendMessage("LootResource", new Loot(Loot.LootType.Wood, cargo.m_wood));
            }
        }

        // TODO: Playing sinking animation.
        Destroy(gameObject);
    }

    /// <summary>
    /// Updates the health bar.
    /// </summary>
    public void UpdateHealthBar()
    {
        m_healthIndicator.transform.localScale = new Vector3((float)m_healthPoints / MaximumHealthPoints, m_healthIndicator.transform.localScale.y, m_healthIndicator.transform.localScale.z);
    }
}
