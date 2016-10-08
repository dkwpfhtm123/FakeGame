using UnityEngine;
using System.Collections;

namespace Fake
{
    public class ObjectCreator : MonoBehaviour
    {
        public GameObject PlayerObject;
        public GameObject Boss;
        public GameObject EnemyObject;

   // 베이스뷸렛 완성후 다시 활성화     private BaseBullet bulletKinds;

        private Vector2 PlayerSpawnPoint;

        private Transform transformCache;

        private float radius;

        void Start()
        {
            radius = 5.0f;
            transformCache = GetComponent<Transform>();
      // 베이스뷸렛 완성후 다시활성화      bulletKinds = gameObject.GetComponent<BaseBullet>();
            
            PlayerSpawnPoint = new Vector2(transformCache.localPosition.x, transformCache.localPosition.y - radius * 0.5f);

            StartCoroutine(StartGame());
        }

        private IEnumerator StartGame()
        {
            CreatePlayer();

            yield return new WaitForSeconds(3.0f); // 대기시간 이후 적생성
        }

        public void CreatePlayer()
        {
            GameObject playerObject = Instantiate(PlayerObject);
            Player.PlayerController set = playerObject.GetComponent<Player.PlayerController>();

            playerObject.transform.localPosition = PlayerSpawnPoint;
            playerObject.transform.localRotation = Quaternion.identity;
            playerObject.transform.localScale = Vector2.one;

            set.Setup(3, 1, 3); // life, power, boom

            GameManager.Instance.PlayerTransform = playerObject.transform;
            Enemy.EnemyAttackKinds.Instance.CheckBoom = set;
        }

        public void CreateEnemy()
        {
            GameObject enemyObject = Instantiate(EnemyObject);
            Enemy.EnemyController set = enemyObject.GetComponent<Enemy.EnemyController>();
            // 구현중       Enemy.EnemyAttackKinds kinds = enemyObject.GetComponent<Enemy.EnemyAttackKinds>();
            MoveObject direct = enemyObject.GetComponent<MoveObject>();

            enemyObject.transform.localPosition = new Vector2(2, 0);
            enemyObject.transform.localRotation = Quaternion.identity;
            enemyObject.transform.localScale = Vector2.one;

            set.EnemyTypeCheck = Enemy.EnemyController.EnemyType.SmallEnemy1; // 수정예정
            direct.Set(new Vector2(2, -3), new Vector2(-10, 0), new Vector2(2, 3));
        }
    }
}