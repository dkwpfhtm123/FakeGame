using UnityEngine;
using System.Collections;

namespace Fake.Enemy
{
    public class EnemyBullet : BaseBullet
    {
        public delegate Vector2 MoveTypeDelegate(float deltaTime);
        public MoveTypeDelegate MoveType;

        public float Angle
        {
            get;
            private set;
        }

        public Player.PlayerController CheckBoom
        {
            get;
            private set;
        }

        private Transform transformCache;
        private BaseBullet bulletType;
        private float currentTime;
        private Vector2 startingPosition;

        void Start()
        {
            transformCache = GetComponent<Transform>();
            bulletType = gameObject.GetComponent<BaseBullet>();
            currentTime = 0;
            startingPosition = transformCache.localPosition;

            var angle = GetRotation();
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
                Item.ItemSpawn.Instance.SpawnItem(transformCache, Item.ItemSpawn.ItemTypeObject.ScoreItem);
                Destroy(gameObject);
            }
        }

        public void SetUp(float angle, Player.PlayerController target)
        {
            Angle = angle;
            CheckBoom = target;
        }

        private float GetRotation()
        {
            var angle = -Mathf.Atan2(BulletDirection.x, BulletDirection.y) * Mathf.Rad2Deg;
            return angle;
        }

        private void MoveBullet()
        {
            if (MoveType != null)
            {
                BulletDirection = MoveType(currentTime); // EnemyAttackType에서 받아옴.
            }
            var targetDirection = BulletDirection;
            var position = BulletDirection;

            var radAngle = Angle * Mathf.Deg2Rad;

            position.x = targetDirection.x * Mathf.Cos(radAngle) - targetDirection.y * Mathf.Sin(radAngle);
            position.y = targetDirection.x * Mathf.Sin(radAngle) + targetDirection.y * Mathf.Cos(radAngle);

            transform.localPosition = startingPosition + (BulletSpeed * position * currentTime);

            currentTime += Time.deltaTime;
        }

        private IEnumerator BoomBullet()
        {
            yield return new WaitForSeconds(1.0f); // 1 초후 폭발.
            EnemyAttackKinds.Instance.FireConeType(transformCache, EnemyAttackKinds.AttackType.RedAttack, 2.0f);
            Destroy(gameObject);
        }
    }
}