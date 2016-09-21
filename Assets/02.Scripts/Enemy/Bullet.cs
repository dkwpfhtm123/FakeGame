﻿






using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class Bullet : MonoBehaviour
    {
        public delegate Vector2 MoveTypeDelegate(float deltaTime);
        public MoveTypeDelegate MoveType;

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

        public float Angle
        {
            get;
            private set;
        }

        public Player.PlayerCtrl CheckBoom
        {
            get;
            private set;
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
            if (CheckBoom.OnGoingBoom == true)
            {
                ItemSpawn.Instance.SpawnItem(transformCache, ItemSpawn.ItemTypeObject.ScoreItem);
                Destroy(gameObject);
            }
        }

        public void SetUp(Vector2 direction, float bulletSpeed, float angle, Player.PlayerCtrl target)
        {
            Direction = direction;
            BulletSpeed = bulletSpeed;
            Angle = angle;
            CheckBoom = target;
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
            AttackKinds.Instance.FireConeType(transformCache, AttackKinds.AttackType.RedAttack, 2.0f);
            Destroy(gameObject);
        }
    }
}