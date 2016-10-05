using UnityEngine;
using System.Collections;

namespace Fake
{
    public class ObjectCreator : MonoBehaviour
    {
        public GameObject PlayerObject;
        public GameObject Boss;
        public GameObject EnemyObject;

        private BaseBullet bulletKinds;

        private Vector2 PlayerSpawnPoint;

        private Transform transformCache;

        private Vector2 leftUp;

        private float radius;

        void Start()
        {
            radius = 5.0f;
            transformCache = GetComponent<Transform>();
            bulletKinds = gameObject.GetComponent<BaseBullet>();

            leftUp = new Vector2(transformCache.localPosition.x - radius, transformCache.localPosition.y + radius);
            
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
            Player.PlayerAttackKinds kinds = playerObject.GetComponent<Player.PlayerAttackKinds>();

            playerObject.transform.localPosition = new Vector2(-3, -3);
            playerObject.transform.localRotation = Quaternion.identity;
            playerObject.transform.localScale = Vector2.one;

            set.Setup(3, 1, 3); // life, power, boom

            kinds.PlayerSlowAttack = bulletKinds.PlayerSlowAttack;
            kinds.PlayerFastAttack = bulletKinds.PlayerFastAttack;

            GameMgr.Instance.PlayerTransform = playerObject.transform;
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
            direct.Set(new Vector2(2, 0), new Vector2(-5, 0), new Vector2(-2, 5));
        }
    }
}