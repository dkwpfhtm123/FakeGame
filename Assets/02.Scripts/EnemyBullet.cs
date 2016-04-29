﻿using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{
    public delegate Vector2 MoveTypeDelegate(float deltaTime);
    public MoveTypeDelegate MoveType;

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

    private float angle;
    public float Angle
    {
        get
        {
            return angle;
        }
        private set
        {
            angle = value;
        }
    }

    private Transform transformCache;
    private BulletTypeScript bulletType;
    private float currentTime;
    private Vector2 startingPosition;


    void Start()
    {
        transformCache = GetComponent<Transform>();
        bulletType = gameObject.GetComponent<BulletTypeScript>();
        currentTime = 0;
        startingPosition = transformCache.localPosition;

        float angle = GetRotation();
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        if (bulletType.BulletTypeCheck == BulletType.BlueKnife)
        {
            StartCoroutine(BoomBullet());
        }
    }

    void Update()
    {
        MoveBullet();

        if (GameMgr.Instance.OnGoingBoom == true)
        {
            ItemSpawn.Instance.SpawnItem(gameObject.transform, ItemSpawn.ItemTypeObject.ScoreItem);
            Destroy(gameObject);
        }
    }

    public void setValue(Vector2 direction, float bulletSpeed , float angle)
    {
        this.direction = direction;
        this.bulletSpeed = bulletSpeed;
        this.angle = angle;
    }

    private float GetRotation()
    {
        float angle = -Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
        return angle;
    }

    private void MoveBullet()
    {
        if (MoveType != null)
        {
            Direction = MoveType(currentTime); // EnemyAttackType에서 받아옴.
        }
        Vector2 targetDirection = Direction;
        Vector2 position = Direction;

        float radAngle = Angle * Mathf.Deg2Rad;

        position.x = targetDirection.x * Mathf.Cos(radAngle) - targetDirection.y * Mathf.Sin(radAngle);
        position.y = targetDirection.x * Mathf.Sin(radAngle) + targetDirection.y * Mathf.Cos(radAngle);

        transform.localPosition = startingPosition + (BulletSpeed * position * currentTime);

        currentTime += Time.deltaTime;
    }

    private IEnumerator BoomBullet()
    {
        yield return new WaitForSeconds(1.0f); // 1 초후 폭발.
        EnemyAttackType.Instance.FireConeType(transformCache, EnemyAttackType.AttackType.RedAttack , 2.0f);
        Destroy(gameObject);
    }
}