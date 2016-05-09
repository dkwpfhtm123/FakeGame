using UnityEngine;
using System.Collections;

public static class GlobalClass
{
    public static Vector2 RotateDirection(Vector2 direction, float degree)
    {
        float radian = degree * Mathf.Deg2Rad;
        Vector2 targetDirection = direction;

        direction.x = targetDirection.x * Mathf.Cos(radian) - targetDirection.y * Mathf.Sin(radian);
        direction.y = targetDirection.x * Mathf.Sin(radian) + targetDirection.y * Mathf.Cos(radian);

        return direction;
    }
}
