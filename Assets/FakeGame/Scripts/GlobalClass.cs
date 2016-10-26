using UnityEngine;
using System.Collections;

namespace Fake
{
    public static class GlobalClass
    {
        public static Vector2 RotateDirection(Vector2 direction, float degree)
        {
            var radian = degree * Mathf.Deg2Rad;

            return new Vector2(direction.x * Mathf.Cos(radian) - direction.y * Mathf.Sin(radian), direction.x * Mathf.Sin(radian) + direction.y * Mathf.Cos(radian));
        }
    }
}