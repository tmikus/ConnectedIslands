using UnityEngine;

static class Vector2Extensions
{
    /// <summary>
    /// Rotates the vector2 around its position by specified amount of radians.
    /// </summary>
    /// <param name="vector2">Vector to rotate.</param>
    /// <param name="radians">Angle in radians.</param>
    /// <returns>Rotated vector.</returns>
    public static Vector2 Rotate(this Vector2 vector2, float radians)
    {
        var sin = Mathf.Sin(radians);
        var cos = Mathf.Cos(radians);

        var x = vector2.x;
        var y = vector2.y;

        vector2.x = (cos * x) - (sin * y);
        vector2.y = (sin * x) + (cos * y);

        return vector2;
    }
}