using System;
using System.Collections;
using UnityEngine;

public class Loot
{
    /// <summary>
    /// Enumeration of loot type.
    /// </summary>
    public enum LootType
    {
        Money,
        Cloth,
        Gold,
        Metal,
        Wood
    }

    /// <summary>
    /// Amount looted.
    /// </summary>
    public int m_amount;

    /// <summary>
    /// Type of the loot.
    /// </summary>
    public LootType m_type;

    /// <summary>
    /// Creates empty loot.
    /// </summary>
    public Loot()
    {
    }

    /// <summary>
    /// Creates loot from specified values.
    /// </summary>
    /// <param name="type">Type of the loot.</param>
    /// <param name="amount">Looted amount.</param>
    public Loot(LootType type, int amount)
    {
        m_amount = amount;
        m_type = type;
    }
}
