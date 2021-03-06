﻿using UnityEngine;
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

        public Player.PlayerController LivePlayerController
        {
            get { return livePlayerController; }
            private set { livePlayerController = value; }
        }

        public bool PlayerBooming
        {
            get { return playerBooming; }
            private set { playerBooming = value; }
        }

        #region variable
        public GameObject PlayerObject;
        public GameObject Boss;
        public GameObject EnemyObject;
        public GameObject EnemyAttackKinds;

        private Player.PlayerController livePlayerController;
        private bool playerBooming;
        private Enemy.EnemyAttackKinds enemyAttackKinds;

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
            Debug.Log("Creating attackkinds");
            var enemyAttackKinds = Instantiate(EnemyAttackKinds);
            var setup = enemyAttackKinds.GetComponent<Enemy.EnemyAttackKinds>();

            enemyAttackKinds.transform.localPosition = Vector2.zero;
            enemyAttackKinds.transform.localRotation = Quaternion.identity;
            enemyAttackKinds.transform.localScale = Vector2.one;

            setup.ObjectCreator = this;
            this.enemyAttackKinds = setup;
        }

        public void CreatePlayer()
        {
            Debug.Log("Creating player");
            var playerObject = Instantiate(PlayerObject);
            var playerController = playerObject.GetComponent<Player.PlayerController>();

            playerObject.transform.localPosition = PlayerSpawnPoint;
            playerObject.transform.localRotation = Quaternion.identity;
            playerObject.transform.localScale = Vector2.one;

            playerController.Setup(3, 1, 3); // life, power, boom

            LivePlayerController = playerController;

            OnPlayerRespawnEvent();
            player = playerController;
            if (player != null)
            {
                player.PlayerDead -= OnPlayerDeadEvent;
                player.OnBoomStart -= OnPlayerStartBoom;   //// 이부분도 -=를 해야하는지 질문
                player.OnBoomEnd -= OnPlayerEndBoom;

                player.PlayerDead += OnPlayerDeadEvent;
                player.OnBoomStart += OnPlayerStartBoom;
                player.OnBoomEnd += OnPlayerEndBoom;
            }


            GameManager.Instance.PlayerTransform = playerObject.transform;
        }

        public void CreateEnemy()
        {
            var enemyObject = Instantiate(EnemyObject);
            var setup = enemyObject.GetComponent<Enemy.EnemyController>();
            var mover = enemyObject.GetComponent<Mover>();

            enemyObject.transform.localPosition = new Vector2(2, 0);
            enemyObject.transform.localRotation = Quaternion.identity;
            enemyObject.transform.localScale = Vector2.one;

            setup.ObjectCreator = this;
            setup.EnemyAttackKinds = enemyAttackKinds;
            setup.EnemyTypeCheck = Enemy.EnemyController.EnemyType.SmallEnemy1; // 수정예정
            mover.SetBezierCurve(new Vector2(2, -3), new Vector2(-10, 0), new Vector2(2, 3));
        }
        #endregion

        #region OnEvent
        private void OnPlayerRespawnEvent()
        {
            var playerRespawn = OnPlayerRespawn;
            if (playerRespawn != null)
                playerRespawn();
        }

        private void OnPlayerDeadEvent()
        {
            OnPlayerDead(); // Event
        }

        private void OnPlayerStartBoom()
        {
            playerBooming = true;
        }

        private void OnPlayerEndBoom()
        {
            playerBooming = false;
        }
        #endregion
    }
}