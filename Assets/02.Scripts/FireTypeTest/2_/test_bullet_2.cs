using UnityEngine;
using System.Collections;

public class test_bullet_2 : MonoBehaviour
{
    public Vector2 Direction
    {
        get;
        private set;
    }
    public float BulletSpeed
    {
        get;
        private set;
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
        this.Direction = direction;  // this.direction을 쓰는지 Direction을 쓰는지 질문
        this.BulletSpeed = bulletSpeed;
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
        //   MoveChild();

        Direction = oldDirection;
        BulletSpeed = oldBulletSpeed;
    }
}
