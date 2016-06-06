using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Enemy
{
    public class EnemyCtrl : MonoBehaviour
    {
        public Canvas HPBar;

        public delegate void EnemyAttackTypeDelegate(Transform tr, AttackKinds.AttackType type, float bulletSpeed);

        public enum EnemyType
        {
            SmallEnemy1,
            SmallEnemy2,
            SmallEnemy3
        }

        public EnemyType EnemyTypeCheck;

        public Image GreenHpBar;

        private BulletTypeScript bulletType;

        private Transform transformCache;

        private int maxHP = 10;
        private int currentHP;

        void Start()
        {
            transformCache = GetComponent<Transform>();
            currentHP = maxHP;

            CreateHPbar();

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

        private IEnumerator StartAttack(float attackTime, float bulletSpeed, AttackKinds.AttackType attackType, EnemyAttackTypeDelegate attack)
        {
            while (true)
            {
                yield return new WaitForSeconds(attackTime); // 공격 텀
                attack(transform, attackType, bulletSpeed);
            }
        }

        private void CreateHPbar()
        {
            Canvas hp = Instantiate(HPBar);
            Transform hpTransform = hp.GetComponent<Transform>();
            hpTransform.parent = transformCache;
            hpTransform.position = transformCache.localPosition + Vector3.up;

            GreenHpBar = hp.GetComponentInChildren<Image>();
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.GetComponent<BulletTypeScript>() != null)
            {
                bulletType = coll.gameObject.GetComponent<BulletTypeScript>();

                if (bulletType.BulletTypeCheck == BulletType.PlayerBullet)
                {
                    Destroy(coll.gameObject);
                    currentHP--;
                }
            }
            else if (coll.gameObject.GetComponent<ThisIsBoom>() != null)
            {
                // 충돌하는 동안으로 바꿔야함.
                currentHP--;
            }

            GreenHpBar.fillAmount = (float) currentHP / (float) maxHP;

            if (currentHP < 0)
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
