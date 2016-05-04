using UnityEngine;
using System.Collections;

public class bullet2 : MonoBehaviour
{
    private Vector2 direction;
    public Vector2 Direction
    {
        get { return direction; }
        private set { direction = value; }
    }
    public float bulletSpeed;
    public float BulletSpeed
    {
        get { return bulletSpeed; }
        private set { bulletSpeed = value; }
    }

    private fireobject2 parent;
    private Transform transformCache;

    void Start()
    {
        transformCache = GetComponent<Transform>();
        parent = gameObject.GetComponentInParent<fireobject2>();
    }

    void Update()
    {
        if (manager2.Instance.StopTime == false)
        {
            MoveBullet();
            if (parent != null)
            {
                MoveChild();
            }
        }
        else {
            ChangeDirection();
        }
    }

    public void SetValue(Vector2 direction, float bulletSpeed)
    {
        Direction = direction; 
        BulletSpeed = bulletSpeed;
    }

    private void MoveChild()
    {
        Vector2 position = transformCache.localPosition;
        Vector2 oppositeParentDirection = GlobalClass.RotateDirection(parent.Direction, -180.0f);

        position += oppositeParentDirection * parent.ObjectSpeed * Time.deltaTime;

        transformCache.localPosition = position;
    }

    private void MoveBullet()
    {
        Vector2 position = transformCache.localPosition;

        position += Direction * BulletSpeed * Time.deltaTime;

        transformCache.localPosition = position;
    }

    private void ChangeDirection()
    {
        Vector2 oldDirection = Direction;
        float oldBulletSpeed = BulletSpeed;

        BulletSpeed = 0.5f;
        Direction = GlobalClass.RotateDirection(parent.Direction, -180.0f);

        MoveBullet();

        Direction = oldDirection;
        BulletSpeed = oldBulletSpeed;
    }
}
