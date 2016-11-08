using UnityEngine;
using System.Collections;

namespace Fake.Enemy
{
    public class EnemyBullet : BaseBullet
    {
        #region property
        public float Angle
        {
            get;
            private set;
        }

        public Player.PlayerController TargetPlayerController
        {
            get { return target; }
            private set
            {
                if (target != value)
                {
                    if (target != null)
                    {
                        target.PlayerDead -= OnPlayerDead;
                        target.OnBoomStart -= OnStartPlayerBoom;
                        target.OnBoomEnd -= OnEndPlayerBoom;
                    }

                    target = value;

                    if (target != null)
                    {
                        target.PlayerDead += OnPlayerDead;
                        target.OnBoomStart += OnStartPlayerBoom;
                        target.OnBoomEnd += OnEndPlayerBoom;
                    }
                }
            }
        }

        public ObjectCreator ObjectCreator
        {
            get { return objectCreator; }
            set
            {
                if (objectCreator != null)
                {
         //           objectCreator.OnPlayerRespawn -= OnPlayerRespawn;
                }

                objectCreator = value;

                if (objectCreator != null)
                {
        //            objectCreator.OnPlayerRespawn += OnPlayerRespawn;
                }
            }
        }
        #endregion

        #region varible
        public delegate Vector2 MoveTypeDelegate(float deltaTime);
        public MoveTypeDelegate MoveType;

        public EnemyAttackKinds EnemyAttackKinds;

        private Transform transformCache;
        private BaseBullet bulletType;
        private float currentTime;
        private Vector2 startingPosition;
        private Player.PlayerController target;
        private ObjectCreator objectCreator;
        private EnemyStartAttackParams parameters;

        private bool booming;
        private bool playerDead;
        #endregion

        #region Start,Update
        void Start()
        {
            transformCache = GetComponent<Transform>();
            bulletType = gameObject.GetComponent<BaseBullet>();
            currentTime = 0;
            startingPosition = transformCache.localPosition;

            booming = false;
            playerDead = false;

            var angle = GetRotation();
            transform.localRotation = Quaternion.Euler(0, 0, angle);

            if (bulletType.BulletTypeCheck == BulletType.BlueKnife)
            {
                StartCoroutine(BoomBullet());
            }
        }

        void Update()
        {
            if (playerDead == false)
            {
                MoveBullet();
            }

            if (booming == true)
            {
                Destroy(gameObject);
            }
        }
        #endregion

        public void SetUp(float angle, ObjectCreator objectCreatorCache, EnemyAttackKinds enemyAttackKinds)
        {
            Angle = angle;
            ObjectCreator = objectCreatorCache;
            EnemyAttackKinds = enemyAttackKinds;
            // 싱글턴 변수 만들어서 싱글턴 넣기
        }

        #region eventMethod

        private void OnDestroy()
        {
            Item.ItemSpawn.Instance.SpawnItem(transformCache, Item.ItemSpawn.ItemTypeObject.ScoreItem);
        }

        private void OnPlayerDead()
        {
            TargetPlayerController = null;
            Destroy(gameObject);
        }

        private void OnStartPlayerBoom()
        {
            booming = true;
        }

        private void OnEndPlayerBoom()
        {
            booming = false;
        }
        #endregion

        private float GetRotation()
        {
            var angle = -Mathf.Atan2(BulletDirection.x, BulletDirection.y) * Mathf.Rad2Deg;
            return angle;
        }

        private void MoveBullet()
        {
            if (MoveType != null) // null일때는 직선
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
            EnemyAttackTypeDelegate attack = EnemyAttackKinds.FireConeType;
            parameters = new EnemyStartAttackParams(transformCache, 2.0f, EnemyAttackKinds.AttackType.RedAttack, attack);
            attack(parameters);
            Destroy(gameObject);
        }
    }
}