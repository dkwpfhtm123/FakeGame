using UnityEngine;
using System.Collections;

namespace Fake.Enemy
{
    public class EnemyAttackKinds : MonoBehaviour
    {
        public GameObject RedKnife;
        public GameObject BlueKnife;
        public GameObject PurpleCircle;

        public Player.PlayerController CheckBoom;

        private int erasethis;

        public enum AttackType
        {
            RedAttack,
            BlueAttack,
            PurpleCircle
        }

        private static EnemyAttackKinds instance;
        public static EnemyAttackKinds Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<EnemyAttackKinds>();

                return instance;
            }
        }

        public void FireConeType(Transform spawnTransform, AttackType attackType, float bulletSpeed)
        {
            var oneshot = 5;
            var angle = 60.0f;
            var anglePlus = angle / (oneshot - 1);
            angle *= 0.5f;

            while (oneshot > 0)
            {
                var targetDirection = RotateBullet(angle, spawnTransform);
                CreateMoveBullet(targetDirection, spawnTransform, attackType, 0, 1, bulletSpeed);

                oneshot--;
                angle -= anglePlus;
            }
        }

        public void FireBoomType(Transform spawnTransform, AttackType attackType, float bulletSpeed)     // 무작위 발사후 ConFireType 형태로 발사.
        {
            int oneshot = 5;

            while (oneshot > 0)
            {
                var angle = Random.Range(0.0f, 360.0f);

                var targetDirection = RotateBullet(angle, spawnTransform);
                CreateMoveBullet(targetDirection, spawnTransform, attackType, 0, 1, bulletSpeed);

                oneshot--;
            }
        }

        public void FireSinType(Transform spawnTransform, AttackType attackType, float bulletSpeed)     // 6방향 sin형태의 탄 6발씩 발사 후 20도 꺾어서 6발발사. x6
        {
            StartCoroutine(SinTypeCoroutine(spawnTransform, attackType, bulletSpeed));
        }

        IEnumerator SinTypeCoroutine(Transform spawnTransform, AttackType attackType, float bulletSpeed)
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
                        CreateMoveBullet(direction, spawnTransform, attackType, angle, 0.5f, bulletSpeed);
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

        private static Vector2 RotateBullet(float angle, Transform spawnTransform)     // 총알 회전
        {
            var playerTransform = GameManager.Instance.PlayerTransform;
            var targetDirection = (playerTransform.transform.position - spawnTransform.transform.position).normalized;

            return GlobalClass.RotateDirection(targetDirection, angle);
        }

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

            bullet.SetBaseBullet(0, 2.0f, targetDirection.normalized, true);

            bullet.SetUp(angle, CheckBoom);

            if (attackType == AttackType.PurpleCircle)
            {
                bullet.MoveType = SinCurve;
            }
            else
            {
                bullet.MoveType = null;
            }
        }
    }
}