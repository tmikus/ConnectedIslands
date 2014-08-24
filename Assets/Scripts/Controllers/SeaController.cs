using System;
using UnityEngine;
using System.Collections;

public class SeaController : MonoBehaviour
{
    /// <summary>
    /// Number of horizontal tiles to create.
    /// </summary>
    public int m_horizontalTiles = 10;

    /// <summary>
    /// Object to use to represent sea tile.
    /// </summary>
    public GameObject m_seaTileGameObject;

    /// <summary>
    /// Number of vertical tiles to create.
    /// </summary>
    public int m_verticalTiles = 10;

    /// <summary>
    /// Called when the sea object is created.
    /// </summary>
    private void Start()
    {
        var spriteBounds = m_seaTileGameObject.GetComponent<SpriteRenderer>().bounds;
        var horizontalMiddle = m_horizontalTiles / 2;
        var verticalMiddle = m_verticalTiles / 2;
        var spriteHeight = spriteBounds.max.y;
        var spriteWidth = spriteBounds.max.x;
        var spriteMidHeight = spriteHeight / 2;
        var spriteMidWidth = spriteWidth / 2;
        
        for (var horizontalTile = 1; horizontalTile <= m_horizontalTiles; ++horizontalTile)
        {
            for (var verticalTile = 1; verticalTile <= m_verticalTiles; ++verticalTile)
            {
                var horizontalOffset = horizontalMiddle - horizontalTile;
                var verticalOffset = verticalMiddle - verticalTile;

                var positionX = horizontalOffset * spriteWidth + spriteMidWidth;
                var positionY = verticalOffset * spriteHeight + spriteMidHeight;
                var seaTilePosition = new Vector3(positionX, positionY, m_seaTileGameObject.transform.position.z);

                Instantiate(m_seaTileGameObject, seaTilePosition, m_seaTileGameObject.transform.rotation);
            }
        }
    }
}
