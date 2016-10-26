using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Fake.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public Canvas HPBar;

        public delegate void EnemyAttackTypeDelegate(Transform tr, EnemyAttackKinds.AttackType type, float bulletSpeed);

        public enum EnemyType
        {
            SmallEnemy1,
            SmallEnemy2,
            SmallEnemy3
        }

        public EnemyType EnemyTypeCheck;

        public Image GreenHpBar;

        private BaseBullet bulletType;

        private Transform transformCache;

        private int maxHP = 10;
        private int currentHP;

        void Start()
        {
            transformCache = GetComponent<Transform>();
            currentHP = maxHP;

            CreateHPbar();

            StartCoroutine(AttackPlayer());
        }

        private IEnumerator AttackPlayer()
        {
            //     System.Action<Transform, EnemyAttackType.AttackType , float> attack;
            EnemyAttackTypeDelegate attack;
            if (EnemyTypeCheck == EnemyType.SmallEnemy1)
            {
                attack = EnemyAttackKinds.Instance.FireConeType;
                StartCoroutine(StartAttack(2.0f, 2.0f, EnemyAttackKinds.AttackType.RedAttack, attack));
            }
            else if (EnemyTypeCheck == EnemyType.SmallEnemy2)
            {
                attack = EnemyAttackKinds.Instance.FireBoomType;
                StartCoroutine(StartAttack(2.5f, Random.Range(1.0f, 2.0f), EnemyAttackKinds.AttackType.BlueAttack, attack));
            }
            else if (EnemyTypeCheck == EnemyType.SmallEnemy3)
            {
                attack = EnemyAttackKinds.Instance.FireSinType;
                yield return new WaitForSeconds(0.5f);
                attack(transform, EnemyAttackKinds.AttackType.PurpleCircle, 4.0f);
            }
        }

        private IEnumerator StartAttack(float attackTime, float bulletSpeed, EnemyAttackKinds.AttackType attackType, EnemyAttackTypeDelegate attack)
        {
            while (true)
            {
                yield return new WaitForSeconds(attackTime); // 공격 텀
                attack(transform, attackType, bulletSpeed);
            }
        }

        private void CreateHPbar()
        {
            var hp = Instantiate(HPBar);
            var hpTransform = hp.GetComponent<Transform>();
            hpTransform.parent = transformCache;
            hpTransform.position = transformCache.localPosition + Vector3.up;

            GreenHpBar = hp.GetComponentInChildren<Image>();
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.GetComponent<BaseBullet>() != null)
            {
                bulletType = coll.gameObject.GetComponent<BaseBullet>();

                if (bulletType.BulletTypeCheck == BulletType.PlayerBullet)
                {
                    Destroy(coll.gameObject);
                    currentHP--;
                }
            }

            GreenHpBar.fillAmount = (float)currentHP / (float)maxHP;

            if (currentHP < 0)
            {
                KillEnemy();
            }
        }

        private void KillEnemy()
        {
            Item.ItemSpawn.Instance.SpawnItem(transform, Item.ItemSpawn.ItemTypeObject.PowerItem);
            Destroy(gameObject);
        }
    }
}