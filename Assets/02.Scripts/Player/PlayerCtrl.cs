using UnityEngine;
using System.Collections;

namespace Player
{
    public class PlayerCtrl : MonoBehaviour
    {
        // Boom관련 변수들
        public GameObject BoomPrefab;    // 프리팹
        public int BoomCount = 3;        // 남은 갯수
        private float boomTime = 2.0f;   // 지속시간
        private GameObject boomObject;   // 게임오브젝트 저장
        private bool boomCheck = false;  // 폭탄이 켜져있는지 체크

        private Transform transformCache;
        private float moveSpeed = 5.0f;

        private ItemTypeScript item = null;

        void Awake()
        {
            transformCache = GetComponent<Transform>();

            GameMgr.Instance.PlayerTransform = transformCache;
        }

        void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // 부활하는 중이 아닐때
            if (GameMgr.Instance.RespawnPlayer == false)
            {
                Vector2 moveDir = (Vector2.up * vertical) + (Vector2.right * horizontal);

                if (Input.GetKeyDown(KeyCode.X))
                {
                    // 폭탄이 활성화되지 않았을때 폭탄진행가능
                    if (boomCheck == false)
                    {
                        boomCheck = true;
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

                // 폭탄이 있을때 폭탄이 플레이어와 같이 움직임.
                if (boomObject != null)
                {
                    boomObject.transform.Translate(moveDir * Time.deltaTime * moveSpeed, Space.Self);
                }

                transformCache.Translate(moveDir * Time.deltaTime * moveSpeed, Space.Self);
            }
        }

        private IEnumerator BoomEvent()
        {
            boomObject = (GameObject)Instantiate(BoomPrefab, transformCache.localPosition, Quaternion.identity);
            GameMgr.Instance.Boom();
            yield return new WaitForSeconds(boomTime);
            Destroy(boomObject);
            boomCheck = false;
            BoomCount--;
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.GetComponent<ItemTypeScript>() != null)
            {
                item = coll.gameObject.GetComponent<ItemTypeScript>();

                if (item.ItemTypeCheck == ItemType.ScoreItem)
                {
                    GameMgr.Instance.AddPlayerScore();
                    Destroy(coll.gameObject);
                }

                else if (item.ItemTypeCheck == ItemType.PowerItem)
                {
                    GameMgr.Instance.AddPlayerPower();
                    Destroy(coll.gameObject);
                }
            }

            if (GameMgr.Instance.RespawnPlayer == false)
            {
                if (coll.gameObject.GetComponent<ThisIsEnemyBullet>()) // 적 탄을 구별하기위해 하나는 남겨둠.
                {
                    GameMgr.Instance.KillPlayer(gameObject);
                }
            }
        }
    }
}
