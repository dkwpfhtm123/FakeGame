using UnityEngine;
using System.Collections;

namespace Fake.Enemy
{
    public class EnemyAttackKinds : MonoBehaviour
    {
        public GameObject RedKnife;
        public GameObject BlueKnife;
        public GameObject PurpleCircle;

        private ObjectCreator objectCreator;
        private static Transform playerTransform;

        #region property
        public ObjectCreator ObjectCreator
        {
            get { return objectCreator; }
            set
            {
                if(objectCreator != null)
                {
                    objectCreator.OnPlayerRespawn -= OnPlayerRespawn;
                    objectCreator.OnPlayerDead -= OnPlayerDead;
                }

                objectCreator = value;

                if (objectCreator != null)
                {
                    objectCreator.OnPlayerRespawn += OnPlayerRespawn;
                    objectCreator.OnPlayerDead += OnPlayerDead;
                    OnPlayerRespawn(); // 첫 설정
                }
            }
        }

        public static Transform PlayerTransform
        {
            get { return playerTransform; }
            private set { playerTransform = value; }
        }
        #endregion

        public enum AttackType
        {
            RedAttack,
            BlueAttack,
            PurpleCircle
        }

        private void OnPlayerRespawn()
        {
            if (playerTransform == null)
                playerTransform = objectCreator.LivePlayer.transform;
        }

        private void OnPlayerDead()
        {
            if (playerTransform != null)
                playerTransform = null;
        }

        #region firetype
        public void FireConeType(EnemyStartAttackParams parameters)
        {
            var oneshot = 5;
            var angle = 60.0f;
            var anglePlus = angle / (oneshot - 1);
            angle *= 0.5f;

            while (oneshot > 0)
            {
                var targetDirection = RotateBullet(angle, parameters.SpawnTransform);
                CreateMoveBullet(targetDirection, parameters.SpawnTransform, parameters.AttackType, 0, 1, parameters.BulletSpeed);

                oneshot--;
                angle -= anglePlus;
            }
        }

        public void FireBoomType(EnemyStartAttackParams parameters)     // 무작위 발사후 ConFireType 형태로 발사.
        {
            int oneshot = 5;

            while (oneshot > 0)
            {
                var angle = Random.Range(0.0f, 360.0f);

                var targetDirection = RotateBullet(angle, parameters.SpawnTransform);
                CreateMoveBullet(targetDirection, parameters.SpawnTransform, parameters.AttackType, 0, 1, parameters.BulletSpeed);

                oneshot--;
            }
        }

        public void FireSinType(EnemyStartAttackParams parameters)     // 6방향 sin형태의 탄 6발씩 발사 후 20도 꺾어서 6발발사. x6
        {
            StartCoroutine(SinTypeCoroutine(parameters));
        }

        IEnumerator SinTypeCoroutine(EnemyStartAttackParams parameters)
        {
            var angle = 0.0f;
            var oneShot = 6.0f;
            var anglePlus = 0.0f;

            var direction = new Vector2(1, 0);

            while (true)
            {
                for (var i = 0; i < oneShot; i++)
                {
                    for (var z = 0; z < oneShot; z++)
                    {
                        CreateMoveBullet(direction, parameters.SpawnTransform, parameters.AttackType, angle, 0.5f, parameters.BulletSpeed);
                        angle += 60.0f;
                    }
                    yield return new WaitForSeconds(0.2f);
                }
                anglePlus += 20.0f;
                angle = anglePlus;
                yield return new WaitForSeconds(0.2f);
            }
        }

        private static Vector2 SinCurve(float currentTime)
        {
            var dx = 300.0f * currentTime * Mathf.Deg2Rad;
            var dy = Mathf.Sin(dx);

            var direction = new Vector2(dx, dy);

            return direction.normalized;
        }
        #endregion

        private static Vector2 RotateBullet(float angle, Transform spawnTransform)     // 총알 회전
        {
            var targetDirection = (PlayerTransform.position - spawnTransform.position).normalized;

            return GlobalClass.RotateDirection(targetDirection, angle);
        }

        #region CreateBullet - Shooter
        private void CreateMoveBullet(Vector2 targetDirection, Transform spawnTransform, AttackType attackType, float angle, float localScale, float bulletSpeed)
        {
            GameObject bulletObject = null;
            if (attackType == AttackType.RedAttack)
            {
                bulletObject = Instantiate(RedKnife);
            }
            else if (attackType == AttackType.BlueAttack)
            {
                bulletObject = Instantiate(BlueKnife);
            }
            else if (attackType == AttackType.PurpleCircle)
            {
                bulletObject = Instantiate(PurpleCircle);
            }

            var bulletTransform = bulletObject.transform;
            var bullet = bulletObject.GetComponent<EnemyBullet>();

            bulletTransform.localPosition = spawnTransform.localPosition;
            bulletTransform.localRotation = Quaternion.identity;
            bulletTransform.localScale = Vector3.one * localScale;

            bullet.SetBaseBullet(0, bulletSpeed, targetDirection.normalized, true);

            bullet.SetUp(angle, ObjectCreator, this);

            if (attackType == AttackType.PurpleCircle)
            {
                bullet.MoveType = SinCurve;
            }
            else
            {
                bullet.MoveType = null;
            }
        }
        #endregion
    }
}