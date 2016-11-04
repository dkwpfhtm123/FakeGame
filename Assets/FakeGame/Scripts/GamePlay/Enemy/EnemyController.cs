using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Fake.Enemy
{
    public delegate void EnemyAttackTypeDelegate(Transform tr, EnemyAttackKinds.AttackType type, float bulletSpeed);

    public class EnemyController : MonoBehaviour
    {
        public enum EnemyType
        {
            SmallEnemy1,
            SmallEnemy2,
            SmallEnemy3
        }

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

                if(objectCreator != null)
                {
                    objectCreator.OnPlayerRespawn += OnPlayerRespawn;
                    objectCreator.OnPlayerDead += OnPlayerDead;

                    OnPlayerRespawn();
                }
            }
        }
        #endregion

        #region varible
        public Canvas HPBar;

        public EnemyType EnemyTypeCheck;
        public Image GreenHpBar;

        private ObjectCreator objectCreator;
        private BaseBullet bulletType;
        private Transform transformCache;
        private int maxHP = 10;
        private int currentHP;
        private IEnumerator attckCoroutine;

        private bool playerDead;
        #endregion

        void Start()
        {
            transformCache = GetComponent<Transform>();
            currentHP = maxHP;

            playerDead = false;

            CreateHPbar();

            StartCoroutine(AttackPlayer());
        }

        private IEnumerator AttackPlayer()
        {
            EnemyAttackTypeDelegate attack;
            if (EnemyTypeCheck == EnemyType.SmallEnemy1)
            {
                attack = EnemyAttackKinds.Instance.FireConeType;
                StartCoroutine(StartAttack(new EnemyStartAttackParams(2.0f, 2.0f, EnemyAttackKinds.AttackType.RedAttack, attack)));
            }
            else if (EnemyTypeCheck == EnemyType.SmallEnemy2)
            {
                attack = EnemyAttackKinds.Instance.FireBoomType;
                StartCoroutine(StartAttack(new EnemyStartAttackParams(2.5f, Random.Range(1.0f, 2.0f), EnemyAttackKinds.AttackType.BlueAttack, attack)));
            }
            else if (EnemyTypeCheck == EnemyType.SmallEnemy3)
            {
                attack = EnemyAttackKinds.Instance.FireSinType;
                yield return new WaitForSeconds(0.5f);
                attack(transform, EnemyAttackKinds.AttackType.PurpleCircle, 4.0f);
            }
        }

        private void OnPlayerDead()
        {
            playerDead = true;

            if (attckCoroutine != null)
            {
                StopCoroutine(attckCoroutine);
                attckCoroutine = null;
            }
        }

        private void OnPlayerRespawn()
        {
            playerDead = false;

            if (attckCoroutine == null)
            {
     //           attckCoroutine = StartAttack();
                StartCoroutine(attckCoroutine);
            }
        }

        private IEnumerator StartAttack(EnemyStartAttackParams parameters)
        {
            while(playerDead == false)
            {
                yield return new WaitForSeconds(parameters.AttackTime); // 공격 텀
                parameters.Attack(transform, parameters.AttackType, parameters.BulletSpeed);
            } // 플레이어가 리스폰했을때 다시 공격하도록 수정
        }

        private void CreateHPbar()
        {
            var hp = Instantiate(HPBar);
            var hpTransform = hp.GetComponent<Transform>();
            hpTransform.SetParent(transformCache);
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

                GreenHpBar.fillAmount = currentHP / maxHP;

                if (currentHP < 0)
                {
                    KillEnemy();
                }
            }
        }

        private void KillEnemy()
        {
            Item.ItemSpawn.Instance.SpawnItem(transform, Item.ItemSpawn.ItemTypeObject.PowerItem);
            Destroy(gameObject);
        }
    }
}