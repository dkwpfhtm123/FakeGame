using UnityEngine;
using System.Collections;

public class TPBullet : MonoBehaviour
{
    public enum Side
    {
        Up,
        Down,
        Left,
        Right
    }

    public Side SideOption;

    public Vector2 Direction;
    public float Speed;

    public Vector2 CrossPoint;
    public Vector2 TeleportPoint;

    public float Angle;

    private Transform transformCache;

    private float teleportNumber;

    void Start()
    {
        transformCache = GetComponent<Transform>();

        teleportNumber = 0;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 position = transformCache.localPosition;

        if (SideOption == Side.Up)
        {
            if (position.y > CrossPoint.y && teleportNumber == 0)
            {
                position = Teleport(position);

            }
            else if (position.y < CrossPoint.y && teleportNumber == 1)
            {
                position = Teleport(position);
            }
        }
        else if (SideOption == Side.Left)
        {
            if (position.x < CrossPoint.x && teleportNumber == 0)
            {
                position = Teleport(position);
            }
            else if (position.x > CrossPoint.x && teleportNumber == 1)
            {
                position = Teleport(position);
            }
        }
        else if (SideOption == Side.Down)
        {
            if (position.y < CrossPoint.y && teleportNumber == 0)
            {
                position = Teleport(position);
            }
            else if (position.y > CrossPoint.y && teleportNumber == 1)
            {
                position = Teleport(position);
            }
        }
        else if (SideOption == Side.Right)
        {
            if (position.x > CrossPoint.x && teleportNumber == 0)
            {
                position = Teleport(position);
            }
            else if (position.x < CrossPoint.x && teleportNumber == 1)
            {
                position = Teleport(position);
            }
        }

        position += Direction * Speed * Time.deltaTime;

        transformCache.localPosition = position;
    }

    private Vector2 Teleport(Vector2 position)
    {
        Direction = GlobalClass.RotateDirection(Direction, 180.0f);
        float rotateAngle = -Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
        transformCache.localRotation = Quaternion.Euler(0, 0, rotateAngle);

        transform.localPosition = TeleportPoint;
        position = transformCache.localPosition;

        teleportNumber++;

        return position;
    }

    public void SetVariable(Vector2 direction, float speed, Vector2 crossPoint, Vector2 teleportPoint, Side sideOption, float angle)
    {
        Direction = direction;
        Speed = speed;
        CrossPoint = crossPoint;
        TeleportPoint = teleportPoint;
        SideOption = sideOption;
        Angle = angle;
    }
}
