using UnityEngine;
using System.Collections;

namespace Fake
{
    public delegate void PlayerRespawnEventHandler(GameObject player);
    public delegate void PlayerDeadEventHandler();

    public class ObjectCreator : MonoBehaviour
    {
        public event PlayerRespawnEventHandler PlayerRespawn;
        public event PlayerDeadEventHandler PlayerDead;

        public GameObject PlayerObject;
        public GameObject Boss;
        public GameObject EnemyObject;
        public GameObject EnemyAttackKindsObject;

        private Vector2 PlayerSpawnPoint;
        private Player.PlayerController player;

        private Transform transformCache;

        private float radius;

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

        private void CreateEnemyAttackKinds()
        {
            Debug.Log("Creating attackkidns");
            GameObject enemyAttackKindsObject = Instantiate(EnemyAttackKindsObject);
            Enemy.EnemyAttackKinds set = enemyAttackKindsObject.GetComponent<Enemy.EnemyAttackKinds>();

            enemyAttackKindsObject.transform.localPosition = Vector2.zero;
            enemyAttackKindsObject.transform.localRotation = Quaternion.identity;
            enemyAttackKindsObject.transform.localScale = Vector2.one;

            set.SetObjectCreatorCache(this);
        }

        public void CreatePlayer()
        {
            Debug.Log("Creating player");
            GameObject playerObject = Instantiate(PlayerObject);
            Player.PlayerController set = playerObject.GetComponent<Player.PlayerController>();

            playerObject.transform.localPosition = PlayerSpawnPoint;
            playerObject.transform.localRotation = Quaternion.identity;
            playerObject.transform.localScale = Vector2.one;

            set.Setup(3, 1, 3); // life, power, boom

            PlayerRespawnEvent(playerObject); // 여기서 멈추는 이유?
            player = set;
            if (player != null)
                player.PlayerDead += PlayerDeadEvent;

            GameManager.Instance.PlayerTransform = playerObject.transform;
        }

        public void CreateEnemy()
        {
            var enemyObject = Instantiate(EnemyObject);
            var set = enemyObject.GetComponent<Enemy.EnemyController>();
            var mover = enemyObject.GetComponent<Mover>();

            enemyObject.transform.localPosition = new Vector2(2, 0);
            enemyObject.transform.localRotation = Quaternion.identity;
            enemyObject.transform.localScale = Vector2.one;

            set.SetEventCache(this);
            set.EnemyTypeCheck = Enemy.EnemyController.EnemyType.SmallEnemy1; // 수정예정
            mover.SetBezierCurve(new Vector2(2, -3), new Vector2(-10, 0), new Vector2(2, 3));
        }

        private void PlayerRespawnEvent(GameObject player)
        {
            PlayerRespawn(player); // Event 오류 질문
        }

        private void PlayerDeadEvent()
        {
            PlayerDead(); // Event
        }
    }
}