using UnityEngine;
using System.Collections;

namespace Fake
{
    public class ObjectCreator : MonoBehaviour
    {
        public GameObject PlayerObject;
        public GameObject Boss;
        public GameObject EnemyObject;

        private Vector2 PlayerSpawnPoint;

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
            var enemyObject = Instantiate(EnemyObject);
            var set = enemyObject.GetComponent<Enemy.EnemyController>();
            var direct = enemyObject.GetComponent<Mover>();

            enemyObject.transform.localPosition = new Vector2(2, 0);
            enemyObject.transform.localRotation = Quaternion.identity;
            enemyObject.transform.localScale = Vector2.one;

            set.EnemyTypeCheck = Enemy.EnemyController.EnemyType.SmallEnemy1; // 수정예정
            direct.SetBezierCurve(new Vector2(2, -3), new Vector2(-10, 0), new Vector2(2, 3));
        }
    }
}