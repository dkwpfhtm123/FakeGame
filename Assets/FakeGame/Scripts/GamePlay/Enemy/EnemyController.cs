using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Fake.Enemy
{
    public delegate void EnemyAttackTypeDelegate(EnemyStartAttackParams parameters);

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

        public EnemyAttackKinds EnemyAttackKinds;

        private ObjectCreator objectCreator;
        private BaseBullet bulletType;
        private Transform transformCache;
        private int maxHP = 10;
        private int currentHP;
        private IEnumerator attckCoroutine;
        private EnemyStartAttackParams parameters;

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
            EnemyAttackTypeDelegate attackDelegate;

            float attackTime;
            float bulletSpeed;
            EnemyAttackKinds.AttackType attackType;

            if (EnemyTypeCheck == EnemyType.SmallEnemy1)
            {
                attackDelegate = EnemyAttackKinds.FireConeType;
                bulletSpeed = 2.0f;
                attackTime = 2.0f;
                attackType = EnemyAttackKinds.AttackType.RedAttack;
            }
            else if (EnemyTypeCheck == EnemyType.SmallEnemy2)
            {
                attackDelegate = EnemyAttackKinds.FireBoomType;
                bulletSpeed = 2.0f;
                attackTime = Random.Range(1.0f, 2.0f);
                attackType = EnemyAttackKinds.AttackType.BlueAttack;
            }
            else //if (EnemyTypeCheck == EnemyType.SmallEnemy3)
            {
                attackDelegate = EnemyAttackKinds.FireSinType;

                bulletSpeed = 4.0f;
                attackTime = 2.0f;
                attackType = EnemyAttackKinds.AttackType.PurpleCircle;
                yield return new WaitForSeconds(0.5f);
                parameters = new EnemyStartAttackParams(transformCache, 4.0f, EnemyAttackKinds.AttackType.PurpleCircle, attackDelegate);
                attackDelegate(parameters);
            }

            parameters = new EnemyStartAttackParams(transformCache, bulletSpeed, attackTime, attackType, attackDelegate);
            StartCoroutine(StartAttack(parameters));
        }

        #region event
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
                if (parameters != null)
                {
                    attckCoroutine = StartAttack(parameters);
                    StartCoroutine(attckCoroutine);
                }
            }
        }

        private void OnDestroy()
        {
            // ondestroy 내에 이벤트 수정하기 // 다른 맞는곳으로 옮기기.
            Item.ItemSpawn.Instance.SpawnItem(transform, Item.ItemSpawn.ItemTypeObject.PowerItem);
        }
        #endregion

        private IEnumerator StartAttack(EnemyStartAttackParams parameters)
        {
            while(playerDead == false)
            {
                yield return new WaitForSeconds(parameters.AttackTime); // 공격 텀
                parameters.Attack(parameters);
                /// 코루틴 delegate를 받아서 enemyattackkinds의 fire함수들을 corutine 형태로 만들어서 attacktime을 넘겨주기.
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

                GreenHpBar.fillAmount = (float)currentHP / maxHP;

                if (currentHP < 0)
                {
                    KillEnemy();
                }
            }
        }

        private void KillEnemy()
        {
            Destroy(gameObject);
        }
    }
}