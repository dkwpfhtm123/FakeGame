using UnityEngine;
using System.Collections;

public class test_bullet_2 : MonoBehaviour
{
    private Vector2 direction;
    public Vector2 Direction
    {
        get
        {
            return direction;
        }
        private set
        {
            direction = value;
        }
    }
    private float bulletSpeed;
    public float BulletSpeed
    {
        get
        {
            return bulletSpeed;
        }
        private set
        {
            bulletSpeed = value;
        }
    }

    private test_fireobject_2 parent;
    private Transform transformCache;

    void Start()
    {
        transformCache = GetComponent<Transform>();
        parent = gameObject.GetComponentInParent<test_fireobject_2>();
    }

    void Update()
    {
        if (test_manager_2.Instance.StopTime == false)
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

    public void SetValue(Vector2 direction , float bulletSpeed)
    {
        this.direction = direction;  // this.direction을 쓰는지 Direction을 쓰는지 질문
        this.bulletSpeed = bulletSpeed;
    }

    private void MoveChild()
    {
        Vector2 position = transformCache.localPosition;
        Vector2 oppositeParentDirection = GlobalMethod.Global.RotateDirection(parent.Direction, -180.0f);

        position += oppositeParentDirection * parent.ObjectSpeed * Time.deltaTime;

        transformCache.localPosition = position;
    }

    private void MoveBullet()
    {
        Vector2 position = transformCache.localPosition;

        position += direction * bulletSpeed * Time.deltaTime;

        transformCache.localPosition = position;
    }

    private void ChangeDirection()
    {
        Vector2 oldDirection = direction;
        float oldBulletSpeed = bulletSpeed;

        bulletSpeed = 0.5f;
        direction = GlobalMethod.Global.RotateDirection(parent.Direction, -180.0f);

        MoveBullet();
        //   MoveChild();

        direction = oldDirection;
        bulletSpeed = oldBulletSpeed;
    }
}
