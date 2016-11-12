using UnityEngine;
using System.Collections;

namespace Fake.Player
{
    public delegate void EmptyEventHandler();

    public class PlayerController : MonoBehaviour
    {
        #region eventHandler
        public event EmptyEventHandler PlayerDead;
        public event EmptyEventHandler OnBoomStart;
        public event EmptyEventHandler OnBoomEnd;
        #endregion

        #region varible
        // Boom관련 변수들
        public GameObject BoomPrefab;    // 프리팹
        public int BoomCount;            // 남은 갯수
        public bool OnGoingBoom = false; // 폭탄이 진행중인지 체크
        private float boomTime = 2.0f;   // 지속시간
        private GameObject boomObject;   // 게임오브젝트 저장

        // Power관련 변수들
        public int PlayerPower;
        public GameObject PlayerPowerObject;
        private PowerShooter powerUp = null;    // 파워 오브젝트 순서대로 저장

        private Transform transformCache;
        private float moveSpeed = 5.0f;

        private UI.GameUI gameUI;
        private ObjectCreator restart;

        private int playerLife;

        private ItemTypeCode item = null;

        private enum PowerLife
        {
            Life,
            Power,
        }
        #endregion

        public void Setup(int life, int power, int boom)
        {
            playerLife = life;
            PlayerPower = power;
            BoomCount = boom;
        }

        void Awake()
        {
            transformCache = GetComponent<Transform>();

            gameUI = GameObject.Find("GameUI").GetComponent<UI.GameUI>();
        }

        void Start()
        {
            UI_Player(PlayerPower, PowerLife.Power);
            UI_Player(playerLife, PowerLife.Life);
        }

        void Update()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            var moveDir = (Vector2.up * vertical) + (Vector2.right * horizontal);

            if (Input.GetKeyDown(KeyCode.X))
            {
                // 폭탄이 활성화되지 않았을때 폭탄진행가능
                if (OnGoingBoom == false)
                {
                    OnGoingBoom = true;
                    StartCoroutine(BoomEvent());
                }
            }

            // 쉬프트 눌렀을때 느려짐 빨라짐
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                moveSpeed /= 2.0f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                moveSpeed *= 2.0f;
            }

            transformCache.Translate(moveDir * Time.deltaTime * moveSpeed, Space.Self);
        }

        private IEnumerator BoomEvent()
        {
            boomObject = (GameObject)Instantiate(BoomPrefab, transformCache.localPosition, Quaternion.identity);
            boomObject.transform.SetParent(transformCache, true);
            boomObject.transform.localScale *= 2.0f;
            OnGoingBoom = true;

            if (OnBoomStart != null)
                OnBoomStart();

            yield return new WaitForSeconds(boomTime);

            if (OnBoomEnd != null)
                OnBoomEnd();

            OnGoingBoom = false;
            Destroy(boomObject);
            BoomCount--;
        }

        #region PlayerHit
        private void KillPlayer()
        {
            //gameUI.AppearGameOver();
            if (PlayerDead != null)
                PlayerDead();

            Destroy(gameObject);
        }

        public void HitPlayer(GameObject player)
        {
            PlayerPower--;
            playerLife--;

            if (playerLife < 1)
            {
                KillPlayer();
            }
            else
            {
                UI_Player(PlayerPower, PowerLife.Power);
                UI_Player(playerLife, PowerLife.Life);
            }
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.GetComponent<ItemTypeCode>() != null)
            {
                item = coll.gameObject.GetComponent<ItemTypeCode>();

                if (item.ItemTypeCheck == ItemType.ScoreItem)
                {
                    gameUI.CheckScore(50);
                    Destroy(coll.gameObject);
                }
                else if (item.ItemTypeCheck == ItemType.PowerItem)
                {
                    AddPlayerPower();
                    gameUI.CheckScore(10);
                    Destroy(coll.gameObject);
                }
            }

            if (coll.gameObject.GetComponent<Enemy.EnemyBullet>() != null)
            {
                HitPlayer(gameObject);
            }
        }
        #endregion

        private void UI_Player(int num, PowerLife what)
        {
            if (what == PowerLife.Power)
            {
                gameUI.CheckPlayerPower(num);
            }
            else
            {
                gameUI.CheckPlayerLife(num);
            }
        }

        #region PlayerPower
        public void AddPlayerPower()
        {
            PlayerPower += 1;
            UI_Player(PlayerPower, PowerLife.Power);

            if (PlayerPower > 4)
            {
                PlayerPower = 4;
            }
            else {
                Debug.Log("Power : " + PlayerPower);
                PowerUp(PlayerPower);
            }
        }

        public void PowerUp(float powerLevel)
        {
            var playerPower = Instantiate(PlayerPowerObject);
            var powerCtrl = playerPower.GetComponent<PowerShooter>();

            playerPower.transform.localPosition = transform.localPosition;
            playerPower.transform.localRotation = Quaternion.identity;
            playerPower.transform.localScale = Vector3.one;

            powerCtrl.Radius = 1.0f;
            if (powerUp != null)
            {
                powerCtrl.Angle = (powerUp.Angle + 90.0f);
            }
            else
            {
                powerCtrl.Angle = 0;
            }
            powerCtrl.StartRotatePower(transformCache);

            powerUp = powerCtrl;
        }
        #endregion
    }
}