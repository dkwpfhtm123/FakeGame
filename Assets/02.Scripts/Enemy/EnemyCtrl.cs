using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class EnemyCtrl : MonoBehaviour
    {
        public int HP = 5;
        public delegate void EnemyAttackTypeDelegate(Transform tr, AttackKinds.AttackType type, float bulletSpeed);

        public enum EnemyType
        {
            SmallEnemy1,
            SmallEnemy2,
            SmallEnemy3
        }

        public EnemyType EnemyTypeCheck;

        private BulletTypeScript bulletType;

        void Start()
        {
            StartCoroutine(AttackPlayer());
            //   StartCoroutine(MoveEnemy()); 임시 주석상태
        }

        private IEnumerator MoveEnemy()
        {
            iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("start"), "time", 5, "easetype", iTween.EaseType.easeOutCubic));
            yield return new WaitForSeconds(2.0f);
            iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("top"), "time", 10, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.loop));
        }

        private IEnumerator AttackPlayer()
        {
            //     System.Action<Transform, EnemyAttackType.AttackType , float> attack;
            EnemyAttackTypeDelegate attack;
            if (EnemyTypeCheck == EnemyType.SmallEnemy1)
            {
                attack = AttackKinds.Instance.FireConeType;
                StartCoroutine(StartAttack(2.0f, 2.0f, AttackKinds.AttackType.RedAttack, attack));
            }
            else if (EnemyTypeCheck == EnemyType.SmallEnemy2)
            {
                attack = AttackKinds.Instance.FireBoomType;
                StartCoroutine(StartAttack(2.5f, Random.Range(1.0f, 2.0f), AttackKinds.AttackType.BlueAttack, attack));
            }
            else if (EnemyTypeCheck == EnemyType.SmallEnemy3)
            {
                attack = AttackKinds.Instance.FireSinType;
                yield return new WaitForSeconds(0.5f);
                attack(transform, AttackKinds.AttackType.PurpleCircle, 4.0f);
            }
        }

        IEnumerator StartAttack(float attackTime, float bulletSpeed, AttackKinds.AttackType attackType, EnemyAttackTypeDelegate attack)
        {
            while (true)
            {
                yield return new WaitForSeconds(attackTime); // 공격 텀
                attack(transform, attackType, bulletSpeed);
            }
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.GetComponent<BulletTypeScript>() != null)
            {
                bulletType = coll.gameObject.GetComponent<BulletTypeScript>();

                if (bulletType.BulletTypeCheck == BulletType.PlayerBullet)
                {
                    Destroy(coll.gameObject);
                    HP--;
                }
            }
            else if (coll.gameObject.GetComponent<ThisIsBoom>() != null)
            {
                // 충돌하는 동안으로 바꿔야함.
                HP--;
            }

            if (HP < 0)
            {
                KillEnemy();
            }
        }

        private void KillEnemy()
        {
            ItemSpawn.Instance.SpawnItem(transform, ItemSpawn.ItemTypeObject.PowerItem);
            Destroy(gameObject);
        }
    }
}
