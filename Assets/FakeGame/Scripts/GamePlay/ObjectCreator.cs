using UnityEngine;
using System.Collections;

namespace Fake
{
    public delegate void EmptyEventHandler();

    public class ObjectCreator : MonoBehaviour
    {
        #region event
        public event EmptyEventHandler OnPlayerRespawn;
        public event EmptyEventHandler OnPlayerDead;
        #endregion

        public GameObject LivePlayer
        {
            get { return livePlayer; }
            set { livePlayer = value; }
        }

        #region variable
        public GameObject PlayerObject;
        public GameObject Boss;
        public GameObject EnemyObject;
        public GameObject EnemyAttackKindsObject;

        private GameObject livePlayer;

        private Vector2 PlayerSpawnPoint;
        private Player.PlayerController player;

        private Transform transformCache;

        private float radius;
        #endregion

        void Start()
        {
            radius = 5.0f;
            transformCache = GetComponent<Transform>();
            PlayerSpawnPoint = new Vector2(transformCache.localPosition.x, transformCache.localPosition.y - radius * 0.5f);

            StartCoroutine(StartGame());
        }

        private IEnumerator StartGame()
        {
            CreatePlayer();
            CreateEnemyAttackKinds();
            yield return new WaitForSeconds(3.0f); // 대기시간 이후 적생성
        }

        #region CreateMethod
        private void CreateEnemyAttackKinds()
        {
            Debug.Log("Creating attackkidns");
            var enemyAttackKindsObject = Instantiate(EnemyAttackKindsObject);
            var setting = enemyAttackKindsObject.GetComponent<Enemy.EnemyAttackKinds>();

            enemyAttackKindsObject.transform.localPosition = Vector2.zero;
            enemyAttackKindsObject.transform.localRotation = Quaternion.identity;
            enemyAttackKindsObject.transform.localScale = Vector2.one;

            setting.ObjectCreator = this;
        }

        public void CreatePlayer()
        {
            Debug.Log("Creating player");
            var playerObject = Instantiate(PlayerObject);
            var setting = playerObject.GetComponent<Player.PlayerController>();

            playerObject.transform.localPosition = PlayerSpawnPoint;
            playerObject.transform.localRotation = Quaternion.identity;
            playerObject.transform.localScale = Vector2.one;

            setting.Setup(3, 1, 3); // life, power, boom

            PlayerRespawnEvent(); // 여기서 멈추는 이유?
            player = setting;
            if (player != null)
                player.PlayerDead += PlayerDeadEvent;

            LivePlayer = playerObject;

            GameManager.Instance.PlayerTransform = playerObject.transform;
        }

        public void CreateEnemy()
        {
            var enemyObject = Instantiate(EnemyObject);
            var setting = enemyObject.GetComponent<Enemy.EnemyController>();
            var mover = enemyObject.GetComponent<Mover>();

            enemyObject.transform.localPosition = new Vector2(2, 0);
            enemyObject.transform.localRotation = Quaternion.identity;
            enemyObject.transform.localScale = Vector2.one;

            setting.ObjectCreator = this;
            setting.EnemyTypeCheck = Enemy.EnemyController.EnemyType.SmallEnemy1; // 수정예정
            mover.SetBezierCurve(new Vector2(2, -3), new Vector2(-10, 0), new Vector2(2, 3));
        }
        #endregion

        private void PlayerRespawnEvent()
        {
            var playerRespawn = OnPlayerRespawn;
            if (playerRespawn != null)
                playerRespawn();
        }

        private void PlayerDeadEvent()
        {
            OnPlayerDead(); // Event
        }
    }
}